using AzureML.Studio.Core.Models;

namespace AzureML.Studio.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var studioClient = new StudioClient();


            var sWS = new WorkspaceSettings()
            {
                WorkspaceId = "x",
                AuthorizationToken = "y",
                Location = "z"
            };

            var experimentId = "x";

            var dWS = new WorkspaceSettings()
            {
                WorkspaceId = "x",
                AuthorizationToken = "y",
                Location = "z"
            };

            studioClient.CopyExperiment(sWS, experimentId, dWS);
        }
    }
}
