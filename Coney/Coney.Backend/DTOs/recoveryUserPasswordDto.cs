namespace Coney.Backend.DTOs
{
    public class recoveryUserPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
