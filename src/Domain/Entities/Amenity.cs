using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // Услуга
    public class Amenity
    {
        public int Id { get; set; }
        public required string ServiceName { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public int Price { get; set; }
        public int DurationMinutes { get; set; }
    }
}
