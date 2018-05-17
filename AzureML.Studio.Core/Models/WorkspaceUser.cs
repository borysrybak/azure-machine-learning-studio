namespace AzureML.Studio.Core.Models
{
    public class WorkspaceUser
    {
        public WorkspaceUser(WorkspaceUserInternal workspaceUserInternal)
        {
            Status = workspaceUserInternal.Status;
            Email = workspaceUserInternal.User.Email;
            Name = workspaceUserInternal.User.Email;
            Role = workspaceUserInternal.User.Role;
            Id = workspaceUserInternal.User.Id;
        }

        public string Id { get; set; }

        public string Status { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
