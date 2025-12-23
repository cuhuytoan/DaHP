using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Authentication API controller
/// </summary>
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user info</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            return ApiUnauthorized("Invalid email or password");
        }

        _logger.LogInformation("User {Email} logged in successfully", request.Email);
        return ApiSuccess(result, "Login successful");
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Registration data</param>
    /// <returns>Success status</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var result = await _authService.RegisterAsync(request);
        if (!result)
        {
            return ApiError("Registration failed. Email may already be in use.");
        }

        _logger.LogInformation("New user registered: {Email}", request.Email);
        return ApiSuccess(true, "Registration successful. Please check your email to confirm your account.");
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New JWT token</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
        {
            return ApiUnauthorized("Invalid or expired refresh token");
        }

        return ApiSuccess(result, "Token refreshed successfully");
    }

    /// <summary>
    /// Confirm email address
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="token">Confirmation token</param>
    /// <returns>Success status</returns>
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return ApiError("Invalid confirmation link");
        }

        var result = await _authService.ConfirmEmailAsync(userId, token);
        if (!result)
        {
            return ApiError("Email confirmation failed");
        }

        return ApiSuccess(true, "Email confirmed successfully");
    }

    /// <summary>
    /// Request password reset
    /// </summary>
    /// <param name="request">Forgot password request</param>
    /// <returns>Success status</returns>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var result = await _authService.ForgotPasswordAsync(request);
        // Always return success to prevent email enumeration
        return ApiSuccess(true, "If the email exists, a password reset link has been sent.");
    }

    /// <summary>
    /// Reset password with token
    /// </summary>
    /// <param name="request">Reset password request</param>
    /// <returns>Success status</returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var result = await _authService.ResetPasswordAsync(request);
        if (!result)
        {
            return ApiError("Password reset failed. The link may have expired.");
        }

        return ApiSuccess(true, "Password reset successfully");
    }

    /// <summary>
    /// Change password for authenticated user
    /// </summary>
    /// <param name="request">Change password request</param>
    /// <returns>Success status</returns>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _authService.ChangePasswordAsync(userId, request);
        if (!result)
        {
            return ApiError("Password change failed. Current password may be incorrect.");
        }

        return ApiSuccess(true, "Password changed successfully");
    }

    /// <summary>
    /// Logout current user
    /// </summary>
    /// <returns>Success status</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var userId = CurrentUserId;
        if (!string.IsNullOrEmpty(userId))
        {
            await _authService.LogoutAsync(userId);
            _logger.LogInformation("User {UserId} logged out", userId);
        }

        return ApiSuccess(true, "Logged out successfully");
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var user = await _authService.GetCurrentUserAsync(userId);
        if (user == null)
        {
            return ApiUnauthorized();
        }

        return ApiSuccess(user);
    }

    /// <summary>
    /// Validate token
    /// </summary>
    /// <returns>Token validity status</returns>
    [HttpGet("validate")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ValidateToken()
    {
        // If we reach here, the token is valid (Authorize attribute passed)
        return ApiSuccess(true, "Token is valid");
    }
}