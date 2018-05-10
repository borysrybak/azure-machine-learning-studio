namespace AzureML.Studio.Models
{
    public class Module
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FamilyId { get; set; }
        public bool IsDeterministic { get; set; }
        public bool IsBlocking { get; set; }
        public string ModuleType { get; set; }
        public string ReleaseState { get; set; }
        public ModuleLanguageMetadata ModuleLanguage { get; set; }
        public string ResourceUploadId { get; set; }
        public int Size { get; set; }
        public string CreatedDate { get; set; }
        public string Owner { get; set; }
        public bool IsLatest { get; set; }
        public bool IsDeprecated { get; set; }
        public string SourceOrigin { get; set; }
        public string ClientVersion { get; set; }
        public string ServiceVersion { get; set; }
        public int Batch { get; set; }
    }
}
