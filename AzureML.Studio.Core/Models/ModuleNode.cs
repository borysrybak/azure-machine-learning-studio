using System.Collections.Generic;

namespace AzureML.Studio.Core.Models
{
    public class ModuleNode
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string Comment { get; set; }
        public bool? CommentCollapsed { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }
        public bool? UsePreviousResults { get; set; }
        public List<ModuleParameter> ModuleParameters { get; set; }
        public bool? IsPartOfPartialRun { get; set; }
        public List<InputPortInternal> InputPortsInternal { get; set; }
        public List<OutputPortInternal> OutputPortsInternal { get; set; }
    }
}
