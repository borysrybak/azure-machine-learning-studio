namespace AzureML.Studio.Core.Models
{
    public class WorkspaceRdfe
    {
        internal string Id { get; set; }

        internal string Name { get; set; }
        internal string SubscriptionId { get; set; }
        internal string Region { get; set; }
        internal string Description { get; set; }
        internal string OwnerId { get; set; }
        internal string StorageAccountName { get; set; }
        internal string WorkspaceState { get; set; }
        internal string EditorLink { get; set; }
        internal AuthorizationToken AuthorizationToken { get; set; }
    }
}
