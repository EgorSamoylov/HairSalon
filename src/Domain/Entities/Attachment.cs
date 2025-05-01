using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        [Column("file_name")]
        public required string FileName { get; set; }
        [Column("stored_path")]
        public required string StoredPath { get; set; }
        [Column("content_type")]
        public required string ContentType { get; set; }
        public long Size { get; set; }
        [Column("create_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
