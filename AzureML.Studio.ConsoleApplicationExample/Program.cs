using AzureML.Studio.Core.Models;

namespace AzureML.Studio.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var studioClient = new StudioClient();

            //CopyExperimentFromWorkspaceToWorkspaceSamePricingSameRegion(studioClient);
            //CopyExperimentFromWorkspaceToWorkspaceDifferentPricingSameRegion(studioClient);
            //CopyExperimentFromWorkspaceToWorkspaceSamePricingDifferentRegion(studioClient);
            //CopyExperimentFromWorkspaceToWorkspaceDifferentPricingDifferentRegion(studioClient);

            //ModifyExperimentNodeAndOverwrite(studioClient);
            //ModifyExperimentNodeAndSaveAsAnotherExperiment(studioClient);
        }

        /// <summary>
        /// Copy Experiment: 'Import Data - Experiment'
        /// Source Workspace: 'FakeWestEuropeCommandCenterS1'
        /// Destination Workspace: 'FakeWestEuropeCustomerS1'
        /// 
        /// Before copying experiment should be saved and has finished running status.
        /// </summary>
        /// <param name="studioClient"></param>
        static void CopyExperimentFromWorkspaceToWorkspaceSamePricingSameRegion(StudioClient studioClient)
        {
            var sourceWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            var destinationWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(sourceWorkspace, experimentId);

            studioClient.CopyExperiment(sourceWorkspace, experiment, destinationWorkspace);
        }

        /// <summary>
        /// Copy Experiment: 'Import Data - Experiment'
        /// Source Workspace: 'FakeWestEuropeCommandCenterS1'
        /// Destination Workspace: 'FakeWestEuropeCustomerDEVTEST'
        /// 
        /// Before copying experiment should be saved and has finished running status.
        /// </summary>
        /// <param name="studioClient"></param>
        static void CopyExperimentFromWorkspaceToWorkspaceDifferentPricingSameRegion(StudioClient studioClient)
        {
            var sourceWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            var destinationWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(sourceWorkspace, experimentId);

            studioClient.CopyExperiment(sourceWorkspace, experiment, destinationWorkspace);
        }

        /// <summary>
        /// Copy Experiment: 'Import Data - Experiment'
        /// Source Workspace: 'FakeWestEuropeCommandCenterS1'
        /// Destination Workspace: 'FakeSouthCentralUSCustomerS1'
        /// 
        /// Before copying experiment should be saved and has finished running status.
        /// </summary>
        /// <param name="studioClient"></param>
        static void CopyExperimentFromWorkspaceToWorkspaceSamePricingDifferentRegion(StudioClient studioClient)
        {
            var sourceWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            var destinationWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "South Central US"
            };
            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(sourceWorkspace, experimentId);

            studioClient.ExportExperiment(sourceWorkspace, experiment);

            var inputFilePath = @"C:\...\experimentFileName";
            studioClient.ImportExperiment(destinationWorkspace, inputFilePath, "Copied from other region");
        }

        /// <summary>
        /// Copy Experiment: 'Import Data - Experiment'
        /// Source Workspace: 'FakeWestEuropeCommandCenterS1'
        /// Destination Workspace: 'FakeSouthCentralUSDEVTEST'
        /// 
        /// Before copying experiment should be saved and has finished running status.
        /// </summary>
        /// <param name="studioClient"></param>
        static void CopyExperimentFromWorkspaceToWorkspaceDifferentPricingDifferentRegion(StudioClient studioClient)
        {
            var sourceWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };
            var destinationWorkspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "South Central US"
            };
            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(sourceWorkspace, experimentId);

            studioClient.ExportExperiment(sourceWorkspace, experiment);

            var inputFilePath = @"C:\Users\...\experimentFileName";
            studioClient.ImportExperiment(destinationWorkspace, inputFilePath, "Copied from other region");
        }

        /// <summary>
        /// Modify Experiment: 'Import Data - Experiment'
        /// Modified Node Name: 'Import Data' //Found by comment "Import Data Comment"
        /// Modified Parameter Name: 'Database Query'
        /// </summary>
        /// <param name="studioClient"></param>
        static void ModifyExperimentNodeAndOverwrite(StudioClient studioClient)
        {
            var workspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };

            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(workspace, experimentId);

            studioClient.ModifyNodeParameter(workspace, experimentId, "Import Data Comment", "Database Query", "SELECT Name, ProductNumber, CAST(Weight AS float) Weight\r\nFROM SalesLT.Product");
        }

        /// <summary>
        /// Modify Experiment: 'Import Data - Experiment'
        /// Modified Node Name: 'Import Data' //Found by comment "Import Data Comment"
        /// Modified Parameter Name: 'Database Query'
        /// </summary>
        /// <param name="studioClient"></param>
        static void ModifyExperimentNodeAndSaveAsAnotherExperiment(StudioClient studioClient)
        {
            var workspace = new WorkspaceSettings()
            {
                WorkspaceId = "",
                AuthorizationToken = "",
                Location = "West Europe"
            };

            //var experiments = studioClient.GetExperiments(sourceWorkspace);
            var experimentId = "";
            var experiment = studioClient.GetExperiment(workspace, experimentId);

            studioClient.ModifyNodeParameter(workspace, experimentId, "Import Data Comment", "Database Query", "SELECT Name, Color, CAST(Weight AS float) Weight\r\nFROM SalesLT.Product", "Import Data - Experiment 2");
        }
    }
}
