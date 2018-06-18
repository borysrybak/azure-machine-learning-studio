namespace AzureML.Studio.Core.Models
{
    public class UserAssetBase
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string FamilyId { get; set; }
        public bool IsLatest { get; set; }
        public string DataTypeId { get; set; }
    }
}
