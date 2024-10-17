using Coney.Backend.Repositories;

namespace Coney.Backend.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}