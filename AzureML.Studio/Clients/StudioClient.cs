using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Services;
using AzureML.Studio.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AzureML.Studio
{
    public class StudioClient
    {
        private readonly ManagementService _managementService;

        /// <summary>
        /// Create new instance Azure Machine Learning Studio client to manage Workspaces, Experiments or Assets.
        /// </summary>
        public StudioClient()
        {
            _managementService = ManagementService.Instance;
        }

        /// <summary>
        /// Get Azure Machine Learning Studio workspace metadata.
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get desired workspace.</param>
        /// <returns>Returns workspace object.</returns>
        public Workspace GetWorkspace(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetWorkspaceFromAmlRP(workspaceSettings);
        }

        /// <summary>
        /// Get Azure Machine Learning Studio workspace metadata by creating a workspace settings object based on required triple parameters. 
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns>Returns workspace object after calling base method.</returns>
        public Workspace GetWorkspace(string workspaceId, string authorizationToken, string location)
        {
            return GetWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location });
        }

        /// <summary>
        /// Get selected Azure Machine Learning Studio workspaces metadata. 
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get desired workspaces.</param>
        /// <returns>Returns collection of workspace objects.</returns>
        public IEnumerable<Workspace> GetSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspaceSettings)
        {
            return workspaceSettings.Select(i => GetWorkspace(i));
        }

        /// <summary>
        /// Get Users of Azure Machine Learning Studio workspace.
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get workspace users.</param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public IEnumerable<WorkspaceUser> GetWorkspaceUsers(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetWorkspaceUsers(workspaceSettings);
        }

        /// <summary>
        /// Get Users of Azure Machine Learning Studio workspace by creating a workspace settings object based on required triple parameters. 
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public IEnumerable<WorkspaceUser> GetWorkspaceUsers(string workspaceId, string authorizationToken, string location)
        {
            return GetWorkspaceUsers(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location });
        }

        /// <summary>
        /// Get users from selected Azure Machine Learning Studio workspaces metadata.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetSelectedWorkspacesUsers(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetWorkspaceUsers(ws));
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUser">Required parameter to add new user profile.</param>
        public void AddUserToWorkspace(WorkspaceSettings workspaceSettings, WorkspaceUser workspaceUser)
        {
            _managementService.AddWorkspaceUsers(workspaceSettings, workspaceUser.Email, workspaceUser.Role);
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void AddUserToWorkspace(WorkspaceSettings workspaceSettings, string email, string role)
        {
            AddUserToWorkspace(workspaceSettings, new WorkspaceUser(new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="workspaceUser"></param>
        public void AddUserToWorkspace(string workspaceId, string authorizationToken, string location, WorkspaceUser workspaceUser)
        {
            AddUserToWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location }, workspaceUser);
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void AddUserToWorkspace(string workspaceId, string authorizationToken, string location, string email, string role)
        {
            AddUserToWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location },
                new WorkspaceUser(new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
        }

        /// <summary>
        /// Add new user to selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="workspaceUser"></param>
        public void AddUserToSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings, WorkspaceUser workspaceUser)
        {
            workspacesSettings.ForEach(ws => AddUserToWorkspace(ws, workspaceUser));
        }

        /// <summary>
        /// Add new user to selected workspaces. 
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void AddUserToSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings, string email, string role)
        {
            AddUserToSelectedWorkspaces(workspacesSettings, new WorkspaceUser(new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
        }

        /// <summary>
        /// Add new users to workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToWorkspace(WorkspaceSettings workspaceSettings, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspaceUsers.ForEach(wu => AddUserToWorkspace(workspaceSettings, wu));
        }

        /// <summary>
        /// Add new users to workspace. 
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToWorkspace(string workspaceId, string authorizationToken, string location, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            AddUsersToWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location }, workspaceUsers);
        }

        /// <summary>
        /// Add new users to selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspacesSettings.ForEach(ws => AddUsersToWorkspace(ws, workspaceUsers));
        }
    }
}