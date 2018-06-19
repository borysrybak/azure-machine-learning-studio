namespace AzureML.Studio.Core.Models
{
    public class Workspace
    {
        public string WorkspaceId { get; set; }

        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string HdInsightClusterConnectionString { get; set; }
        public string HdInsightStorageConnectionString { get; set; }
        public bool UseDefaultHdInsightSettings { get; set; }
        public AuthorizationToken AuthorizationToken { get; set; }
        public string MigrationStatus { get; set; }
        public string OwnerEmail { get; set; }
        public string UserStorage { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string SubscriptionState { get; set; }
        public string Region { get; set; }
        public string WorkspaceStatus { get; set; }
        public string Type { get; set; }
        public string CreatedTime { get; set; }
    }
}
