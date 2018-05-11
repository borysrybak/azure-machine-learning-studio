using AzureML.Studio.Models;
using AzureML.Studio.Models.Models;
using Newtonsoft.Json;
using System;

namespace AzureML.Studio.Services
{
    public class ManagementService
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
        #endregion
    }
}
