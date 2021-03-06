﻿using System.Collections.Generic;

namespace AzureML.Studio.Core.Models
{
    public class StudioGraph
    {
        public StudioGraph()
        {
            Nodes = new List<StudioGraphNode>();
            Edges = new List<StudioGraphEdge>();
        }

        public string Id { get; set; }

        public List<StudioGraphNode> Nodes { get; set; }
        public List<StudioGraphEdge> Edges { get; set; }
        public string UserData { get; set; }
    }
}
