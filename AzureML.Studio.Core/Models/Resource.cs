namespace AzureML.Studio.Core.Models
{
    internal class Resource
    {
        internal string Name { get; set; }
        internal string Kind { get; set; }
        internal EndpointLocation Location { get; set; }
    }
}
