using AzureML.Studio.Models.Models;

namespace AzureML.Studio.Services
{
    public class ManagementService
    {
        private readonly ApiSettingsProfile _apiSettings;

        public ManagementService()
        {
            _apiSettings = ApiConfiguration.GetApiConfigurationSettings();
        }

        public ManagementService(ApiSettingsProfile apiSettingsProfile) : this()
        {
            ApiConfiguration.SetConfiguration(apiSettingsProfile);
        }
    }
}
