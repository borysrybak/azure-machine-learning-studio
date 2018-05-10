namespace AzureML.Studio.Models
{
    public class PackingServiceActivity
    {
        public string ActivityId { get; set; }

        public string Location { get; set; }
        public int ItemsComplete { get; set; }
        public int ItemsPending { get; set; }
        public string Status { get; set; }
    }
}
