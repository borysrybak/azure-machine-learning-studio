namespace AzureML.Studio.Core.Models
{
    public class Dataset : UserAsset
    {
        internal EndPoint VisualizeEndPoint { get; set; }
        internal EndPoint SchemaEndPoint { get; set; }
        internal EndPoint DownloadLocation { get; set; }
        internal string SchemaStatus { get; set; }
        internal string UploadedFromFileName { get; set; }
    }
}
