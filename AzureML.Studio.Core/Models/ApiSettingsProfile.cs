namespace AzureML.Studio.Core.Models
{
    public class ApiSettingsProfile
    {
        public string Version { get; set; }
        public string StudioApiBaseUrl { get; set; }
        public string WebServiceApiBaseUrl { get; set; }
        public string AzureManagementApiBaseUrl { get; set; }
        public string StudioApi { get; set; }
        public string WebServiceApi { get; set; }
        public string GraphLayoutApi { get; set; }

        private string _sdkName;
        public string SdkName
        {
            get { return _sdkName; }
            set
            {
                _sdkName = value + Version;
            }
        }
    }
}
