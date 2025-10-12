using TaskMaster.Domain.Common;

namespace TaskMaster.Domain.Entities
{
    public class Comment:BaseEntity
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        // comments cant be empty
        public required string Content { get; set; }
        // no need to be comment on comment, in case it is first comment
        // if ParentCommentId is not null, then it is reply
        public int? ParentCommentId { get; set; }
        // ===== NAVIGATION PROPERTIES =====
        // this defines relationships between entities
        public virtual Task Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Comment? ParentComment { get; set; }
        public virtual List<Comment> Replies { get; set; } = new List<Comment>();
    }
}
