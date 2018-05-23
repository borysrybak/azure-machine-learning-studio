using AzureML.Studio.Core.Exceptions;
using AzureML.Studio.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using AzureML.Studio.Core.Enums;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AzureML.Studio")]

namespace AzureML.Studio.Core.Services
{
    internal class ManagementService
    {
        private ApiSettingsProfile _apiSettings;
        private readonly HttpClientService _httpClientService;
        private readonly JsonSerializer _jsonSerializer;

        private static ManagementService _instance;
        internal static ManagementService Instance
        {
            get
            {
                return _instance ?? (_instance = new ManagementService());
            }
        }

        private ManagementService()
        {
            _apiSettings = ApiConfiguration.GetApiConfigurationSettings();
            _httpClientService = new HttpClientService(_apiSettings.SdkName);
            _jsonSerializer = new JsonSerializer();
        }

        internal ManagementService(ApiSettingsProfile apiSettingsProfile) : this()
        {
            ApiConfiguration.SetConfiguration(apiSettingsProfile);
        }

        #region Private Helpers
        private void ValidateWorkspaceSetting(WorkspaceSettings setting)
        {
            if (setting.Location == null || setting.Location == string.Empty) { throw new ArgumentException("No Location specified."); }
            if (setting.WorkspaceId == null || setting.WorkspaceId == string.Empty) { throw new ArgumentException("No Workspace Id specified."); }
            if (setting.AuthorizationToken == null || setting.AuthorizationToken == string.Empty) { throw new ArgumentException("No Authorization Token specified."); }
            SetApiUrl(setting.Location);
        }

        private void SetApiUrl(string location)
        {
            var key = string.Empty;
            switch (location.ToLower())
            {
                case "south central us":
                    key = "";
                    SetApiEndpoints(key, ".net");
                    break;
                case "west europe":
                    key = "europewest.";
                    SetApiEndpoints(key, ".net");
                    break;
                case "southeast asia":
                    key = "asiasoutheast.";
                    SetApiEndpoints(key, ".net");
                    break;
                case "japan east":
                    key = "japaneast.";
                    SetApiEndpoints(key, ".net");
                    break;
                case "germany central":
                    key = "germanycentral.";
                    SetApiEndpoints(key, ".de");
                    break;
                case "west central us":
                    key = "uswestcentral.";
                    SetApiEndpoints(key, ".net");
                    break;
                case "integration test":
                    key = "";
                    SetApiEndpoints(key, "-int.net");
                    break;
                default:
                    throw new Exception("Unsupported location: " + location);
            }
        }

        private void SetApiEndpoints(string key, string postfix)
        {
            _apiSettings.StudioApi = string.Format(_apiSettings.StudioApiBaseUrl, key, postfix);
            _apiSettings.WebServiceApi = string.Format(_apiSettings.WebServiceApiBaseUrl, key, postfix);
        }

        private string GetSpecificObjectFromJson(string rawJson, string jsonObject)
        {
            dynamic parsed = JsonConvert.DeserializeObject<object>(rawJson);
            var serializedJsonObject = JsonConvert.SerializeObject(parsed[jsonObject]);

            return serializedJsonObject;
        }

        private string CreateSubmitExperimentRequest(Experiment exp, string rawJson, bool runExperiment, string newName, bool createNewCopy)
        {
            var graph = GetSpecificObjectFromJson(rawJson, "Graph");
            var webService = GetSpecificObjectFromJson(rawJson, "WebService");
            var request = "{" + string.Format("\"Description\":\"{0}\", \"Summary\":\"{1}\", \"IsDraft\":" + (runExperiment ? "false" : "true") +
                ", \"ParentExperimentId\":\"{2}\", \"DisableNodeUpdate\":false, \"Category\":\"user\", \"ExperimentGraph\":{3}, \"WebService\":{4}",
                            string.IsNullOrEmpty(newName) ? exp.Description : newName, exp.Summary, createNewCopy ? null : exp.ParentExperimentId, graph, webService) + "}";

            return request;
        }

        private HttpWebRequest GetRdfeHttpRequest(string managementCertThumbprint, string reqUrl, string method)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(reqUrl);
            httpRequest.Method = method;
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("x-ms-version", "2014-10-01");
            var managementCert = GetStoreCertificate(managementCertThumbprint);
            httpRequest.ClientCertificates.Add(managementCert);

            return httpRequest;
        }

        private X509Certificate2 GetStoreCertificate(string thumbprint)
        {
            var locations = new List<StoreLocation>
            {
                StoreLocation.CurrentUser,
                StoreLocation.LocalMachine
            };

            foreach (var location in locations)
            {
                var store = new X509Store("My", location);
                try
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var certificates = store.Certificates.Find(
                      X509FindType.FindByThumbprint, thumbprint, false);
                    if (certificates.Count == 1)
                    {
                        return certificates[0];
                    }
                }
                finally
                {
                    store.Close();
                }
            }
            throw new ArgumentException(string.Format(
              "A Certificate with Thumbprint '{0}' could not be located.",
              thumbprint));
        }

        private string UpdateNodesPositions(string jsonGraph, StudioGraph graph)
        {
            dynamic experimentDag = JsonConvert.DeserializeObject<object>(jsonGraph);
            var regularNodes = ExtractNodesFromXml(experimentDag["Graph"]["SerializedClientData"]);
            var webServiceNodes = ExtractNodesFromXml(experimentDag["WebService"]["SerializedClientData"]);

            var newPositions = new StringBuilder();
            jsonGraph = ReplaceJsonGraphOldPositionWithNewPosition(jsonGraph, graph, "Graph", experimentDag, regularNodes, newPositions);
            newPositions.Clear();
            jsonGraph = ReplaceJsonGraphOldPositionWithNewPosition(jsonGraph, graph, "WebService", experimentDag, webServiceNodes, newPositions);

            return jsonGraph;
        }

        private string ReplaceJsonGraphOldPositionWithNewPosition(string jsonGraph, StudioGraph graph, string jsonObject, dynamic experimentDag, dynamic nodes, StringBuilder newPositions)
        {
            if (nodes.Count > 0)
            {
                foreach (var node in graph.Nodes.Where(n => nodes.Contains(n.Id)))
                    newPositions.Append("<NodePosition Node='" + node.Id + "' Position='" + node.CenterX + "," + node.CenterY + "," + node.Width + "," + node.Height + "'/>");
                var oldPositions = Regex.Match(experimentDag[jsonObject]["SerializedClientData"].ToString(), "<NodePositions>(.*)</NodePositions>").Groups[1].Value;
                jsonGraph = jsonGraph.Replace(oldPositions, newPositions.ToString());
            }

            return jsonGraph;
        }

        private List<string> ExtractNodesFromXml(string xml)
        {
            var nodes = new List<string>();
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            foreach (XmlNode node in xmlDocument.SelectSingleNode("//NodePositions"))
            {
                var nodeId = node.Attributes["Node"].Value;
                nodes.Add(nodeId);
            }

            return nodes;
        }

        private async Task<string> GetResultAsync(HttpWebRequest httpRequest)
        {
            var result = string.Empty;
            using (var webResponse = await httpRequest.GetResponseAsync())
            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        private async void DownloadFileAsync(string url, string filename)
        {
            var httpResult = await _httpClientService.HttpGet(url, false);
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            if (File.Exists(filename))
            {
                throw new Exception(filename + " alread exists.");
            }
            using (var fileStream = File.Create(filename))
            {
                httpResult.PayloadStream.Seek(0, SeekOrigin.Begin);
                httpResult.PayloadStream.CopyTo(fileStream);
            }
        }

        private void SubmitExperiment(WorkspaceSettings setting, Experiment experiment, string rawJson, string newName, bool createNewCopy, bool run)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var body = CreateSubmitExperimentRequest(experiment, rawJson, run, newName, createNewCopy);
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}", setting.WorkspaceId, createNewCopy ? string.Empty : experiment.Id);
            var httpResult = _httpClientService.HttpPost(queryUrl, body).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        private void InsertNodesIntoGraph(dynamic dag, StudioGraph graph, string section)
        {
            var nodePositions = dag[section]["SerializedClientData"];
            var nodes = ExtractNodesFromXml(nodePositions);
            foreach (var nodeId in nodes)
            {
                graph.Nodes.Add(new StudioGraphNode
                {
                    Id = nodeId,
                    Width = 300,
                    Height = 100,
                    UserData = nodeId
                });
            }
        }

        private StudioGraph CreateStudioGraph(dynamic dag)
        {
            var graph = new StudioGraph();
            InsertNodesIntoGraph(dag, graph, "Graph");
            InsertNodesIntoGraph(dag, graph, "WebService");
            var datasetNodes = new Dictionary<string, string>();
            foreach (var moduleNode in dag["Graph"]["ModuleNodes"])
            {
                string nodeId = moduleNode["Id"];
                foreach (var inputPort in moduleNode["InputPortsInternal"])
                {
                    if (inputPort["DataSourceId"] != null && !datasetNodes.Keys.Contains(nodeId))
                    {
                        datasetNodes.Add(nodeId, inputPort["DataSourceId"].ToString());
                    }
                }
            }

            foreach (dynamic edge in dag["Graph"]["EdgesInternal"])
            {
                var sourceOutputPort = edge["SourceOutputPortId"].ToString();
                var destinationInputPort = edge["DestinationInputPortId"].ToString();
                var sourceNode = (sourceOutputPort.Split(':')[0]);
                var destinationNode = (destinationInputPort.Split(':')[0]);
                graph.Edges.Add(new StudioGraphEdge
                {
                    DestinationNode = graph.Nodes.Single(n => n.Id == destinationNode),
                    SourceNode = graph.Nodes.Single(n => n.Id == sourceNode)
                });
            }

            foreach (var nodeId in datasetNodes.Keys)
            {
                graph.Edges.Add(new StudioGraphEdge
                {
                    DestinationNode = graph.Nodes.Single(n => n.Id == nodeId),
                    SourceNode = graph.Nodes.Single(n => n.Id == datasetNodes[nodeId])
                }
                );
            }

            if (dag["WebService"] != null)
            {
                if (dag["WebService"]["Inputs"] != null)
                {
                    foreach (var webServiceInput in dag["WebService"]["Inputs"])
                    {
                        if (webServiceInput["PortId"] != null)
                        {
                            var webSvcModuleId = webServiceInput["Id"].ToString();
                            var connectedModuleId = webServiceInput["PortId"].ToString().Split(':')[0];
                            graph.Edges.Add(new StudioGraphEdge
                            {
                                DestinationNode = graph.Nodes.Single(n => n.Id == connectedModuleId),
                                SourceNode = graph.Nodes.Single(n => n.Id == webSvcModuleId)
                            });
                        }
                    }
                }

                if (dag["WebService"]["Outputs"] != null)
                {
                    foreach (var webServiceOutput in dag["WebService"]["Outputs"])
                    {
                        if (webServiceOutput["PortId"] != null)
                        {
                            var webServiceModuleId = webServiceOutput["Id"].ToString();
                            var connectedModuleId = webServiceOutput["PortId"].ToString().Split(':')[0];
                            graph.Edges.Add(new StudioGraphEdge
                            {
                                DestinationNode = graph.Nodes.Single(n => n.Id == webServiceModuleId),
                                SourceNode = graph.Nodes.Single(n => n.Id == connectedModuleId)
                            });
                        }
                    }
                }
            }

            return graph;
        }
        #endregion

        #region Workspace
        internal WorkspaceRdfe[] GetWorkspacesFromRdfe(string managementCertThumbprint, string azureSubscriptionId)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl, azureSubscriptionId);
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "GET");
            var result = GetResultAsync(httpRequest).Result;
            var workspaces = JsonConvert.DeserializeObject<WorkspaceRdfe[]>(result);

            return workspaces;
        }

        internal string CreateWorkspace(string managementCertThumbprint, string azureSubscriptionId, string workspaceName, string location, string storageAccountName, string storageAccountKey, string ownerEmail, string source)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl + "/e582920d010646acbb0ec3183dc2243a", azureSubscriptionId);
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "PUT");
            var payload = JsonConvert.SerializeObject(new
            {
                Name = workspaceName,
                Region = location,
                StorageAccountName = storageAccountName,
                StorageAccountKey = storageAccountKey,
                OwnerId = ownerEmail,
                ImmediateActivation = true,
                Source = source
            });
            httpRequest.ContentLength = payload.Length;
            var stream = httpRequest.GetRequestStream();
            var buffer = Encoding.UTF8.GetBytes(payload);
            stream.Write(buffer, 0, buffer.Length);
            var result = GetResultAsync(httpRequest).Result;
            dynamic d = JsonConvert.DeserializeObject<object>(result);

            return d["Id"];
        }

        internal WorkspaceRdfe GetCreateWorkspaceStatus(string managementCertThumbprint, string azureSubscriptionId, string workspaceId, string region)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl + "/{1}?Region={2}", azureSubscriptionId, workspaceId, HttpUtility.HtmlEncode(region));
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "GET");
            var result = GetResultAsync(httpRequest).Result;
            var workspace = JsonConvert.DeserializeObject<WorkspaceRdfe>(result);

            return workspace;
        }

        internal void RemoveWorkspace(string managementCertThumbprint, string azureSubscriptionId, string workspaceId, string region)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl + "{1}?Region={2}", azureSubscriptionId, workspaceId, HttpUtility.HtmlEncode(region));
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "DELETE");
            var webResponse = httpRequest.GetResponse();
            var contentLength = webResponse.ContentLength;
            var buffer = new byte[contentLength];
            webResponse.GetResponseStream().Read(buffer, 0, (int)contentLength);

            var result = UnicodeEncoding.ASCII.GetString(buffer);
        }

        internal Workspace GetWorkspaceFromAmlRP(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var workspace = JsonConvert.DeserializeObject<Workspace>(httpResult.Payload);
                return workspace;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void AddWorkspaceUsers(WorkspaceSettings setting, string emails, string role)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/invitations", setting.WorkspaceId);
            var body = "{Role: \"" + role + "\", Emails:\"" + emails + "\"}";
            var httpResult = _httpClientService.HttpPost(queryUrl, body).Result;
            if (httpResult.IsSuccess)
            {
                var payload = httpResult.Payload;
                return;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal WorkspaceUser[] GetWorkspaceUsers(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/users", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var usersInternal = JsonConvert.DeserializeObject<WorkspaceUserInternal[]>(httpResult.Payload);
                var users = new List<WorkspaceUser>();
                foreach (WorkspaceUserInternal u in usersInternal)
                {
                    users.Add(new WorkspaceUser(u));
                }
                return users.ToArray();
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }
        #endregion

        #region Dataset
        internal Dataset[] GetDataset(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/datasources", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var datasets = JsonConvert.DeserializeObject<Dataset[]>(httpResult.Payload);

            return datasets;
        }

        internal void DeleteDataset(WorkspaceSettings setting, string datasetFamilyId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/datasources/family/{1}", setting.WorkspaceId, datasetFamilyId);
            var httpResult = _httpClientService.HttpDelete(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void DownloadDatasetAsync(WorkspaceSettings setting, string datasetId, string filename)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/datasources/{1}", setting.WorkspaceId, datasetId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var dataset = JsonConvert.DeserializeObject<Dataset>(httpResult.Payload);
            var downloadUrl = dataset.DownloadLocation.BaseUri + dataset.DownloadLocation.Location + dataset.DownloadLocation.AccessCredential;
            DownloadFileAsync(downloadUrl, filename);
        }

        internal async Task<string> UploadResourceAsync(WorkspaceSettings setting, string fileFormat, string fileName)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("resourceuploads/workspaces/{0}/?userStorage=true&dataTypeId={1}", setting.WorkspaceId, fileFormat);
            var httpResult = await _httpClientService.HttpPostFile(queryUrl, fileName);
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }

            return httpResult.Payload;
        }

        internal string UploadResource(WorkspaceSettings setting, string fileFormat)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("resourceuploads/workspaces/{0}/?userStorage=true&dataTypeId={1}", setting.WorkspaceId, fileFormat);
            var httpResult = _httpClientService.HttpPost(queryUrl, string.Empty).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }

            return httpResult.Payload;
        }

        internal async Task<string> UploadResourceInChunksAsnyc(WorkspaceSettings setting, int numOfBlocks, int blockId, string uploadId, string fileName, string fileFormat)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("blobuploads/workspaces/{0}/?numberOfBlocks={1}&blockId={2}&uploadId={3}&dataTypeId={4}",
                setting.WorkspaceId, numOfBlocks, blockId, uploadId, fileFormat);
            var httpResult = await _httpClientService.HttpPostFile(queryUrl, fileName);
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }

            return httpResult.Payload;
        }

        internal string StartDatasetSchemaGen(WorkspaceSettings setting, string dataTypeId, string uploadFileId, string datasetName, string description, string uploadFileName)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            dynamic schemaJob = new
            {
                DataSource = new
                {
                    Name = datasetName,
                    DataTypeId = dataTypeId,
                    Description = description,
                    FamilyId = string.Empty,
                    Owner = "PowerShell",
                    SourceOrigin = "FromResourceUpload"
                },
                UploadId = uploadFileId,
                UploadedFromFileName = Path.GetFileName(uploadFileName),
                ClientPoll = true
            };
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/datasources", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpPost(queryUrl, JsonConvert.SerializeObject(schemaJob)).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var dataSourceId = httpResult.Payload.Replace("\"", "");

            return dataSourceId;
        }

        internal string GetDatasetSchemaGenStatus(WorkspaceSettings setting, string dataSourceId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/datasources/{1}", setting.WorkspaceId, dataSourceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            dynamic parsed = JsonConvert.DeserializeObject<object>(httpResult.Payload);
            var schemaJobStatus = parsed["SchemaStatus"];

            return schemaJobStatus;
        }
        #endregion

        #region Custom Module
        internal string BeginParseCustomModuleJob(WorkspaceSettings setting, string moduleUploadMetadata)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/modules/custom", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpPost(queryUrl, moduleUploadMetadata).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var activityId = httpResult.Payload.Replace("\"", "");

            return activityId;
        }

        internal string GetCustomModuleBuildJobStatus(WorkspaceSettings setting, string activityGroupId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/modules/custom?activityGroupId={1}", setting.WorkspaceId, activityGroupId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var jobStatus = httpResult.Payload;

            return jobStatus;
        }

        internal Module[] GetModules(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/modules", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
            var modules = JsonConvert.DeserializeObject<Module[]>(httpResult.Payload);
            return modules;
        }
        #endregion

        #region Experiment
        internal Experiment[] GetExperiments(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var experiments = JsonConvert.DeserializeObject<Experiment[]>(httpResult.Payload);
                experiments = experiments.Where(e => e.Category == "user" || string.IsNullOrEmpty(e.Category)).ToArray();
                return experiments;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal Experiment GetExperimentById(WorkspaceSettings setting, string experimentId, out string rawJson)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            rawJson = string.Empty;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}", setting.WorkspaceId, experimentId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                rawJson = httpResult.Payload;
                var experiment = JsonConvert.DeserializeObject<Experiment>(httpResult.Payload);
                return experiment;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void RunExperiment(WorkspaceSettings setting, Experiment exp, string rawJson)
        {
            SubmitExperiment(setting, exp, rawJson, string.Empty, false, true);
        }

        internal void SaveExperiment(WorkspaceSettings setting, Experiment exp, string rawJson)
        {
            SubmitExperiment(setting, exp, rawJson, string.Empty, false, false);
        }

        internal void SaveExperimentAs(WorkspaceSettings setting, Experiment exp, string rawJson, string newName)
        {
            SubmitExperiment(setting, exp, rawJson, newName, true, false);
        }

        internal void RemoveExperimentById(WorkspaceSettings setting, string ExperimentId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}?deleteAncestors=true", setting.WorkspaceId, ExperimentId);
            var httpResult = _httpClientService.HttpDelete(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal PackingServiceActivity PackExperiment(WorkspaceSettings setting, string experimentId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/packages?api-version=2.0&experimentid={1}/&clearCredentials=true&includeAuthorId=false", setting.WorkspaceId, experimentId);
            var httpResult = _httpClientService.HttpPost(queryUrl, string.Empty).Result;
            if (httpResult.IsSuccess)
            {
                var activity = JsonConvert.DeserializeObject<PackingServiceActivity>(httpResult.Payload);
                return activity;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal PackingServiceActivity GetActivityStatus(WorkspaceSettings setting, string activityId, bool isPacking)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/packages?{1}ActivityId={2}", setting.WorkspaceId, (isPacking ? "package" : "unpack"), activityId);
            var httpResult = _httpClientService.HttpGet(queryUrl, true).Result;
            if (httpResult.IsSuccess)
            {
                var activity = JsonConvert.DeserializeObject<PackingServiceActivity>(httpResult.Payload);
                return activity;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal PackingServiceActivity UnpackExperiment(WorkspaceSettings setting, string packedLocation, string sourceRegion)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/packages?api-version=2.0&packageUri={1}{2}", setting.WorkspaceId, HttpUtility.UrlEncode(packedLocation), "&region=" + sourceRegion.Replace(" ", string.Empty));
            var httpResult = _httpClientService.HttpPut(queryUrl, string.Empty).Result;
            if (httpResult.IsSuccess)
            {
                var activity = JsonConvert.DeserializeObject<PackingServiceActivity>(httpResult.Payload);
                return activity;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal PackingServiceActivity UnpackExperimentFromGallery(WorkspaceSettings setting, string packageUri, string galleryUrl, string entityId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/packages?api-version=2.0&packageUri={1}&communityUri={2}&entityId={3}", setting.WorkspaceId, HttpUtility.UrlEncode(packageUri), HttpUtility.UrlEncode(galleryUrl), entityId);
            var httpResult = _httpClientService.HttpPut(setting.AuthorizationToken, queryUrl, string.Empty).Result;
            if (httpResult.IsSuccess)
            {
                var activity = JsonConvert.DeserializeObject<PackingServiceActivity>(httpResult.Payload);
                return activity;
            }
            else
            {

                throw new AmlRestApiException(httpResult);
            }
        }

        internal string ExportAmlWebServiceDefinitionFromExperiment(WorkspaceSettings setting, string experimentId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}/webservicedefinition", setting.WorkspaceId, experimentId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                return httpResult.Payload;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal string AutoLayoutGraph(string jsonGraph)
        {
            var studioGraph = CreateStudioGraph(JsonConvert.DeserializeObject<object>(jsonGraph));
            var httpResult = _httpClientService.HttpPost(_apiSettings.GraphLayoutApi + "AutoLayout", JsonConvert.SerializeObject(studioGraph)).Result;
            if (httpResult.IsSuccess)
            {
                studioGraph = JsonConvert.DeserializeObject<StudioGraph>(httpResult.Payload);
                var serializedGraph = JsonConvert.SerializeObject(studioGraph);
                jsonGraph = UpdateNodesPositions(jsonGraph, studioGraph);
                return jsonGraph;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }
        #endregion

        #region User Assets
        internal UserAsset[] GetTrainedModels(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/trainedmodels", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var trainedModels = JsonConvert.DeserializeObject<UserAsset[]>(httpResult.Payload);
                return trainedModels;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal UserAsset[] GetTransforms(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/transformmodules", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var transformsModels = JsonConvert.DeserializeObject<UserAsset[]>(httpResult.Payload);
                return transformsModels;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void PromoteUserAsset(WorkspaceSettings setting, string experimentId, string nodeId, string nodeOutputName, string assetName, string assetDescription, UserAssetType assetType, string familyId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/{1}", setting.WorkspaceId, assetType == UserAssetType.Transform ? "transformmodules" : (assetType == UserAssetType.TrainedModel ? "trainedmodels" : "datasources"));
            var postPayloadInJson = string.Empty;
            switch (assetType)
            {
                case UserAssetType.Transform:
                    var transformPayload = new
                    {
                        ExperimentId = experimentId,
                        ModuleNodeId = nodeId,
                        OutputName = nodeOutputName,
                        Transform = new
                        {
                            Name = assetName,
                            DataTypeId = "iTransformDotNet",
                            Description = assetDescription,
                            SourceOrigin = "FromOutputPromotion",
                            FamilyId = familyId
                        }
                    };
                    postPayloadInJson = JsonConvert.SerializeObject(transformPayload);
                    break;
                case UserAssetType.TrainedModel:
                    var trainedModelPayload = new
                    {
                        ExperimentId = experimentId,
                        ModuleNodeId = nodeId,
                        OutputName = nodeOutputName,
                        TrainedModel = new
                        {
                            Name = assetName,
                            DataTypeId = "iLearnerDotNet",
                            Description = assetDescription,
                            SourceOrigin = "FromOutputPromotion",
                            FamilyId = familyId
                        }
                    };
                    postPayloadInJson = JsonConvert.SerializeObject(trainedModelPayload);
                    break;
                case UserAssetType.Dataset:
                    var datasetPayload = new
                    {
                        ExperimentId = experimentId,
                        ModuleNodeId = nodeId,
                        OutputName = nodeOutputName,
                        DataSource = new
                        {
                            Name = assetName,
                            DataTypeId = "Dataset",
                            Description = assetDescription,
                            SourceOrigin = "FromOutputPromotion",
                            FamilyId = familyId
                        }
                    };
                    postPayloadInJson = JsonConvert.SerializeObject(datasetPayload);
                    break;
            }
            var httpResult = _httpClientService.HttpPost(queryUrl, postPayloadInJson).Result;
            if (httpResult.IsSuccess)
            {
                return;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }
        #endregion

        #region Web Service
        internal WebService[] GetWebServicesInWorkspace(WorkspaceSettings setting)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices", setting.WorkspaceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var WorkspaceWebServices = JsonConvert.DeserializeObject<WebService[]>(httpResult.Payload);
                return WorkspaceWebServices;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal WebService GetWebServicesById(WorkspaceSettings setting, string webServiceId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}", setting.WorkspaceId, webServiceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var webService = JsonConvert.DeserializeObject<WebService>(httpResult.Payload);
                return webService;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal WebServiceCreationStatus DeployWebServiceFromPredictiveExperiment(WorkspaceSettings setting, string predictiveExperimentId, bool updateExistingWebServiceDefaultEndpoint)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}/webservice?generateNewPortNames=false{2}", setting.WorkspaceId, predictiveExperimentId, updateExistingWebServiceDefaultEndpoint ? "&updateExistingWebService=true" : "");
            var httpResult = _httpClientService.HttpPost(queryUrl, string.Empty).Result;
            if (httpResult.IsSuccess)
            {
                var status = JsonConvert.DeserializeObject<WebServiceCreationStatus>(httpResult.Payload);
                return status;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal WebServiceCreationStatus GetWebServiceCreationStatus(WorkspaceSettings setting, string activityId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.StudioApi + string.Format("workspaces/{0}/experiments/{1}/webservice", setting.WorkspaceId, activityId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var status = JsonConvert.DeserializeObject<WebServiceCreationStatus>(httpResult.Payload);
                return status;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void RemoveWebServiceById(WorkspaceSettings setting, string webServiceId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}", setting.WorkspaceId, webServiceId);
            var httpResult = _httpClientService.HttpDelete(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }
        #endregion

        #region Web Service Endpoint
        internal WebServiceEndpoint[] GetWebServiceEndpoints(WorkspaceSettings setting, string webServiceId)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints", setting.WorkspaceId, webServiceId);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var webServiceEndpoints = JsonConvert.DeserializeObject<WebServiceEndpoint[]>(httpResult.Payload);
                return webServiceEndpoints;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal WebServiceEndpoint GetWebServiceEndpointByName(WorkspaceSettings setting, string webServiceId, string endpointName)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints/{2}", setting.WorkspaceId, webServiceId, endpointName);
            var httpResult = _httpClientService.HttpGet(queryUrl).Result;
            if (httpResult.IsSuccess)
            {
                var endpoint = JsonConvert.DeserializeObject<WebServiceEndpoint>(httpResult.Payload);
                return endpoint;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void AddWebServiceEndpoint(WorkspaceSettings setting, AddWebServiceEndpointRequest request)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints/{2}", setting.WorkspaceId, request.WebServiceId, request.EndpointName);
            var body = JsonConvert.SerializeObject(request);
            var httpResult = _httpClientService.HttpPut(queryUrl, body).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal bool RefreshWebServiceEndpoint(WorkspaceSettings setting, string webServiceId, string endpointName, bool overwriteResources)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints/{2}/refresh", setting.WorkspaceId, webServiceId, endpointName);
            var body = "{\"OverwriteResources\": \"" + overwriteResources.ToString() + "\"}";
            var httpResult = _httpClientService.HttpPost(queryUrl, body).Result;
            if (httpResult.StatusCode == 304)
            {
                return false;
            }
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }

            return true;
        }

        internal void PatchWebServiceEndpoint(WorkspaceSettings setting, string webServiceId, string endpointName, dynamic patchReq)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var body = JsonConvert.SerializeObject(patchReq);
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints/{2}", setting.WorkspaceId, webServiceId, endpointName);
            var httpResult = _httpClientService.HttpPatch(queryUrl, body).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal void RemoveWebServiceEndpoint(WorkspaceSettings setting, string webServiceId, string endpointName)
        {
            ValidateWorkspaceSetting(setting);
            _httpClientService.AuthorizationToken = setting.AuthorizationToken;
            var queryUrl = _apiSettings.WebServiceApi + string.Format("workspaces/{0}/webservices/{1}/endpoints/{2}", setting.WorkspaceId, webServiceId, endpointName);
            var httpResult = _httpClientService.HttpDelete(queryUrl).Result;
            if (!httpResult.IsSuccess)
            {
                throw new AmlRestApiException(httpResult);
            }
        }
        #endregion

        #region Invoke Web Service Endpoint
        internal string InvokeRequestResponseService(string PostRequestUrl, string apiKey, string input)
        {
            _httpClientService.AuthorizationToken = apiKey;
            var httpResult = _httpClientService.HttpPost(PostRequestUrl, input).Result;
            if (httpResult.IsSuccess)
            {
                return httpResult.Payload;
            }
            else
            {
                throw new AmlRestApiException(httpResult);
            }
        }

        internal string SubmitBatchExecutionServiceJob(string submitJobRequestUrl, string apiKey, string jobConfig)
        {
            _httpClientService.AuthorizationToken = apiKey;
            var httpResult = _httpClientService.HttpPost(submitJobRequestUrl, jobConfig).Result;
            if (!httpResult.IsSuccess)
            {
                throw new Exception(httpResult.Payload);
            }

            var jobId = httpResult.Payload.Replace("\"", "");

            return jobId;
        }

        internal void StartBatchExecutionServiceJob(string submitJobRequestUrl, string apiKey, string jobId)
        {
            _httpClientService.AuthorizationToken = apiKey;
            var startJobApiLocation = submitJobRequestUrl.Replace("jobs?api-version=2.0", "jobs/" + jobId + "/start?api-version=2.0");
            var httpResult = _httpClientService.HttpPost(startJobApiLocation, string.Empty).Result;
            if (!httpResult.IsSuccess)
            {
                throw new Exception(httpResult.Payload);
            }

        }

        internal string GetBatchExecutionServiceJobStatus(string submitJobRequestUrl, string apiKey, string jobId, out string results)
        {
            _httpClientService.AuthorizationToken = apiKey;
            var getJobStatusApiLocation = submitJobRequestUrl.Replace("jobs?api-version=2.0", "jobs/" + jobId + "?api-version=2.0");
            var httpResult = _httpClientService.HttpGet(getJobStatusApiLocation).Result;
            if (!httpResult.IsSuccess)
            {
                throw new Exception(httpResult.Payload);
            }
            dynamic parsed = JsonConvert.DeserializeObject<object>(httpResult.Payload);
            var jobStatus = parsed["StatusCode"];
            results = httpResult.Payload;

            return jobStatus;
        }
        #endregion
    }
}
