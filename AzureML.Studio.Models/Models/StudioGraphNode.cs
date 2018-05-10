namespace AzureML.Studio.Models
{
    public class StudioGraphNode
    {
        public string Id { get; set; }

        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string UserData { get; set; }
    }
}
