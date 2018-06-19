namespace AzureML.Studio.Core.Models
{
    public class PackingServiceActivity
    {
        public string Id { get; set; }

        public string Location { get; set; }
        public int ItemsComplete { get; set; }
        public int ItemsPending { get; set; }
        public string Status { get; set; }
    }
}
