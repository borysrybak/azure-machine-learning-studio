namespace AzureML.Studio.Core.Models
{
    internal class UserAsset : UserAssetBase
    {
        internal int Batch { get; set; }
        internal string ExperimentId { get; set; }
        internal string Owner { get; set; }
        internal string PromotedFrom { get; set; }
        internal string ResourceUploadId { get; set; }
        internal string SourceOrigin { get; set; }
        internal TrainedModelLanguageMetadata Language { get; set; }
        internal int Size { get; set; }
        internal string CreatedDate { get; set; }
        internal string ClientVersion { get; set; }
        internal int ServiceVersion { get; set; }
        internal string Category { get; set; }
        internal bool IsDeprecated { get; set; }
        internal string Description { get; set; }
    }
}
