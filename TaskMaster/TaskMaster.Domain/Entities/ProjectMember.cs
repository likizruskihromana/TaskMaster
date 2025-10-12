namespace TaskMaster.Domain.Entities
{
    public class ProjectMember
    {
        public int Id { get; set; }
        // foreign key on Project
        public int ProjectId { get; set; }
        // foreign key on Users
        public int UserId { get; set; }
        public Enums.Role Role { get; set; }
        // we want to know when someone is joined
        public DateTime JoinedAt { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}
