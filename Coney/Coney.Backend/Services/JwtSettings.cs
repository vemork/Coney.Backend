namespace Coney.Backend.Services
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ExpiresInMinutes { get; set; }
    }
}