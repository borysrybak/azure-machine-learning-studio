using AzureML.Studio.Models.Models;

namespace AzureML.Studio.Common.Helpers
{
    public class ApiSettingsLoaderHelper
    {
        public ApiSettingsProfile LoadDefaultApiSettings()
        {
            var apiSettingsProfile = new ApiSettingsProfile();

            return apiSettingsProfile;
        }

        public ApiSettingsProfile LoadCustomApiSettings(ApiSettingsProfile customApiSettingdsProfile)
        {
            return customApiSettingdsProfile;
        }
    }
}
