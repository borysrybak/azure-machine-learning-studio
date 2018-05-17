namespace AzureML.Studio.Core.Models
{
    internal class EndpointLocation
    {
        internal string BaseLocation { get; set; }
        internal string RelativeLocation { get; set; }
        internal string SasBlobToken { get; set; }
    }
}
