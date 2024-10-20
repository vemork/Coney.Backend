using Coney.Backend.DTOs;
using Coney.Backend.Repositories;
using Coney.Shared.Entities;

namespace Coney.Backend.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly ILogger<UserService> _logger;

    public UserService(UserRepository userRepository, ILogger<UserService> logger, EmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException("user not found.");
            }
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                IsEmailValidated = user.IsEmailValidated,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            throw new ApplicationException("An error occurred while getting user info.");
        }
    }

    // This method retrieves the information of all users in the database
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsEmailValidated = user.IsEmailValidated,
                IsUserAuthorized = user.IsUserAuthorized,
                Role = user.Role
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user list");
            throw new ApplicationException("An error occurred while getting users.");
        }
    }

    // This method retrieves the user from the database using the provided ID.
    public async Task<User> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("user not found.");
            }

            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsEmailValidated = user.IsEmailValidated,
                IsUserAuthorized = user.IsUserAuthorized,
                Role = user.Role
            };
        }
        catch (Exception ex) when (!(ex is KeyNotFoundException))
        {
            _logger.LogError(ex, $"Error getting user with ID {id}");
            throw new ApplicationException($"An error occurred while getting the user with ID {id}.");
        }
    }

    public async Task<User> FindUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.FindUserAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("user not found.");
            }
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex) when (!(ex is KeyNotFoundException))
        {
            _logger.LogError(ex, $"Error getting user with ID {id}");
            throw new ApplicationException($"An error occurred while getting the user with ID {id}.");
        }
    }

    public async Task<UserRegistrationDto> AddUserAsync(UserRegistrationDto UserRegDto)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(UserRegDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("The email is already in use.");
            }

            var user = new User
            {
                FirstName = UserRegDto.FirstName,
                LastName = UserRegDto.LastName,
                Email = UserRegDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(UserRegDto.Password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = "anonymous"
            };

            await _userRepository.AddAsync(user);
            _ = Task.Run(async () =>
            {
                await _emailService.SendEmailAsync(UserRegDto.Email);
            });

            return new UserRegistrationDto
            {
                Email = UserRegDto.Email,
                FirstName = UserRegDto.FirstName,
                LastName = UserRegDto.LastName,
                Password = UserRegDto.Password,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating a new user");
            throw new ApplicationException("An error occurred while creating the user.");
        }
    }

    public async Task<bool> ValidateUserEmailAsync(string userEmail)
    {
        try
        {
            var curentUser = await _userRepository.GetByEmailAsync(userEmail);
            if (curentUser == null)
            {
                throw new InvalidOperationException("The email is not already in use.");
            }

            curentUser.IsEmailValidated = true;
            curentUser.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(curentUser);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying to validate user email");
            throw new ApplicationException("\"An error occurred while attempting to find a user....");
        }
    }

    public async Task<bool> SendEmailAsync(string userEmail)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userEmail);
        if (existingUser == null)
        {
            throw new InvalidOperationException("The email is not already in use.");
        }

        _ = Task.Run(async () =>
        {
            await _emailService.SendEmailAsync(userEmail);
        });
        return true;
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto updateUserDto)
    {
        try
        {
            // Get the User entity from the repository
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Updates only the provided fields
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            }
            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UpdatedAt = user.UpdatedAt
            };
        }
        catch (Exception ex) when (!(ex is KeyNotFoundException))
        {
            _logger.LogError(ex, $"Error updating user with ID {id}");
            throw new ApplicationException($"An error occurred while updating user with ID {id}.");
        }
    }

    public async Task<UserRegistrationDto> UpdateUserAdminVerificationAsync(string email)
    {
        try
        {
            User? existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser == null)
            {
                throw new InvalidOperationException("The email is not already in use.");
            }

            if (existingUser.IsEmailValidated == true)
            {
                existingUser.IsUserAuthorized = true;
                existingUser.Role = "user";
                await _userRepository.UpdateAsync(existingUser);
                return new UserRegistrationDto
                {
                    Email = existingUser.Email,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName
                };
            }
            else
            {
                throw new InvalidOperationException("The email is not already verificated by the user.");
            }
        }
        catch (Exception ex) when (!(ex is KeyNotFoundException))
        {
            _logger.LogError(ex, $"Error updating user with ID {email}");
            throw new ApplicationException($"An error occurred while updating user with ID {email}.");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.FindUserAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("The user not Found.");
            }
            await _userRepository.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID {id}");
            throw new ApplicationException($"An error occurred while deleting the user with ID {id}.");
        }
    }

    public async Task changeUserPasswordService(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(changePasswordDto.Email);
            if (user == null)
            {
                throw new InvalidOperationException("The user has been rejected.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            user.UpdatedAt = DateTime.Now;
            
            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user password  with EMAIL {changePasswordDto.Email}");
            throw new ApplicationException($"An error occurred while updating the user password with EMAIL {changePasswordDto.Email}.");
        }
    }

    public async Task recoveryUserPasswordService(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("The user has been not found.");
            }
            
            var token = Guid.NewGuid().ToString();
            user.Token = token;
            user.TokenExp = DateTime.Now.AddMinutes(5);
            await _userRepository.UpdateAsync(user);

            _ = Task.Run(async () =>
            {
                var recoveryLink = $"https://localhost:7104/api/Users/verifyUserRecoveryToken?userEmail={Uri.EscapeDataString(email)}&recoveryToken={token}";
                await _emailService.SendEmailAsync(recoveryLink, email);
            });
            return;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user password  with EMAIL {email}");
            throw new ApplicationException($"An error occurred while updating the user password with EMAIL {email}.");
        }
    }

        public async Task<string> verifyUserRecoveryTokenService(string email, string recoveryToken)
    {
        try
        {
           
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(recoveryToken))
            {
                throw new ApplicationException($"An error occurred while updating the user password with EMAIL {email}.");
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || recoveryToken != user.Token || user.TokenExp < DateTime.Now)
            {
                throw new InvalidOperationException("Unexpected problem checking user recovery information.");
            }
            var randomPass = Guid.NewGuid().ToString();
            user.Password = BCrypt.Net.BCrypt.HashPassword(randomPass);
            user.Token = "";
            user.TokenExp = null;
            await _userRepository.UpdateAsync(user);
            return randomPass;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user password  with EMAIL {email}");
            throw new ApplicationException($"An error occurred while updating the user password with EMAIL {email}.");
        }
    }
}