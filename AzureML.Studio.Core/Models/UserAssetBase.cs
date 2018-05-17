namespace AzureML.Studio.Core.Models
{
    public class UserAssetBase
    {
        internal string Id { get; set; }

        internal string Name { get; set; }
        internal string FamilyId { get; set; }
        internal bool IsLatest { get; set; }
        internal string DataTypeId { get; set; }
    }
}
