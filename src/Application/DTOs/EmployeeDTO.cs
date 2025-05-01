using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public required string Position { get; set; }
        public int? LogoAttachmentId { get; set; }
        public string? LogoAttachmentUrl { get; set; }
    }
}
