using AzureML.Studio.Extensions;

namespace AzureML.Studio.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var studioClient = new StudioClient();
            var fakeWorkspace = studioClient.GetWorkspace("5378385d551e4011a444388fbf9290c4", "ulxHRR/5wVl1JZN8iADRHi0czGFzK5sV5T9JmALDcuU/WCo8NJOwA6e7bNRJyQvVm9quhIen5RdjhyxTmLx2eg==", "West Europe");

            var fakeWorkspaceExperiments = fakeWorkspace.GetExperiments();
            foreach (var exp in fakeWorkspaceExperiments)
            {
                fakeWorkspace.SaveExperimentAs(exp.ExperimentId, "New Name");
            }


        }
    }
}
