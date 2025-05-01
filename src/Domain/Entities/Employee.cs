using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName {  get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public required string Position { get; set; }
        public int? LogoAttachmentId { get; set; }
    }
}
