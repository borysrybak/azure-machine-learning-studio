namespace AzureML.Studio.Core.Models
{
    internal class PackingServiceActivity
    {
        internal string Id { get; set; }

        internal string Location { get; set; }
        internal int ItemsComplete { get; set; }
        internal int ItemsPending { get; set; }
        internal string Status { get; set; }
    }
}
