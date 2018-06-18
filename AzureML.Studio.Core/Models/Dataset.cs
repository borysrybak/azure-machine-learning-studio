namespace AzureML.Studio.Core.Models
{
    public class Dataset : UserAsset
    {
        public EndPoint VisualizeEndPoint { get; set; }
        public EndPoint SchemaEndPoint { get; set; }
        public EndPoint DownloadLocation { get; set; }
        public string SchemaStatus { get; set; }
        public string UploadedFromFileName { get; set; }
    }
}
