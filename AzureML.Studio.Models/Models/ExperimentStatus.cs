namespace AzureML.Studio.Models
{
    public class ExperimentStatus
    {
        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }
        public string CreationTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Metadata { get; set; }
    }
}
