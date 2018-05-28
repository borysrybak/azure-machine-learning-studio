using AzureML.Studio.Core.Enums;
using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace AzureML.Studio.Extensions
{
    public static class WorkspaceExtensions
    {
        private static readonly ManagementService _managementService;
        static WorkspaceExtensions()
        {
            _managementService = ManagementService.Instance;
        }

        /// <summary>
        /// Get Users of Azure Machine Learning Studio workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get workspace users.</param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public static IEnumerable<WorkspaceUser> GetUsers(this Workspace workspace)
        {
            return _managementService.GetWorkspaceUsers(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Add new user to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public static void AddUser(this Workspace workspace, string email, string role)
        {
            _managementService.AddWorkspaceUsers(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            },
                email, role);
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUser">Required parameter to add new user profile.</param>
        public static void AddUser(this Workspace workspace, WorkspaceUser workspaceUser)
        {
            AddUser(workspace, workspaceUser.Email, workspaceUser.Role);
        }

        /// <summary>
        /// Add new users to workspace. 
        /// </summary>
        /// <param name="workspace">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUsers">Required parameter to add new user profile.</param>
        public static void AddUsers(this Workspace workspace, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspaceUsers.ForEach(wu => AddUser(workspace, wu));
        }

        /// <summary>
        /// Get datasets from workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get dataset from this parcticular workspace.</param>
        /// <returns>Returns dataset collection.</returns>
        public static IEnumerable<Dataset> GetDatasets(this Workspace workspace)
        {
            return _managementService.GetDatasets(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Delete dataset from workspace
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetFamilyId"></param>
        public static void DeleteDataset(this Workspace workspace, string datasetFamilyId)
        {
            _managementService.DeleteDataset(
                 new WorkspaceSettings()
                 {
                     WorkspaceId = workspace.Id,
                     AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                     Location = workspace.Region
                 },
                 datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        public static void DeleteDataset(this Workspace workspace, Dataset dataset)
        {
            DeleteDataset(workspace, dataset.FamilyId);
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetsFamilyIds"></param>
        public static void DeleteDatasets(this Workspace workspace, IEnumerable<string> datasetsFamilyIds)
        {
            datasetsFamilyIds.ForEach(dfi => DeleteDataset(workspace, dfi));
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public static void DeleteDatasets(this Workspace workspace, IEnumerable<Dataset> datasets)
        {
            DeleteDatasets(workspace, datasets.Select(d => d.FamilyId));
        }

        /// <summary>
        /// Delete all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public static void DeleteAllDatasets(this Workspace workspace)
        {
            DeleteDatasets(workspace, GetDatasets(workspace));
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetId"></param>
        /// <param name="fileName"></param>
        public static void DownloadDataset(this Workspace workspace, string datasetId, string fileName = "dataset")
        {
            _managementService.DownloadDatasetAsync(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, datasetId, $"{fileName}.{workspace.Id}.{datasetId}");
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        /// <param name="fileName"></param>
        public static void DownloadDataset(this Workspace workspace, Dataset dataset, string fileName = "dataset")
        {
            DownloadDataset(workspace, dataset.Id, fileName);
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetsIds"></param>
        public static void DownloadDatasets(this Workspace workspace, IEnumerable<string> datasetsIds)
        {
            datasetsIds.ForEach(di => DownloadDataset(workspace, di));
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public static void DownloadDatasets(this Workspace workspace, IEnumerable<Dataset> datasets)
        {
            datasets.ForEach(d => DownloadDataset(workspace, d));
        }

        /// <summary>
        /// Download all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public static void DownloadAllDatasets(this Workspace workspace)
        {
            DownloadDatasets(workspace, GetDatasets(workspace));
        }

        /// <summary>
        /// Upload resource to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public static async void UploadResource(this Workspace workspace, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            await _managementService.UploadResourceAsync(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, resourceFileFormat.GetDescription(), filePath);
        }

        /// <summary>
        /// Upload resources to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resources"></param>
        public static void UploadResources(this Workspace workspace, IDictionary<string, ResourceFileFormat> resources)
        {
            resources.ForEach(pair => UploadResource(workspace, pair.Value, pair.Key));
        }

        /// <summary>
        /// Get experiments from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public static IEnumerable<Experiment> GetExperiments(this Workspace workspace)
        {
            return _managementService.GetExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Get experiment by id.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public static Experiment GetExperimentById(this Workspace workspace, string experimentId)
        {
            var rawJson = string.Empty;
            return _managementService.GetExperimentById(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, experimentId, out rawJson);
        }
    }
}
