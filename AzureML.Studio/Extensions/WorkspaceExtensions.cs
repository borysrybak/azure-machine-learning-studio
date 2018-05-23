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
        /// Get users from selected Azure Machine Learning Studio workspaces metadata.
        /// </summary>
        /// <param name="workspaces">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public static IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetUsers(this IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetUsers(w));
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUser">Required parameter to add new user profile.</param>
        public static void AddUser(this Workspace workspace, WorkspaceUser workspaceUser)
        {
            _managementService.AddWorkspaceUsers(new WorkspaceSettings()
            {
                WorkspaceId = workspace.Id,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            },
                workspaceUser.Email, workspaceUser.Role);
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
    }
}
