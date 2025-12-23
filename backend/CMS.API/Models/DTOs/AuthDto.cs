using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Login request DTO
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}

/// <summary>
/// Login response DTO
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}

/// <summary>
/// Register request DTO
/// </summary>
public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// Refresh token request DTO
/// </summary>
public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Change password request DTO
/// </summary>
public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Forgot password request DTO
/// </summary>
public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Reset password request DTO
/// </summary>
public class ResetPasswordRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// User DTO
/// </summary>
public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Gender { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public long? DistrictId { get; set; }
    public string? DistrictName { get; set; }
    public long? WardId { get; set; }
    public string? WardName { get; set; }
    public long? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    
    // Identity properties
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    
    public List<string> Roles { get; set; } = new();
    public UserProfileDto? Profile { get; set; }
}

/// <summary>
/// User list DTO
/// </summary>
public class UserListDto
{
    public string Id { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// User profile DTO
/// </summary>
public class UserProfileDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public long? ProductBrandId { get; set; }
    public string? ProductBrandName { get; set; }
    public long? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? Position { get; set; }
    public string? Bio { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? LinkedIn { get; set; }
    public string? Website { get; set; }
}

/// <summary>
/// Update user request DTO
/// </summary>
public class UpdateUserRequest
{
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(500)]
    public string? Avatar { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? Gender { get; set; }

    public long? LocationId { get; set; }

    public long? DistrictId { get; set; }

    public long? WardId { get; set; }
}

/// <summary>
/// Update user profile request DTO
/// </summary>
public class UpdateUserProfileRequest
{
    public long? ProductBrandId { get; set; }

    public long? DepartmentId { get; set; }

    [StringLength(200)]
    public string? Position { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    [StringLength(200)]
    public string? Facebook { get; set; }

    [StringLength(200)]
    public string? Twitter { get; set; }

    [StringLength(200)]
    public string? LinkedIn { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }
}

/// <summary>
/// User search request DTO
/// </summary>
public class UserSearchRequest
{
    public string? Keyword { get; set; }
    public string? Role { get; set; }
    public bool? Active { get; set; }
    public long? ProductBrandId { get; set; }
    public long? DepartmentId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Create user request (admin)
/// </summary>
public class CreateUserRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public bool Active { get; set; } = true;

    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Admin update user request
/// </summary>
public class AdminUpdateUserRequest
{
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(500)]
    public string? Avatar { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? Gender { get; set; }

    public long? LocationId { get; set; }

    public long? DistrictId { get; set; }

    public long? WardId { get; set; }

    public bool Active { get; set; } = true;

    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Update user role request
/// </summary>
public class UpdateUserRoleRequest
{
    [Required]
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// Admin reset password request
/// </summary>
public class AdminResetPasswordRequest
{
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Role DTO
/// </summary>
public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int UserCount { get; set; }
}