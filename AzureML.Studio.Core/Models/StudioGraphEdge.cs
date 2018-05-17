namespace AzureML.Studio.Core.Models
{
    internal class StudioGraphEdge
    {
        internal string Id { get; set; }

        internal StudioGraphNode SourceNode { get; set; }
        internal StudioGraphNode DestinationNode { get; set; }
        internal string UserData { get; set; }
    }
}
