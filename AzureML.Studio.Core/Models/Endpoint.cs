namespace AzureML.Studio.Core.Models
{
    public class EndPoint
    {
        public string BaseUri { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public string EndpointType { get; set; }
        public string CredentialContainer { get; set; }
        public string AccessCredential { get; set; }
        public string Location { get; set; }
        public string FileType { get; set; }
        public bool IsAuxiliary { get; set; }
    }
}
