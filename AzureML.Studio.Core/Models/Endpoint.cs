namespace AzureML.Studio.Core.Models
{
    internal class EndPoint
    {
        internal string BaseUri { get; set; }
        internal int Size { get; set; }
        internal string Name { get; set; }
        internal string EndpointType { get; set; }
        internal string CredentialContainer { get; set; }
        internal string AccessCredential { get; set; }
        internal string Location { get; set; }
        internal string FileType { get; set; }
        internal bool IsAuxiliary { get; set; }
    }
}
