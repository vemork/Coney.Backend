using System.ComponentModel.DataAnnotations;

namespace Coney.Backend.DTOs;

public class UserEmailUpdateDto
{
    public string? Email { get; set; }
    public bool? IsEmailValitdated { get; set; }

    public DateTime UpdatedAt { get; set; }
}