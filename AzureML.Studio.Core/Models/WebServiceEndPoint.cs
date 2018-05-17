namespace AzureML.Studio.Core.Models
{
    internal class WebServiceEndpoint
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string CreationTime { get; set; }
        internal string WebServiceId { get; set; }
        internal string WorkspaceId { get; set; }
        internal string HelpLocation { get; set; }
        internal string PrimaryKey { get; set; }
        internal string SecondaryKey { get; set; }
        internal string ApiLocation { get; set; }
        internal string Version { get; set; }
        internal bool PreventUpdate { get; set; }
        internal bool SampleDataEnabled { get; set; }
        internal string ExperimentLocation { get; set; }
        internal int MaxConcurrentCalls { get; set; }
        internal string DiagnosticsTraceLevel { get; set; }
        internal string ThrottleLevel { get; set; }
        internal Resource[] Resources { get; set; }
    }
}
