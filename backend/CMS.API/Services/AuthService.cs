using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IUserService userService,
        ILogger<AuthService> logger)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: user not found for email {Email}", request.Email);
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed: invalid password for user {Email}", request.Email);
            return null;
        }

        // Get user details with roles
        var userDto = await _userService.GetByIdAsync(user.Id);
        if (userDto == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(userDto, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

        // Update last login
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile != null)
        {
            profile.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return new LoginResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<LoginResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Find refresh token
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken && !t.IsRevoked && !t.IsUsed);

        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid or expired refresh token");
            return null;
        }

        // Mark token as used
        storedToken.IsUsed = true;
        await _context.SaveChangesAsync();

        // Get user
        var user = await _userManager.FindByIdAsync(storedToken.UserId);
        if (user == null)
        {
            return null;
        }

        var userDto = await _userService.GetByIdAsync(user.Id);
        if (userDto == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        // Generate new tokens
        var accessToken = _tokenService.GenerateAccessToken(userDto, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Save new refresh token
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

        return new LoginResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: email already exists {Email}", request.Email);
            return false;
        }

        // Create user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true // Auto-confirm for now
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            _logger.LogError("Registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }

        // Add default role
        await _userManager.AddToRoleAsync(user, "User");

        // Create profile
        var profile = new AspNetUserProfile
        {
            UserId = user.Id,
            FullName = request.FullName,
            RegisterDate = DateTime.UtcNow,
            Verified = true
        };

        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User registered successfully: {Email}", request.Email);
        return true;
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user doesn't exist
            return true;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // TODO: Send email with reset link
        _logger.LogInformation("Password reset token generated for {Email}", request.Email);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded;
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        await _tokenService.RevokeAllRefreshTokensAsync(userId);
        return true;
    }

    public async Task<bool> RevokeTokenAsync(string userId)
    {
        return await _tokenService.RevokeRefreshTokenAsync(userId);
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        return await _userService.GetByIdAsync(userId);
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        // Token validation is handled by the JWT middleware
        await Task.CompletedTask;
        return true;
    }
}
