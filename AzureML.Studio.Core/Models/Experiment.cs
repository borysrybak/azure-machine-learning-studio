namespace AzureML.Studio.Core.Models
{
    internal class Experiment
    {
        internal string Id { get; set; }

        internal string RunId { get; set; }
        internal string ParentExperimentId { get; set; }
        internal string OriginalExperimentDocumentationLink { get; set; }
        internal string Summary { get; set; }
        internal string Description { get; set; }
        internal ExperimentStatus Status { get; set; }
        internal string ExperimentTag { get; set; }
        internal string Creator { get; set; }
        internal bool IsLeaf { get; set; }
        internal string DisableNodesUpdate { get; set; }
        internal string Category { get; set; }
    }
}
