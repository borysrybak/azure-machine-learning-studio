namespace AzureML.Studio
{
    internal class Settings
    {
        internal string Version = "0.3.4";
        internal string StudioApiBaseURL = @"https://{0}studioapi.azureml{1}/api/";
        internal string WebServiceApiBaseUrl = @"https://{0}management.azureml{1}/";
        internal string AzureManagementApiBaseUrl = @"https://management.core.windows.net/{0}/cloudservices/amlsdk/resources/machinelearning/~/workspaces/";
        internal string StudioApi = "https://studioapi.azureml.net/api/";
        internal string WebServiceApi = "https://management.azureml.net/";
        internal string GraphLayoutApi = "http://daglayoutservice20160320092532.azurewebsites.net/api/";
        internal string SdkName = "dotnetsdk_" + Version;
    }
}
