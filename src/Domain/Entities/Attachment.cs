namespace Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string StoredPath { get; set; }
        public required string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
