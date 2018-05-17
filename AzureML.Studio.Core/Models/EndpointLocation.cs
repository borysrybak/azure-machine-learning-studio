namespace AzureML.Studio.Core.Models
{
    public class EndpointLocation
    {
        public string BaseLocation { get; set; }
        public string RelativeLocation { get; set; }
        public string SasBlobToken { get; set; }
    }
}
