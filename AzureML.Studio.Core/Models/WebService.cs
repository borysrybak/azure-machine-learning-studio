namespace AzureML.Studio.Core.Models
{
    internal class WebService
    {
        internal string Id { get; set; }

        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string CreationTime { get; set; }
        internal string WorkspaceId { get; set; }
        internal string DefaultEndpointName { get; set; }
        internal int EndpointCount { get; set; }
    }
}
