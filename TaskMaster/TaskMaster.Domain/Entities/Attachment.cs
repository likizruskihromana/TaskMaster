namespace TaskMaster.Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public required string FileName { get; set; }
        public long FileSize { get; set; }
        public required string FileUrl { get; set; }
        // by some user
        public int UploadedById { get; set; }
        public DateTime UploadedAt { get; set; }
        public virtual Task Task { get; set; } = null!;
        // it is clearer then User
        public virtual User UploadedBy { get; set; } = null!;
    }
}