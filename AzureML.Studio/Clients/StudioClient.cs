using AzureML.Studio.Core.Enums;
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
        public IEnumerable<Dataset> GetDatasetsFromWorkspace(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetDatasets(workspaceSettings);
        }

        /// <summary>
        /// Get a dataset from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns>Returns dataset collection from particular workspace.</returns>
        public IEnumerable<Dataset> GetDatasetsFromWorkspace(string workspaceId, string authorizationToken, string location)
        {
            return _managementService.GetDatasets(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location });
        }

        /// <summary>
        /// Get a dataset from workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get desired dataset.<</param>
        /// <returns>Returns dataset collection from particular workspace.</returns>
        public IEnumerable<Dataset> GetDatasetsFromWorkspace(Workspace workspace)
        {
            return GetDatasetsFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetsFromSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetDatasetsFromWorkspace(ws));
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspaces">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetsFromSelectedWorkspaces(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetDatasetsFromWorkspace(w));
        }


        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="datasetFamilyId"></param>
        public void DeleteDatasetFromWorkspace(WorkspaceSettings workspaceSettings, string datasetFamilyId)
        {
            _managementService.DeleteDataset(workspaceSettings, datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="dataset"></param>
        public void DeleteDatasetFromWorkspace(WorkspaceSettings workspaceSettings, Dataset dataset)
        {
            DeleteDatasetFromWorkspace(workspaceSettings, dataset.FamilyId);
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
        /// Delete dataset from workspace.
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

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetFamilyId"></param>
        public void DeleteDatasetFromWorkspace(Workspace workspace, string datasetFamilyId)
        {
            DeleteDatasetFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        public void DeleteDatasetFromWorkspace(Workspace workspace, Dataset dataset)
        {
            DeleteDatasetFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, dataset.FamilyId);
        }


        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="datasets"></param>
        public void DeleteDatasetsFromWorkspace(WorkspaceSettings workspaceSettings, IEnumerable<Dataset> datasets)
        {
            datasets.ForEach(d => DeleteDatasetFromWorkspace(workspaceSettings, d));
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="datasets"></param>
        public void DeleteDatasetsFromWorkspace(string workspaceId, string authorizationToken, string location, IEnumerable<Dataset> datasets)
        {
            DeleteDatasetsFromWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, datasets);
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public void DeleteDatasetsFromWorkspace(Workspace workspace, IEnumerable<Dataset> datasets)
        {
            DeleteDatasetsFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasets);
        }

        /// <summary>
        /// Delete all datasets from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void DeleteAllDatasetsFromWorkspace(WorkspaceSettings workspaceSettings)
        {
            DeleteDatasetsFromWorkspace(workspaceSettings, GetDatasetsFromWorkspace(workspaceSettings));
        }

        /// <summary>
        /// Delete all datasets from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void DeleteAllDatasetsFromWorkspace(string workspaceId, string authorizationToken, string location)
        {
            DeleteAllDatasetsFromWorkspace(new WorkspaceSettings() { WorkspaceId = workspaceId, AuthorizationToken = authorizationToken, Location = location });
        }

        /// <summary>
        /// Delete all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public void DeleteAllDatasetsFromWorkspace(Workspace workspace)
        {
            DeleteAllDatasetsFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Delete all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void DeleteAllDatasetsFromSelectedWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => DeleteAllDatasetsFromWorkspace(ws));
        }

        /// <summary>
        /// Delete all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void DeleteAllDatasetsFromSelectedWorkspaces(IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => DeleteAllDatasetsFromWorkspace(w));
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="datasetId"></param>
        public void DownloadDatasetFromWorkspace(WorkspaceSettings workspaceSettings, string datasetId, string fileName = "dataset")
        {
            _managementService.DownloadDatasetAsync(workspaceSettings, datasetId, $"{fileName}.{workspaceSettings.WorkspaceId}.{datasetId}");
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="datasetId"></param>
        public void DownloadDatasetFromWorkspace(string workspaceId, string authorizationToken, string location, string datasetId, string fileName = "dataset")
        {
            DownloadDatasetFromWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, datasetId, fileName);
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetId"></param>
        public void DownloadDatasetFromWorkspace(Workspace workspace, string datasetId, string fileName = "dataset")
        {
            DownloadDatasetFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasetId, fileName);
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="dataset"></param>
        public void DownloadDatasetFromWorkspace(WorkspaceSettings workspaceSettings, Dataset dataset, string fileName = "dataset")
        {
            DownloadDatasetFromWorkspace(workspaceSettings, dataset.Id, fileName);
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="dataset"></param>
        public void DownloadDatasetFromWorkspace(string workspaceId, string authorizationToken, string location, Dataset dataset, string fileName = "dataset")
        {
            DownloadDatasetFromWorkspace(workspaceId, authorizationToken, location, dataset.Id, fileName);
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        public void DownloadDatasetFromWorkspace(Workspace workspace, Dataset dataset, string fileName = "dataset")
        {
            DownloadDatasetFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, dataset, fileName);
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="datasets"></param>
        public void DownloadDatasetsFromWorkspace(WorkspaceSettings workspaceSettings, IEnumerable<Dataset> datasets)
        {
            datasets.ForEach(d => DownloadDatasetFromWorkspace(workspaceSettings, d));
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="datasets"></param>
        public void DownloadDatasetsFromWorkspace(string workspaceId, string authorizationToken, string location, IEnumerable<Dataset> datasets)
        {
            DownloadDatasetsFromWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, datasets);
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public void DownloadDatasetsFromWorkspace(Workspace workspace, IEnumerable<Dataset> datasets)
        {
            DownloadDatasetsFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasets);
        }

        /// <summary>
        /// Download all datasets from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void DownloadAllDatasetsFromWorkspace(WorkspaceSettings workspaceSettings)
        {
            DownloadDatasetsFromWorkspace(workspaceSettings, GetDatasetsFromWorkspace(workspaceSettings));
        }

        /// <summary>
        /// Download all datasets from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void DownloadAllDatasetsFromWorkspace(string workspaceId, string authorizationToken, string location)
        {
            DownloadAllDatasetsFromWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            });
        }

        /// <summary>
        /// Download all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public void DownloadAllDatasetsFromWorkspace(Workspace workspace)
        {
            DownloadAllDatasetsFromWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Download all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void DownloadAllDatasetsFromWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => DownloadAllDatasetsFromWorkspace(ws));
        }

        /// <summary>
        /// Download all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void DownloadAllDatasetsFromWorkspaces(IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => DownloadAllDatasetsFromWorkspace(w));
        }

        /// <summary>
        /// Upload resource file to workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public async void UploadResourceToWorkspace(WorkspaceSettings workspaceSettings, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            await _managementService.UploadResourceAsync(workspaceSettings, resourceFileFormat.GetDescription(), filePath);
        }

        /// <summary>
        /// Upload resource file to workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public void UploadResourceToWorkspace(string workspaceId, string authorizationToken, string location, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            UploadResourceToWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, resourceFileFormat, filePath);
        }

        /// <summary>
        /// Upload resource file to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public void UploadResourceToWorkspace(Workspace workspace, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            UploadResourceToWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, resourceFileFormat, filePath);
        }

        /// <summary>
        /// Upload resource files to workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="resources"></param>
        public void UploadResourcesToWorkspace(WorkspaceSettings workspaceSettings, IDictionary<string, ResourceFileFormat> resources)
        {
            resources.ForEach(pair => UploadResourceToWorkspace(workspaceSettings, pair.Value, pair.Key));
        }

        /// <summary>
        /// Upload resource files to workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="resources"></param>
        public void UploadResourcesToWorkspace(string workspaceId, string authorizationToken, string location, IDictionary<string, ResourceFileFormat> resources)
        {
            UploadResourcesToWorkspace(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, resources);
        }

        /// <summary>
        /// Upload resource files to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resources"></param>
        public void UploadResourcesToWorkspace(Workspace workspace, IDictionary<string, ResourceFileFormat> resources)
        {
            UploadResourcesToWorkspace(workspace.Id, workspace.AuthorizationToken.PrimaryToken, workspace.Region, resources);
        }

        /// <summary>
        /// Upload resource file to workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public void UploadResourceToWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            workspacesSettings.ForEach(ws => UploadResourceToWorkspace(ws, resourceFileFormat, filePath));
        }

        /// <summary>
        /// Upload resource file to workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public void UploadResourceToWorkspaces(IEnumerable<Workspace> workspaces, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            workspaces.ForEach(w => UploadResourceToWorkspace(w, resourceFileFormat, filePath));
        }

        /// <summary>
        /// Upload resource files to workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="resources"></param>
        public void UploadResourcesToWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings, IDictionary<string, ResourceFileFormat> resources)
        {
            workspacesSettings.ForEach(ws => UploadResourcesToWorkspace(ws, resources));
        }

        /// <summary>
        /// Upload resource files to workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="resources"></param>
        public void UploadResourcesToWorkspaces(IEnumerable<Workspace> workspaces, IDictionary<string, ResourceFileFormat> resources)
        {
            workspaces.ForEach(w => UploadResourcesToWorkspace(w, resources));
        }

        #region Private Helpers

        #endregion


    }
}