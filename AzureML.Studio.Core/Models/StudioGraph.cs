using System.Collections.Generic;

namespace AzureML.Studio.Core.Models
{
    internal class StudioGraph
    {
        internal StudioGraph()
        {
            Nodes = new List<StudioGraphNode>();
            Edges = new List<StudioGraphEdge>();
        }

        internal string Id { get; set; }

        internal List<StudioGraphNode> Nodes { get; set; }
        internal List<StudioGraphEdge> Edges { get; set; }
        internal string UserData { get; set; }
    }
}
