namespace AzureML.Studio.Core.Models
{
    public class Experiment
    {
        public string Id { get; set; }

        public string RunId { get; set; }
        public string ParentExperimentId { get; set; }
        public string OriginalExperimentDocumentationLink { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public ExperimentStatus Status { get; set; }
        public string ExperimentTag { get; set; }
        public string Creator { get; set; }
        public bool IsLeaf { get; set; }
        public string DisableNodesUpdate { get; set; }
        public string Category { get; set; }
    }
}
