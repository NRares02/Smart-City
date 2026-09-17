using Microsoft.AspNetCore.Identity;
using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> RegisterAsync(
        string name,
        string email,
        string password,
        string roleId,
        string? phoneNumber = null)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            RoleId = roleId,
            IsActive = true,
            Audit = new Audit
            {
                InsertedBy = null,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        return await _userRepository.CreateAsync(user);
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return user;
    }
}
