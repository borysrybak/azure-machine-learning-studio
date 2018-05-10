namespace AzureML.Studio.Models
{
    public class UserAsset : UserAssetBase
    {
        public int Batch { get; set; }
        public string ExperimentId { get; set; }
        public string Owner { get; set; }
        public string PromotedFrom { get; set; }
        public string ResourceUploadId { get; set; }
        public string SourceOrigin { get; set; }
        public TrainedModelLanguageMetadata Language { get; set; }
        public int Size { get; set; }
        public string CreatedDate { get; set; }
        public string ClientVersion { get; set; }
        public int ServiceVersion { get; set; }
        public string Category { get; set; }
        public bool IsDeprecated { get; set; }
        public string Description { get; set; }
    }
}
