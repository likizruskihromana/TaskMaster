using TaskMaster.Domain.Common;
using TaskMaster.Domain.Enums;
namespace TaskMaster.Domain.Entities
{
    public class Task: BaseEntity
    {
        // primary key
        public required string Title { get; set; }
        public string? Description { get; set; } 
        // foreign key - links to project that owns task
        public int ProjectId { get; set; }
        // foreign key - links to user that CAN be assigned task
        public int? AssigneeId { get; set; }
        // foreign key - links to user that created task
        public int CreatorId { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        // should be nullable 
        public DateTime? DueDate { get; set; }

        // ===== NAVIGATION PROPERTIES =====
        // this defines relationships between entities

        // null! - promise this will be set, trust me compiler ;)
        public virtual Project Project { get; set; } = null!;
        public virtual User? Assignee { get; set; }
        public virtual User Creator { get; set; } = null!;
        // many to one
        // reference to empty list to avoid null reference errors
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        // many to one
        // reference to empty list to avoid null reference errors
        public virtual List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual List<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    }
}