using AzureML.Studio.Core.Models;

namespace AzureML.Studio.Core
{
    internal static class ApiConfiguration
    {
        private static ApiSettingsProfile _apiSettingsProfile;

        static ApiConfiguration()
        {
            _apiSettingsProfile = new ApiSettingsProfile();
            SetDefaultConfiguration();
        }

        internal static void SetDefaultConfiguration()
        {
            _apiSettingsProfile.Version = "0.3.4";
            _apiSettingsProfile.StudioApiBaseUrl = @"https://{0}studioapi.azureml{1}/api/";
            _apiSettingsProfile.WebServiceApiBaseUrl = @"https://{0}management.azureml{1}/";
            _apiSettingsProfile.AzureManagementApiBaseUrl = @"https://management.core.windows.net/{0}/cloudservices/amlsdk/resources/machinelearning/~/workspaces/";
            _apiSettingsProfile.StudioApi = "https://studioapi.azureml.net/api/";
            _apiSettingsProfile.WebServiceApi = "https://management.azureml.net/";
            _apiSettingsProfile.GraphLayoutApi = "http://daglayoutservice20160320092532.azurewebsites.net/api/";
            _apiSettingsProfile.SdkName = "dotnetsdk_" + _apiSettingsProfile.Version;
        }

        internal static void SetConfiguration(ApiSettingsProfile apiSettingsProfile)
        {
            _apiSettingsProfile = apiSettingsProfile;
        }

        internal static ApiSettingsProfile GetApiConfigurationSettings()
        {
            return _apiSettingsProfile;
        }
    }
}
