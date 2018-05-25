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
        /// Get workspace metadata.
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get desired workspace.</param>
        /// <returns>Returns workspace object.</returns>
        public Workspace GetWorkspace(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetWorkspaceFromAmlRP(workspaceSettings);
        }

        /// <summary>
        /// Get workspace metadata by creating a workspace settings object based on required triple parameters. 
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
        /// Get selected workspaces metadata. 
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get desired workspaces.</param>
        /// <returns>Returns collection of workspace objects.</returns>
        public IEnumerable<Workspace> GetSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspaceSettings)
        {
            return workspaceSettings.Select(i => GetWorkspace(i));
        }

        /// <summary>
        /// Get Users of workspace.
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get workspace users.</param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public IEnumerable<WorkspaceUser> GetWorkspaceUsers(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetWorkspaceUsers(workspaceSettings);
        }

        /// <summary>
        /// Get Users of workspace by creating a workspace settings object based on required triple parameters. 
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
        /// Get Users of workspace by creating a workspace settings object based on required triple parameters.  
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public IEnumerable<WorkspaceUser> GetWorkspaceUser(Workspace workspace)
        {
            return GetWorkspaceUsers(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get users from selected workspaces metadata.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetSelectedWorkspacesUsers(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetWorkspaceUsers(ws));
        }

        /// <summary>
        /// Get users from selected workspaces metadata.
        /// </summary>
        /// <param name="workspaces">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetSelectedWorkspacesUsers(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetWorkspaceUser(w));
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
            AddUserToWorkspace(workspaceSettings, new WorkspaceUser(
                new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
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
            AddUserToWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location },
                workspaceUser);
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
        /// Add new user to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="workspaceUser"></param>
        public void AddUserToWorkspace(Workspace workspace, WorkspaceUser workspaceUser)
        {
            AddUserToWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, workspaceUser);
        }

        /// <summary>
        /// Add new user to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void AddUserToWorkspace(Workspace workspace, string email, string role)
        {
            AddUserToWorkspace(workspace, new WorkspaceUser(
                new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
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
            AddUserToSelectedWorkspaces(workspacesSettings, new WorkspaceUser(
                new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
        }

        /// <summary>
        /// Add new user to selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUser"></param>
        public void AddUserToSelectedWorkspaces(IEnumerable<Workspace> workspaces, WorkspaceUser workspaceUser)
        {
            workspaces.ForEach(w => AddUserToWorkspace(w, workspaceUser));
        }

        public void AddUserToSelectedWorkspaces(IEnumerable<Workspace> workspaces, string email, string role)
        {
            workspaces.ForEach(w => AddUserToWorkspace(w, new WorkspaceUser(
                new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } })));
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
            AddUsersToWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location },
                workspaceUsers);
        }

        /// <summary>
        /// Add new users to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToWorkspace(Workspace workspace, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            AddUsersToWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, workspaceUsers);
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

        /// <summary>
        /// Add new users to selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToSelectedWorkspaces(IEnumerable<Workspace> workspaces, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspaces.ForEach(w => AddUsersToWorkspace(w, workspaceUsers));
        }

        /// <summary>
        /// Get a dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings">Required parameter to get desired dataset.</param>
        /// <returns>Returns dataset collection from particular workspace.</returns>
        public IEnumerable<Dataset> GetDatasetFromWorkspace(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetDataset(workspaceSettings);
        }

        /// <summary>
        /// Get a dataset from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns>Returns dataset collection from particular workspace.</returns>
        public IEnumerable<Dataset> GetDatasetFromWorkspace(string workspaceId, string authorizationToken, string location)
        {
            return _managementService.GetDataset(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location });
        }

        /// <summary>
        /// Get a dataset from workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get desired dataset.<</param>
        /// <returns>Returns dataset collection from particular workspace.</returns>
        public IEnumerable<Dataset> GetDatasetFromWorkspace(Workspace workspace)
        {
            return GetDatasetFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetFromSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetDatasetFromWorkspace(ws));
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspaces">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetFromSelectedWorkspaces(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetDatasetFromWorkspace(w));
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="dataset"></param>
        public void DeleteDatasetFromWorkspace(WorkspaceSettings workspaceSettings, Dataset dataset)
        {
            _managementService.DeleteDataset(workspaceSettings, dataset.FamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="dataset"></param>
        public void DeleteDatasetFromWorkspace(string workspaceId, string authorizationToken, string location, Dataset dataset)
        {
            DeleteDatasetFromWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location },
                dataset);
        }

        /// <summary>
        /// Delete dataset from workspace
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="datasetFamilyId"></param>
        public void DeleteDatasetFromWorkspace(WorkspaceSettings workspaceSettings, string datasetFamilyId)
        {
            _managementService.DeleteDataset(workspaceSettings, datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="datasetFamilyId"></param>
        public void DeleteDatasetFromWorkspace(string workspaceId, string authorizationToken, string location, string datasetFamilyId)
        {
            DeleteDatasetFromWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location },
                datasetFamilyId);
        }



        #region Private Helpers

        #endregion


    }
}