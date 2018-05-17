namespace AzureML.Studio.Core.Models
{
    internal class ExperimentStatus
    {
        internal string StatusCode { get; set; }
        internal string StatusDetail { get; set; }
        internal string CreationTime { get; set; }
        internal string StartTime { get; set; }
        internal string EndTime { get; set; }
        internal string Metadata { get; set; }
    }
}
