namespace Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public string? Note { get; set; }
        public string? Position { get; set; }
        public int? LogoAttachmentId { get; set; }
        public string? LogoAttachmentUrl { get; set; }
    }
}
