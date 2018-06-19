namespace AzureML.Studio.Core.Models
{
    public class AddWebServiceEndpointRequest
    {
        public string WebServiceId { get; set; }
        public string EndpointName { get; set; }
        public string Description { get; set; }
        public string ThrottleLevel { get; set; }
        public int? MaxConcurrentCalls { get; set; }
        public bool PreventUpdate { get; set; }
    }
}
