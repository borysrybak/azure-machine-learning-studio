namespace AzureML.Studio.Core.Models
{
    internal class Module
    {
        internal string Id { get; set; }

        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string Category { get; set; }
        internal string FamilyId { get; set; }
        internal bool IsDeterministic { get; set; }
        internal bool IsBlocking { get; set; }
        internal string ModuleType { get; set; }
        internal string ReleaseState { get; set; }
        internal ModuleLanguageMetadata ModuleLanguage { get; set; }
        internal string ResourceUploadId { get; set; }
        internal int Size { get; set; }
        internal string CreatedDate { get; set; }
        internal string Owner { get; set; }
        internal bool IsLatest { get; set; }
        internal bool IsDeprecated { get; set; }
        internal string SourceOrigin { get; set; }
        internal string ClientVersion { get; set; }
        internal string ServiceVersion { get; set; }
        internal int Batch { get; set; }
    }
}
