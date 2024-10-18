namespace Coney.Backend.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool? IsEmailValidated { get; set; }
        public bool? IsUserAuthorized { get; set; }
        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}