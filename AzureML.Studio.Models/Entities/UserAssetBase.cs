namespace AzureML.Studio.Models.Entities
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
