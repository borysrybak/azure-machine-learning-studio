namespace AzureML.Studio.Core.Models
{
    internal class StudioGraphNode
    {
        internal string Id { get; set; }

        internal float CenterX { get; set; }
        internal float CenterY { get; set; }
        internal int Width { get; set; }
        internal int Height { get; set; }
        internal string UserData { get; set; }
    }
}
