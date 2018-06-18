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
                WorkspaceId = "ac248c970b46421f992e6b03ad6701ba",
                AuthorizationToken = "IBDZaU/W6Xlvy9yKMifhtNOqQ/0umXZGrCVyU4L7jnxeNMZM9M1UHawQrR6+qSz7OpZyExRe7WLP0bq/3Hwdyw==",
                Location = "West Europe"
            };

            var experimentId = "ac248c970b46421f992e6b03ad6701ba.f-id.39b55411a6ba4b3bb9f45d100e4ce04d";

            var dWS = new WorkspaceSettings()
            {
                WorkspaceId = "5378385d551e4011a444388fbf9290c4",
                AuthorizationToken = "ulxHRR/5wVl1JZN8iADRHi0czGFzK5sV5T9JmALDcuU/WCo8NJOwA6e7bNRJyQvVm9quhIen5RdjhyxTmLx2eg==",
                Location = "West Europe"
            };

            studioClient.CopyExperiment(sWS, experimentId, dWS);
        }
    }
}
