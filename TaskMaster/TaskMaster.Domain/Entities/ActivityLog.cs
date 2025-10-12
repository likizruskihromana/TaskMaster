namespace TaskMaster.Domain.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        // enums can not be required
        public Enums.EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public int UserId { get; set; }
        public Enums.Action Action { get; set; }
        public string? OldValue { get; set; }
        // why ? - deleted items also can be nullable
        public string? NewValue { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual User User { get; set; } = null!;
    }
}