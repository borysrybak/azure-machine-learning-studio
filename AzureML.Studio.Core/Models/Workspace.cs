namespace AzureML.Studio.Core.Models
{
    internal class Workspace
    {
        internal string Id { get; set; }

        internal string FriendlyName { get; set; }
        internal string Description { get; set; }
        internal string HdInsightClusterConnectionString { get; set; }
        internal string HdInsightStorageConnectionString { get; set; }
        internal bool UseDefaultHdInsightSettings { get; set; }
        internal AuthorizationToken AuthorizationToken { get; set; }
        internal string MigrationStatus { get; set; }
        internal string OwnerEmail { get; set; }
        internal string UserStorage { get; set; }
        internal string SubscriptionId { get; set; }
        internal string SubscriptionName { get; set; }
        internal string SubscriptionState { get; set; }
        internal string Region { get; set; }
        internal string WorkspaceStatus { get; set; }
        internal string Type { get; set; }
        internal string CreatedTime { get; set; }
    }
}
