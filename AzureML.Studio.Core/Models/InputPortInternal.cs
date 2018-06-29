namespace AzureML.Studio.Core.Models
{
    public class InputPortInternal
    {
        public string Name { get; set; }
        public string DataSourceId { get; set; }
        public string TrainedModelId { get; set; }
        public string TransformModuleId { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }
        public string PortIndex { get; set; }
        public string NodeId { get; set; }
    }
}
