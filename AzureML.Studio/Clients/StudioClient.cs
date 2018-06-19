using AzureML.Studio.Core.Enums;
using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Services;
using AzureML.Studio.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

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
        public IEnumerable<Workspace> GetWorkspaces(IEnumerable<WorkspaceSettings> workspaceSettings)
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
            return GetWorkspaceUsers(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get users from selected workspaces metadata.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetWorkspaceUsers(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetWorkspaceUsers(ws));
        }

        /// <summary>
        /// Get users from selected workspaces metadata.
        /// </summary>
        /// <param name="workspaces">Required parameter to get users from specific workspace.</param>
        /// <returns>Returns dictionary of workspaces and its workspace users.</returns>
        public IDictionary<Workspace, IEnumerable<WorkspaceUser>> GetWorkspaceUsers(IEnumerable<Workspace> workspaces)
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
            AddUserToWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, workspaceUser);
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
        public void AddUserToWorkspace(IEnumerable<WorkspaceSettings> workspacesSettings, WorkspaceUser workspaceUser)
        {
            workspacesSettings.ForEach(ws => AddUserToWorkspace(ws, workspaceUser));
        }

        /// <summary>
        /// Add new user to selected workspaces. 
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void AddUserToWorkspace(IEnumerable<WorkspaceSettings> workspacesSettings, string email, string role)
        {
            AddUserToWorkspace(workspacesSettings, new WorkspaceUser(
                new WorkspaceUserInternal() { User = new UserDetailInternal() { Email = email, Role = role } }));
        }

        /// <summary>
        /// Add new user to selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUser"></param>
        public void AddUserToWorkspace(IEnumerable<Workspace> workspaces, WorkspaceUser workspaceUser)
        {
            workspaces.ForEach(w => AddUserToWorkspace(w, workspaceUser));
        }

        public void AddUserToWorkspace(IEnumerable<Workspace> workspaces, string email, string role)
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
            AddUsersToWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, workspaceUsers);
        }

        /// <summary>
        /// Add new users to selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToWorkspace(IEnumerable<WorkspaceSettings> workspacesSettings, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspacesSettings.ForEach(ws => AddUsersToWorkspace(ws, workspaceUsers));
        }

        /// <summary>
        /// Add new users to selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="workspaceUsers"></param>
        public void AddUsersToWorkspace(IEnumerable<Workspace> workspaces, IEnumerable<WorkspaceUser> workspaceUsers)
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
            return GetDatasetsFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetsFromWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetDatasetsFromWorkspace(ws));
        }

        /// <summary>
        /// Get a dataset for selected workspaces.
        /// </summary>
        /// <param name="workspaces">Required parameter to get workspace, dataset dictionary.</param>
        /// <returns>Returns dictionary of workspaces and its datasets.</returns>
        public IDictionary<Workspace, IEnumerable<Dataset>> GetDatasetsFromWorkspaces(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetDatasetsFromWorkspace(w));
        }

        //TODO: 
        //public void PromoteDataset()
        //{

        //}

        //public void PromoteTrainedModel()
        //{

        //}

        //public void PromoteTransform()
        //{

        //}

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
            DeleteDatasetFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        public void DeleteDatasetFromWorkspace(Workspace workspace, Dataset dataset)
        {
            DeleteDatasetFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, dataset.FamilyId);
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
            DeleteDatasetsFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasets);
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
            DeleteAllDatasetsFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Delete all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void DeleteAllDatasetsFromWorkspaces(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => DeleteAllDatasetsFromWorkspace(ws));
        }

        /// <summary>
        /// Delete all datasets from selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void DeleteAllDatasetsFromWorkspaces(IEnumerable<Workspace> workspaces)
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
            DownloadDatasetFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasetId, fileName);
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
            DownloadDatasetFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, dataset, fileName);
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
            DownloadDatasetsFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, datasets);
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
            DownloadAllDatasetsFromWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
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
            UploadResourceToWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, resourceFileFormat, filePath);
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
            UploadResourcesToWorkspace(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, resources);
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

        /// <summary>
        /// Get experiments from workspace.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public IEnumerable<Experiment> GetExperiments(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetExperiments(workspaceSettings);
        }

        /// <summary>
        /// Get experiments from workspace.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public IEnumerable<Experiment> GetExperiments(string workspaceId, string authorizationToken, string location)
        {
            return GetExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            });
        }

        /// <summary>
        /// Get experiments from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public IEnumerable<Experiment> GetExperiments(Workspace workspace)
        {
            return GetExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get experiments from workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <returns>Returns a dictionary of workspaces and its collection of experiments.</returns>
        public IDictionary<Workspace, IEnumerable<Experiment>> GetExperiments(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetExperiments(ws));
        }

        /// <summary>
        /// Get experiments from workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns>Returns a dictionary of workspaces and its collection of experiments.</returns>
        public IDictionary<Workspace, IEnumerable<Experiment>> GetExperiments(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetExperiments(w));
        }

        /// <summary>
        /// Get experiment by id.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public Experiment GetExperiment(WorkspaceSettings workspaceSettings, string experimentId)
        {
            var rawJson = string.Empty;
            return _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
        }

        /// <summary>
        /// Get experiment by id.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public Experiment GetExperiment(string workspaceId, string authorizationToken, string location, string experimentId)
        {
            return GetExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId);
        }

        /// <summary>
        /// Get experiment by id.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public Experiment GetExperiment(Workspace workspace, string experimentId)
        {
            return GetExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId);
        }

        /// <summary>
        /// Get experiments by ids.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        /// <returns>Returns experiments collection.</returns>
        public IEnumerable<Experiment> GetExperiments(WorkspaceSettings workspaceSettings, IEnumerable<string> experimentsIds)
        {
            return experimentsIds.Select(ei => GetExperiment(workspaceSettings, ei));
        }

        /// <summary>
        /// Get experiments by ids.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentsIds"></param>
        /// <returns>Returns experiments collection.</returns>
        public IEnumerable<Experiment> GetExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<string> experimentsIds)
        {
            return GetExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentsIds);
        }

        /// <summary>
        /// Get experiments by ids.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        /// <returns>Returns experiments collection.</returns>
        public IEnumerable<Experiment> GetExperiments(Workspace workspace, IEnumerable<string> experimentsIds)
        {
            return GetExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentsIds);
        }

        /// <summary>
        /// Run Experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        public void RunExperiment(WorkspaceSettings workspaceSettings, string experimentId)
        {
            var rawJson = string.Empty;
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.RunExperiment(
                workspaceSettings,
                experiment,
                rawJson);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        public void RunExperiment(string workspaceId, string authorizationToken, string location, string experimentId)
        {
            RunExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public void RunExperiment(Workspace workspace, string experimentId)
        {
            RunExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        public void RunExperiment(WorkspaceSettings workspaceSettings, Experiment experiment)
        {
            var rawJson = string.Empty;
            _managementService.GetExperimentById(workspaceSettings, experiment.ExperimentId, out rawJson);
            _managementService.RunExperiment(
                workspaceSettings,
                experiment,
                rawJson);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        public void RunExperiment(string workspaceId, string authorizationToken, string location, Experiment experiment)
        {
            RunExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiment);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public void RunExperiment(Workspace workspace, Experiment experiment)
        {
            RunExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiment);
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        public void RunExperiments(WorkspaceSettings workspaceSettings, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => RunExperiment(workspaceSettings, ei));
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentsIds"></param>
        public void RunExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<string> experimentsIds)
        {
            RunExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentsIds);
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public void RunExperiments(Workspace workspace, IEnumerable<string> experimentsIds)
        {
            RunExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentsIds);
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiments"></param>
        public void RunExperiments(WorkspaceSettings workspaceSettings, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => RunExperiment(workspaceSettings, e));
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiments"></param>
        public void RunExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<Experiment> experiments)
        {
            RunExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiments);
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public void RunExperiments(Workspace workspace, IEnumerable<Experiment> experiments)
        {
            RunExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiments);
        }

        /// <summary>
        /// Run all experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void RunExperiments(WorkspaceSettings workspaceSettings)
        {
            RunExperiments(workspaceSettings, GetExperiments(workspaceSettings));
        }

        /// <summary>
        /// Run all experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void RunExperiments(string workspaceId, string authorizationToken, string location)
        {
            RunExperiments(workspaceId, authorizationToken, location, GetExperiments(workspaceId, authorizationToken, location));
        }

        /// <summary>
        /// Run all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public void RunExperiments(Workspace workspace)
        {
            RunExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Run all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void RunExperiments(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => RunExperiments(ws));
        }

        /// <summary>
        /// Run all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void RunExperiments(IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => RunExperiments(w));
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        public void SaveExperiment(WorkspaceSettings workspaceSettings, string experimentId)
        {
            var rawJson = string.Empty;
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.SaveExperiment(workspaceSettings, experiment, rawJson);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        public void SaveExperiment(string workspaceId, string authorizationToken, string location, string experimentId)
        {
            SaveExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public void SaveExperiment(Workspace workspace, string experimentId)
        {
            SaveExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        public void SaveExperiment(WorkspaceSettings workspaceSettings, Experiment experiment)
        {
            SaveExperiment(workspaceSettings, experiment.ExperimentId);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        public void SaveExperiment(string workspaceId, string authorizationToken, string location, Experiment experiment)
        {
            SaveExperiment(workspaceId, authorizationToken, location, experiment.ExperimentId);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public void SaveExperiment(Workspace workspace, Experiment experiment)
        {
            SaveExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        public void SaveExperiments(WorkspaceSettings workspaceSettings, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => SaveExperiment(workspaceSettings, ei));
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentsIds"></param>
        public void SaveExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<string> experimentsIds)
        {
            SaveExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentsIds);
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public void SaveExperiments(Workspace workspace, IEnumerable<string> experimentsIds)
        {
            SaveExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentsIds);
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiments"></param>
        public void SaveExperiments(WorkspaceSettings workspaceSettings, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => SaveExperiment(workspaceSettings, e));
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiments"></param>
        public void SaveExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<Experiment> experiments)
        {
            SaveExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiments);
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public void SaveExperiments(Workspace workspace, IEnumerable<Experiment> experiments)
        {
            SaveExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiments);
        }

        /// <summary>
        /// Save all experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void SaveExperiments(WorkspaceSettings workspaceSettings)
        {
            SaveExperiments(workspaceSettings, GetExperiments(workspaceSettings));
        }

        /// <summary>
        /// Save all experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void SaveExperiments(string workspaceId, string authorizationToken, string location)
        {
            SaveExperiments(workspaceId, authorizationToken, location, GetExperiments(workspaceId, authorizationToken, location));
        }

        /// <summary>
        /// Save all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public void SaveExperiments(Workspace workspace)
        {
            SaveExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Save all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void SaveExperiments(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => SaveExperiments(ws, GetExperiments(ws)));
        }

        /// <summary>
        /// Save all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void SaveExperiments(IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => SaveExperiments(w, GetExperiments(w)));
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(WorkspaceSettings workspaceSettings, string experimentId, string newName)
        {
            var rawJson = string.Empty;
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.SaveExperimentAs(workspaceSettings, experiment, rawJson, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(string workspaceId, string authorizationToken, string location, string experimentId, string newName)
        {
            SaveExperimentAs(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(Workspace workspace, string experimentId, string newName)
        {
            SaveExperimentAs(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(WorkspaceSettings workspaceSettings, Experiment experiment, string newName)
        {
            SaveExperimentAs(workspaceSettings, experiment.ExperimentId, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(string workspaceId, string authorizationToken, string location, Experiment experiment, string newName)
        {
            SaveExperimentAs(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiment, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        /// <param name="newName"></param>
        public void SaveExperimentAs(Workspace workspace, Experiment experiment, string newName)
        {
            SaveExperimentAs(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiment, newName);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        public void DeleteExperiment(WorkspaceSettings workspaceSettings, string experimentId)
        {
            _managementService.RemoveExperimentById(workspaceSettings, experimentId);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        public void DeleteExperiment(string workspaceId, string authorizationToken, string location, string experimentId)
        {
            DeleteExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public void DeleteExperiment(Workspace workspace, string experimentId)
        {
            DeleteExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        public void DeleteExperiment(WorkspaceSettings workspaceSettings, Experiment experiment)
        {
            DeleteExperiment(workspaceSettings, experiment.ExperimentId);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        public void DeleteExperiment(string workspaceId, string authorizationToken, string location, Experiment experiment)
        {
            DeleteExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiment);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public void DeleteExperiment(Workspace workspace, Experiment experiment)
        {
            DeleteExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiment);
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        public void DeleteExperiments(WorkspaceSettings workspaceSettings, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => DeleteExperiment(workspaceSettings, ei));
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentsIds"></param>
        public void DeleteExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<string> experimentsIds)
        {
            DeleteExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentsIds);
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public void DeleteExperiments(Workspace workspace, IEnumerable<string> experimentsIds)
        {
            DeleteExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentsIds);
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiments"></param>
        public void DeleteExperiments(WorkspaceSettings workspaceSettings, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => DeleteExperiment(workspaceSettings, e));
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiments"></param>
        public void DeleteExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<Experiment> experiments)
        {
            DeleteExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experiments);
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public void DeleteExperiments(Workspace workspace, IEnumerable<Experiment> experiments)
        {
            DeleteExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experiments);
        }

        /// <summary>
        /// Delete all experiments.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void DeleteExperiments(WorkspaceSettings workspaceSettings)
        {
            DeleteExperiments(workspaceSettings, GetExperiments(workspaceSettings));
        }

        /// <summary>
        /// Delete all experiments.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void DeleteExperiments(string workspaceId, string authorizationToken, string location)
        {
            DeleteExperiments(workspaceId, authorizationToken, location, GetExperiments(workspaceId, authorizationToken, location));
        }

        /// <summary>
        /// Delete all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public void DeleteExperiments(Workspace workspace)
        {
            DeleteExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Delete all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        public void DeleteExperiments(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            workspacesSettings.ForEach(ws => DeleteExperiments(ws));
        }

        /// <summary>
        /// Delete all experiments in selected workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        public void DeleteExperiments(IEnumerable<Workspace> workspaces)
        {
            workspaces.ForEach(w => DeleteExperiments(w));
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        public void ExportExperiment(WorkspaceSettings workspaceSettings, string experimentId)
        {
            var rawJson = string.Empty;
            var outputFile = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            File.WriteAllText(outputFile.ExperimentId, rawJson);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        public void ExportExperiment(WorkspaceSettings workspaceSettings, Experiment experiment)
        {
            ExportExperiment(workspaceSettings, experiment.ExperimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        public void ExportExperiment(string workspaceId, string authorizationToken, string location, string experimentId)
        {
            ExportExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        public void ExportExperiment(string workspaceId, string authorizationToken, string location, Experiment experiment)
        {
            ExportExperiment(workspaceId, authorizationToken, location, experiment.ExperimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public void ExportExperiment(Workspace workspace, string experimentId)
        {
            ExportExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public void ExportExperiment(Workspace workspace, Experiment experiment)
        {
            ExportExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentId"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(WorkspaceSettings workspaceSettings, string experimentId, string outputFile)
        {
            var rawJson = string.Empty;
            _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            File.WriteAllText(outputFile, rawJson);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiment"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(WorkspaceSettings workspaceSettings, Experiment experiment, string outputFile)
        {
            ExportExperiment(workspaceSettings, experiment.ExperimentId, outputFile);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentId"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(string workspaceId, string authorizationToken, string location, string experimentId, string outputFile)
        {
            ExportExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentId, outputFile);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiment"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(string workspaceId, string authorizationToken, string location, Experiment experiment, string outputFile)
        {
            ExportExperiment(workspaceId, authorizationToken, location, experiment.ExperimentId, outputFile);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(Workspace workspace, string experimentId, string outputFile)
        {
            ExportExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentId, outputFile);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        /// <param name="outputFile"></param>
        public void ExportExperiment(Workspace workspace, Experiment experiment, string outputFile)
        {
            ExportExperiment(workspace, experiment.ExperimentId, outputFile);
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        public void ExportExperiments(WorkspaceSettings workspaceSettings, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => ExportExperiment(workspaceSettings, ei));
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="experiments"></param>
        public void ExportExperiments(WorkspaceSettings workspaceSettings, IEnumerable<Experiment> experiments)
        {
            ExportExperiments(workspaceSettings, experiments.Select(e => e.ExperimentId));
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experimentsIds"></param>
        public void ExportExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<string> experimentsIds)
        {
            ExportExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, experimentsIds);
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="experiments"></param>
        public void ExportExperiments(string workspaceId, string authorizationToken, string location, IEnumerable<Experiment> experiments)
        {
            ExportExperiments(workspaceId, authorizationToken, location, experiments.Select(e => e.ExperimentId));
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public void ExportExperiments(Workspace workspace, IEnumerable<string> experimentsIds)
        {
            ExportExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, experimentsIds);
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public void ExportExperiments(Workspace workspace, IEnumerable<Experiment> experiments)
        {
            ExportExperiments(workspace, experiments.Select(e => e.ExperimentId));
        }

        /// <summary>
        /// Export all experiments as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        public void ExportExperiments(WorkspaceSettings workspaceSettings)
        {
            ExportExperiments(workspaceSettings, GetExperiments(workspaceSettings));
        }

        /// <summary>
        /// Export all experiments as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        public void ExportExperiments(string workspaceId, string authorizationToken, string location)
        {
            ExportExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            });
        }

        /// <summary>
        /// Export all experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        public void ExportExperiments(Workspace workspace)
        {
            ExportExperiments(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Import experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="inputFile"></param>
        public void ImportExperiment(WorkspaceSettings workspaceSettings, string inputFile)
        {
            ImportExperimentProcess(workspaceSettings, inputFile);
        }

        /// <summary>
        /// Import experiment as JSON to specific workspaces.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="inputFile"></param>
        public void ImportExperiment(IEnumerable<WorkspaceSettings> workspacesSettings, string inputFile)
        {
            workspacesSettings.ForEach(ws => ImportExperiment(ws, inputFile));
        }

        /// <summary>
        /// Import experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="inputFile"></param>
        public void ImportExperiment(string workspaceId, string authorizationToken, string location, string inputFile)
        {
            ImportExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, inputFile);
        }

        /// <summary>
        /// Import experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="inputFile"></param>
        public void ImportExperiment(Workspace workspace, string inputFile)
        {
            ImportExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, inputFile);
        }

        /// <summary>
        /// Import experiment as JSON to specific workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="inputFile"></param>
        public void ImportExperiment(IEnumerable<Workspace> workspaces, string inputFile)
        {
            workspaces.ForEach(w => ImportExperiment(w, inputFile));
        }

        /// <summary>
        /// Import new experiment as JSON.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        public void ImportExperiment(WorkspaceSettings workspaceSettings, string inputFile, string newName)
        {
            ImportExperimentProcess(workspaceSettings, inputFile, newName);
        }

        /// <summary>
        /// Import new experiment as JSON to specific workspaces. 
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        public void ImportExperiment(IEnumerable<WorkspaceSettings> workspacesSettings, string inputFile, string newName)
        {
            workspacesSettings.ForEach(ws => ImportExperiment(ws, inputFile, newName));
        }

        /// <summary>
        /// Import new experiment as JSON.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        public void ImportExperiment(string workspaceId, string authorizationToken, string location, string inputFile, string newName)
        {
            ImportExperiment(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, inputFile, newName);
        }

        /// <summary>
        /// Import new experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        public void ImportExperiment(Workspace workspace, string inputFile, string newName)
        {
            ImportExperiment(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, inputFile, newName);
        }

        /// <summary>
        /// Import new experiment as JSON to specific workspaces.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        public void ImportExperiment(IEnumerable<Workspace> workspaces, string inputFile, string newName)
        {
            workspaces.ForEach(w => ImportExperiment(w, inputFile, newName));
        }

        /// <summary>
        /// Copy experiment from workspace to workspace.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="experimentId"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        public void CopyExperiment(WorkspaceSettings sourceWorkspaceSettings, string experimentId, WorkspaceSettings destinationWorkspaceSettings)
        {
            CopyExperimentProcess(sourceWorkspaceSettings, experimentId, destinationWorkspaceSettings);
        }

        /// <summary>
        /// Copy experiment from workspace to workspace.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="experiment"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        public void CopyExperiment(WorkspaceSettings sourceWorkspaceSettings, Experiment experiment, WorkspaceSettings destinationWorkspaceSettings)
        {
            CopyExperiment(sourceWorkspaceSettings, experiment.ExperimentId, destinationWorkspaceSettings);
        }

        /// <summary>
        /// Copy specific experiments from workspace to workspace.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="experimentsIds"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        public void CopyExperiments(WorkspaceSettings sourceWorkspaceSettings, IEnumerable<string> experimentsIds, WorkspaceSettings destinationWorkspaceSettings)
        {
            experimentsIds.ForEach(ei => CopyExperiment(sourceWorkspaceSettings, ei, destinationWorkspaceSettings));
        }

        /// <summary>
        /// Copy specific experiments from workspace to workspace.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="experiments"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        public void CopyExperiments(WorkspaceSettings sourceWorkspaceSettings, IEnumerable<Experiment> experiments, WorkspaceSettings destinationWorkspaceSettings)
        {
            experiments.ForEach(e => CopyExperiment(sourceWorkspaceSettings, e, destinationWorkspaceSettings));
        }

        /// <summary>
        /// Copy all experiments from workspace to workspace.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        public void CopyExperiments(WorkspaceSettings sourceWorkspaceSettings, WorkspaceSettings destinationWorkspaceSettings)
        {
            CopyExperiments(sourceWorkspaceSettings, GetExperiments(sourceWorkspaceSettings), destinationWorkspaceSettings);
        }

        /// <summary>
        /// Get trained model.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="userAssetId"></param>
        /// <returns>Returns user asset.</returns>
        public UserAsset GetTrainedModel(WorkspaceSettings workspaceSettings, string userAssetId)
        {
            return GetTrainedModels(workspaceSettings).First(tm => tm.Id.Equals(userAssetId));
        }

        /// <summary>
        /// Get trained model.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public UserAsset GetTrainedModel(string workspaceId, string authorizationToken, string location, string userAssetId)
        {
            return GetTrainedModel(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, userAssetId);
        }

        /// <summary>
        /// Get trained model.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public UserAsset GetTrainedModel(Workspace workspace, string userAssetId)
        {
            return GetTrainedModel(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, userAssetId);
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTrainedModels(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetTrainedModels(workspaceSettings);
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTrainedModels(string workspaceId, string authorizationToken, string location)
        {
            return GetTrainedModels(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            });
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTrainedModels(Workspace workspace)
        {
            return GetTrainedModels(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <returns></returns>
        public IDictionary<Workspace, IEnumerable<UserAsset>> GetTrainedModels(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetTrainedModels(ws));
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns></returns>
        public IDictionary<Workspace, IEnumerable<UserAsset>> GetTrainedModels(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetTrainedModels(w));
        }

        /// <summary>
        /// Get transform.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public UserAsset GetTransform(WorkspaceSettings workspaceSettings, string userAssetId)
        {
            return GetTransforms(workspaceSettings).First(tm => tm.Id.Equals(userAssetId));
        }

        /// <summary>
        /// Get transform.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public UserAsset GetTransform(string workspaceId, string authorizationToken, string location, string userAssetId)
        {
            return GetTransform(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            }, userAssetId);
        }

        /// <summary>
        /// Get transform.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public UserAsset GetTransform(Workspace workspace, string userAssetId)
        {
            return GetTransform(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region, userAssetId);
        }

        /// <summary>
        /// Get transform.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTransforms(WorkspaceSettings workspaceSettings)
        {
            return _managementService.GetTransforms(workspaceSettings);
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTransforms(string workspaceId, string authorizationToken, string location)
        {
            return GetTransforms(new WorkspaceSettings()
            {
                WorkspaceId = workspaceId,
                AuthorizationToken = authorizationToken,
                Location = location
            });
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public IEnumerable<UserAsset> GetTransforms(Workspace workspace)
        {
            return GetTransforms(workspace.WorkspaceId, workspace.AuthorizationToken.PrimaryToken, workspace.Region);
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspacesSettings"></param>
        /// <returns></returns>
        public IDictionary<Workspace, IEnumerable<UserAsset>> GetTransforms(IEnumerable<WorkspaceSettings> workspacesSettings)
        {
            return workspacesSettings.ToDictionary(ws => GetWorkspace(ws), ws => GetTransforms(ws));
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspaces"></param>
        /// <returns></returns>
        public IDictionary<Workspace, IEnumerable<UserAsset>> GetTransforms(IEnumerable<Workspace> workspaces)
        {
            return workspaces.ToDictionary(w => w, w => GetTransforms(w));
        }

        #region Private Helpers
        /// <summary>
        /// Import Experiment as JSON process helper.
        /// </summary>
        /// <param name="workspaceSettings"></param>
        /// <param name="inputFile"></param>
        /// <param name="newName"></param>
        private void ImportExperimentProcess(WorkspaceSettings workspaceSettings, string inputFile, string newName = "default")
        {
            var rawJson = File.ReadAllText(inputFile);
            var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(rawJson));
            var serializer = new DataContractJsonSerializer(typeof(Experiment));
            var experiment = (Experiment)serializer.ReadObject(memoryStream);

            if (!newName.Equals("default")) _managementService.SaveExperimentAs(workspaceSettings, experiment, rawJson, newName);
            else _managementService.SaveExperiment(workspaceSettings, experiment, rawJson);
        }

        /// <summary>
        /// Copy experiment from workspace to workspace process helper.
        /// </summary>
        /// <param name="sourceWorkspaceSettings"></param>
        /// <param name="experimentId"></param>
        /// <param name="destinationWorkspaceSettings"></param>
        private void CopyExperimentProcess(WorkspaceSettings sourceWorkspaceSettings, string experimentId, WorkspaceSettings destinationWorkspaceSettings)
        {
            var rawJson = string.Empty;
            var experiment = _managementService.GetExperimentById(sourceWorkspaceSettings, experimentId, out rawJson);

            var activity = _managementService.PackExperiment(sourceWorkspaceSettings, experimentId);
            while (activity.Status != "Complete")
            {
                activity = _managementService.GetActivityStatus(sourceWorkspaceSettings, activity.ActivityId, true);
            }

            activity = _managementService.UnpackExperiment(destinationWorkspaceSettings, activity.Location, destinationWorkspaceSettings.Location);
            while (activity.Status != "Complete")
            {
                activity = _managementService.GetActivityStatus(destinationWorkspaceSettings, activity.ActivityId, false);
            }
        }
        #endregion
    }
}