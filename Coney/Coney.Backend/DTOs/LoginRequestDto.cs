using System.ComponentModel.DataAnnotations;

namespace Coney.Backend.DTOs;

public class LoginRequestDto
{
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}