namespace AzureML.Studio.Core.Models
{
    public class WorkspaceRdfe
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string SubscriptionId { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string StorageAccountName { get; set; }
        public string WorkspaceState { get; set; }
        public string EditorLink { get; set; }
        public AuthorizationToken AuthorizationToken { get; set; }
    }
}
