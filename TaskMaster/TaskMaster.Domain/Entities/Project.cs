using TaskMaster.Domain.Common;
using TaskMaster.Domain.Enums;

namespace TaskMaster.Domain.Entities
{
    public class Project:BaseEntity
    {
        // every project must have name
        public required string Name { get; set; }
        // not every project need description, can be null
        // ? - nullable, without "?" property must have value
        public string? Description { get; set; }
        // foreign key - links to user that own project
        public int OwnerId { get; set; }
        // project status enum
        public ProjectStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // track creation and last update on project
        // ===== NAVIGATION PROPERTIES =====
        // this defines relationships between entities
       
        // one project has one owner
        // null! - promise this will be set, trust me compiler ;)
        public virtual User Owner { get; set; } = null!;
        // one project has many tasks
        // reference to empty list to avoid null reference errors
        public virtual List<Task> Tasks { get; set; } = new List<Task>();
        // one project has many members 
        // reference to empty list to avoid null reference errors
        public virtual List<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    }
}