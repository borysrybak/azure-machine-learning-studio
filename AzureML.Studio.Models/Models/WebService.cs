namespace AzureML.Studio.Models
{
    public class WebService
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CreationTime { get; set; }
        public string WorkspaceId { get; set; }
        public string DefaultEndpointName { get; set; }
        public int EndpointCount { get; set; }
    }
}
