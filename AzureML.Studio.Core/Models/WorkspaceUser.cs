namespace AzureML.Studio.Core.Models
{
    public class WorkspaceUser
    {
        
        internal WorkspaceUser(WorkspaceUserInternal workspaceUserInternal)
        {
            Status = workspaceUserInternal.Status;
            Email = workspaceUserInternal.User.Email;
            Name = workspaceUserInternal.User.Email;
            Role = workspaceUserInternal.User.Role;
            Id = workspaceUserInternal.User.Id;
        }

        internal string Id { get; set; }

        internal string Status { get; set; }
        internal string Email { get; set; }
        internal string Name { get; set; }
        internal string Role { get; set; }
    }
}
