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
    }
}
