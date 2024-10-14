using Coney.Backend.DTOs;
using Coney.Backend.Repositories;
using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

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
                CreatedAt = user.CreatedAt
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
                UpdatedAt = DateTime.Now
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
                LastName = UserRegDto.LastName
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

    // This method is responsible for creating the instance and and registration of the entity
    // with all the information necessary to register a user.
    /*public async Task<UserDto> AddUserAsync(CreateUserDto userDto)
    {
        try
        {
            // It is verified that the email is unique in the system.
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("The email is already in use.");
            }

            // Create the User entity based on the DTO
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex) when (!(ex is InvalidOperationException))
        {
            _logger.LogError(ex, "Error creating a new user");
            throw new ApplicationException("An error occurred while creating the user.");
        }
    }

    // This method is responsible updating information
    // in the DB for the sent user
    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
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

            return new UserDto
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

    // This method is responsible for removing users from the entity.
    public async Task DeleteUserAsync(int id)
    {
        try
        {
            await _userRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID {id}");
            throw new ApplicationException($"An error occurred while deleting the user with ID {id}.");
        }
    }*/
}