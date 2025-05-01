using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Column("first_name")]
        public required string FirstName { get; set; }
        [Column("last_name")]
        public required string LastName {  get; set; }
        [Column("phone_number")]
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        [Column("password_hash")]
        public string? PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public required string Position { get; set; }
        [Column("logo_attachment_id")]
        public int? LogoAttachmentId { get; set; }
    }
}
