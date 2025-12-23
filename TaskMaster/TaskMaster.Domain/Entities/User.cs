using Microsoft.AspNetCore.Identity;

namespace TaskMaster.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        // primary key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Avatar {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime? LastLogoutAt { get; set; }

        // ===== NAVIGATION PROPERTIES =====
        // this defines relationships between entities
        public virtual ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public virtual ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public virtual ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}
