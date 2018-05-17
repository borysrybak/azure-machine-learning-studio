namespace AzureML.Studio.Core.Models
{
    internal class ApiSettingsProfile
    {
        internal string Version { get; set; }
        internal string StudioApiBaseUrl { get; set; }
        internal string WebServiceApiBaseUrl { get; set; }
        internal string AzureManagementApiBaseUrl { get; set; }
        internal string StudioApi { get; set; }
        internal string WebServiceApi { get; set; }
        internal string GraphLayoutApi { get; set; }

        private string _sdkName;
        internal string SdkName
        {
            get { return _sdkName; }
            set
            {
                _sdkName = value + Version;
            }
        }
    }
}
