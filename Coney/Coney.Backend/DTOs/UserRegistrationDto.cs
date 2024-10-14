using System.ComponentModel.DataAnnotations;

namespace Coney.Backend.DTOs;

public class UserRegistrationDto
{
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
    [MaxLength(100)]
    public string Password { get; set; }
}