using AzureML.Studio.Core.Exceptions;
using AzureML.Studio.Core;
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

namespace AzureML.Studio.Core.Services
{
    public abstract class ManagementService
    {
        private ApiSettingsProfile _apiSettings;
        private readonly HttpClientService _httpClientService;
        private readonly JsonSerializer _jsonSerializer;

        public ManagementService()
        {
            _apiSettings = ApiConfiguration.GetApiConfigurationSettings();
            _httpClientService = new HttpClientService(_apiSettings.SdkName);
            _jsonSerializer = new JsonSerializer();
        }

        public ManagementService(ApiSettingsProfile apiSettingsProfile) : this()
        {
            ApiConfiguration.SetConfiguration(apiSettingsProfile);
        }

        #region Private Helpers
        private void ValidateWorkspaceSetting(WorkspaceSetting setting)
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
        #endregion

        #region Workspace
        public WorkspaceRdfe[] GetWorkspacesFromRdfe(string managementCertThumbprint, string azureSubscriptionId)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl, azureSubscriptionId);
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "GET");
            var result = GetResultAsync(httpRequest).Result;
            var workspaces = JsonConvert.DeserializeObject<WorkspaceRdfe[]>(result);

            return workspaces;
        }

        public string CreateWorkspace(string managementCertThumbprint, string azureSubscriptionId, string workspaceName, string location, string storageAccountName, string storageAccountKey, string ownerEmail, string source)
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

        public WorkspaceRdfe GetCreateWorkspaceStatus(string managementCertThumbprint, string azureSubscriptionId, string workspaceId, string region)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl + "/{1}?Region={2}", azureSubscriptionId, workspaceId, HttpUtility.HtmlEncode(region));
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "GET");
            var result = GetResultAsync(httpRequest).Result;
            var workspace = JsonConvert.DeserializeObject<WorkspaceRdfe>(result);

            return workspace;
        }

        public void RemoveWorkspace(string managementCertThumbprint, string azureSubscriptionId, string workspaceId, string region)
        {
            var requestUrl = string.Format(_apiSettings.AzureManagementApiBaseUrl + "{1}?Region={2}", azureSubscriptionId, workspaceId, HttpUtility.HtmlEncode(region));
            var httpRequest = GetRdfeHttpRequest(managementCertThumbprint, requestUrl, "DELETE");
            var webResponse = httpRequest.GetResponse();
            var contentLength = webResponse.ContentLength;
            var buffer = new byte[contentLength];
            webResponse.GetResponseStream().Read(buffer, 0, (int)contentLength);

            var result = UnicodeEncoding.ASCII.GetString(buffer);
        }

        public Workspace GetWorkspaceFromAmlRP(WorkspaceSetting setting)
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
                throw new AmlRestApiException(httpResult);
        }

        public void AddWorkspaceUsers(WorkspaceSetting setting, string emails, string role)
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
                throw new AmlRestApiException(httpResult);
        }

        public WorkspaceUser[] GetWorkspaceUsers(WorkspaceSetting setting)
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
                throw new AmlRestApiException(httpResult);
        }
        #endregion
    }
}
