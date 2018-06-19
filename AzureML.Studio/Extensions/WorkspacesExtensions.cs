using AzureML.Studio.Core.Enums;
using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace AzureML.Studio.Extensions
{
    public static class WorkspacesExtensions
    {
        private static readonly ManagementService _managementService;
        static WorkspacesExtensions()
        {
            _managementService = ManagementService.Instance;
        }

        /// <summary>
        /// Get users from selected Azure Machine Learning Studio workspaces metadata.
        /// </summary>
        /// <param name="workspaces">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public static IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetUsers(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => WorkspaceExtensions.GetUsers(w));
        }

        /// <summary>
        /// Add new user to workspace collection.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public static void AddUser(this IEnumerable<Workspace> workspaces, string email, string role)
        {
            workspaces.ForEach(w => WorkspaceExtensions.AddUser(w, email, role));
        }

        /// <summary>
        /// Add new user to workspace collection.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUser"></param>
        public static void AddUser(this IEnumerable<Workspace> workspaces, WorkspaceUser workspaceUser)
        {
            AddUser(workspaces, workspaceUser.Email, workspaceUser.Role);
        }

        /// <summary>
        /// Add new users to workspace collection.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUsers"></param>
        public static void AddUsers(this IEnumerable<Workspace> workspaces, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspaces.ForEach(w => WorkspaceExtensions.AddUsers(w, workspaceUsers));
        }

        /// <summary>
        /// Get workspace datasets dictionary.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns>Returns a dictionary of workspaces and its datasets.</returns>
        public static IDictionary<Workspace, IEnumerable<Dataset>> GetDatasets(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => WorkspaceExtensions.GetDatasets(w));
        }

        /// <summary>
        /// Delete all datasets in each workspace from collection.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void DeleteAllDatasets(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.DeleteAllDatasets(w));
        }

        /// <summary>
        /// Download all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void DownloadAllDatasets(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.DownloadAllDatasets(w));
        }

        /// <summary>
        /// Upload resource to workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public static void UploadResource(this IEnumerable<Workspace> workspaces, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            workspaces.ForEach(w => WorkspaceExtensions.UploadResource(w, resourceFileFormat, filePath));
        }

        /// <summary>
        /// Upload resources to workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="resources"></param>
        public static void UploadResources(this IEnumerable<Workspace> workspaces, IDictionary<string, ResourceFileFormat> resources)
        {
            workspaces.ForEach(w => WorkspaceExtensions.UploadResources(w, resources));
        }

        /// <summary>
        /// Get experiments dicitonary for specific workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns>Returns a dictionary of workspaces and its collection of experiments.</returns>
        public static IDictionary<Workspace, IEnumerable<Experiment>> GetExperiments(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => WorkspaceExtensions.GetExperiments(w));
        }

        /// <summary>
        /// Run all experiments.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void RunExperiments(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.RunExperiments(w));
        }

        /// <summary>
        /// Save all experiments.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void SaveExperiments(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.SaveExperiments(w));
        }

        /// <summary>
        /// Delete all experiments.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void DeleteExperiments(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.DeleteExperiments(w));
        }

        /// <summary>
        /// Export all experiments as JSON.
        /// </summary>
        /// <param name="workspaces"></param>
        public static void ExportExperiments(this IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => WorkspaceExtensions.ExportExperiments(w));
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns></returns>
        public static IDictionary<Workspace, IEnumerable<UserAsset>> GetTrainedModels(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => WorkspaceExtensions.GetTrainedModels(w));
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns></returns>
        public static IDictionary<Workspace, IEnumerable<UserAsset>> GetTransforms(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => WorkspaceExtensions.GetTransforms(w));
        }
    }
}
