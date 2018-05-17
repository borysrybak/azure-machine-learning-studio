namespace AzureML.Studio.Core.Models
{
    public class AddWebServiceEndpointRequest
    {
        internal string WebServiceId { get; set; }
        internal string EndpointName { get; set; }
        internal string Description { get; set; }
        internal string ThrottleLevel { get; set; }
        internal int? MaxConcurrentCalls { get; set; }
        internal bool PreventUpdate { get; set; }
    }
}
