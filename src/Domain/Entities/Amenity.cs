using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Услуга
    public class Amenity 
    {
        public int Id { get ; set; }
        [Column("service_name")]
        public required string ServiceName { get; set; }
        public string? Description { get; set; }
        [Column("author_id")]
        public int AuthorId { get; set; }
        public int Price { get; set; }
        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }
    }
}
