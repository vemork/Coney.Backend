using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public bool? IsEmailValidated { get; set; }
        public bool? IsUserAuthorized { get; set; }
        public string? Role { get; set; } = "anonymous";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}