namespace AzureML.Studio.Core.Models
{
    public class Resource
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public EndpointLocation Location { get; set; }
    }
}
