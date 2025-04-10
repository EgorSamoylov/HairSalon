﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Услуга
    public class Amenity 
    {
        public int Id { get ; set; }
        public required string ServiceName { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public int Price { get; set; }
        public int DurationMinutes { get; set; }
    }
}
