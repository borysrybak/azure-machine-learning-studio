using AzureML.Studio.Core.Services;

namespace AzureML.Studio.Extensions
{
    public static class ExperimentExtensions
    {
        private static readonly ManagementService _managementService;
        static ExperimentExtensions()
        {
            _managementService = ManagementService.Instance;
        }
    }
}
