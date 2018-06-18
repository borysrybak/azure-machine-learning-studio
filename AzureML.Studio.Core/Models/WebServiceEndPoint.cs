namespace AzureML.Studio.Core.Models
{
    public class WebServiceEndpoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreationTime { get; set; }
        public string WebServiceId { get; set; }
        public string WorkspaceId { get; set; }
        public string HelpLocation { get; set; }
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
        public string ApiLocation { get; set; }
        public string Version { get; set; }
        public bool PreventUpdate { get; set; }
        public bool SampleDataEnabled { get; set; }
        public string ExperimentLocation { get; set; }
        public int MaxConcurrentCalls { get; set; }
        public string DiagnosticsTraceLevel { get; set; }
        public string ThrottleLevel { get; set; }
        public Resource[] Resources { get; set; }
    }
}
