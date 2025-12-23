using CMS.API.Data;
using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Controllers;

/// <summary>
/// Users Admin API controller for managing user accounts
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersAdminController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UsersAdminController> _logger;

    public UsersAdminController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext context,
        ILogger<UsersAdminController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? role = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => 
                (u.UserName != null && u.UserName.Contains(search)) ||
                (u.Email != null && u.Email.Contains(search)));
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .OrderByDescending(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var userDtos = new List<object>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var profile = await _context.Set<AspNetUserProfile>()
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            userDtos.Add(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.LockoutEnabled,
                user.LockoutEnd,
                FullName = profile?.FullName,
                RegisterDate = profile?.RegisterDate,
                LastLoginDate = profile?.LastLoginDate,
                Verified = profile?.Verified,
                Roles = roles
            });
        }

        return ApiSuccess(new
        {
            items = userDtos,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return ApiNotFound("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var profile = await _context.Set<AspNetUserProfile>()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        return ApiSuccess(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.EmailConfirmed,
            user.PhoneNumber,
            user.PhoneNumberConfirmed,
            user.LockoutEnabled,
            user.LockoutEnd,
            FullName = profile?.FullName,
            Company = profile?.Company,
            Address = profile?.Address,
            Phone = profile?.Phone,
            RegisterDate = profile?.RegisterDate,
            LastLoginDate = profile?.LastLoginDate,
            LastActivityDate = profile?.LastActivityDate,
            Verified = profile?.Verified,
            VerifiedDate = profile?.VerifiedDate,
            Gender = profile?.Gender,
            BirthDate = profile?.BirthDate,
            AvatarUrl = profile?.AvatarUrl,
            Roles = roles
        });
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            return ApiError("Invalid request", GetModelErrors());

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = request.EmailConfirmed ?? false,
            PhoneNumber = request.PhoneNumber,
            LockoutEnabled = request.LockoutEnabled ?? true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return ApiError("Failed to create user", result.Errors.Select(e => e.Description).ToList());

        // Create profile
        var profile = new AspNetUserProfile
        {
            UserId = user.Id,
            FullName = request.FullName,
            RegisterDate = DateTime.UtcNow,
            Verified = request.Verified ?? false
        };
        _context.Set<AspNetUserProfile>().Add(profile);
        await _context.SaveChangesAsync();

        // Assign roles
        if (request.Roles != null && request.Roles.Any())
        {
            var validRoles = await _roleManager.Roles
                .Where(r => request.Roles.Contains(r.Name!))
                .Select(r => r.Name!)
                .ToListAsync();
            
            if (validRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, validRoles);
            }
        }

        return ApiCreated(new { user.Id, user.UserName }, message: "User created successfully");
    }

    /// <summary>
    /// Update a user
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return ApiError("Invalid request", GetModelErrors());

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return ApiNotFound("User not found");

        // Update user properties
        if (request.Email != null) user.Email = request.Email;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
        if (request.EmailConfirmed.HasValue) user.EmailConfirmed = request.EmailConfirmed.Value;
        if (request.LockoutEnabled.HasValue) user.LockoutEnabled = request.LockoutEnabled.Value;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return ApiError("Failed to update user", result.Errors.Select(e => e.Description).ToList());

        // Update profile
        var profile = await _context.Set<AspNetUserProfile>()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);
        
        if (profile != null)
        {
            if (request.FullName != null) profile.FullName = request.FullName;
            if (request.Company != null) profile.Company = request.Company;
            if (request.Address != null) profile.Address = request.Address;
            if (request.Phone != null) profile.Phone = request.Phone;
            if (request.Gender.HasValue) profile.Gender = request.Gender;
            if (request.BirthDate.HasValue) profile.BirthDate = request.BirthDate;
            if (request.AvatarUrl != null) profile.AvatarUrl = request.AvatarUrl;
            if (request.Verified.HasValue) profile.Verified = request.Verified;
            
            await _context.SaveChangesAsync();
        }

        // Update roles
        if (request.Roles != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            
            var validRoles = await _roleManager.Roles
                .Where(r => request.Roles.Contains(r.Name!))
                .Select(r => r.Name!)
                .ToListAsync();
            
            if (validRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, validRoles);
            }
        }

        return ApiSuccess(new { user.Id, user.UserName }, "User updated successfully");
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return ApiNotFound("User not found");

        // Delete profile first
        var profile = await _context.Set<AspNetUserProfile>()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile != null)
        {
            _context.Set<AspNetUserProfile>().Remove(profile);
            await _context.SaveChangesAsync();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return ApiError("Failed to delete user", result.Errors.Select(e => e.Description).ToList());

        return NoContent();
    }

    /// <summary>
    /// Lock/unlock a user account
    /// </summary>
    [HttpPost("{id}/toggle-lock")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleLock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return ApiNotFound("User not found");

        if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            // Unlock
            await _userManager.SetLockoutEndDateAsync(user, null);
            return ApiSuccess(false, "User unlocked successfully");
        }
        else
        {
            // Lock for 100 years
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            return ApiSuccess(true, "User locked successfully");
        }
    }

    /// <summary>
    /// Reset user password
    /// </summary>
    [HttpPost("{id}/reset-password")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(string id, [FromBody] AdminResetPasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return ApiNotFound("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

        if (!result.Succeeded)
            return ApiError("Failed to reset password", result.Errors.Select(e => e.Description).ToList());

        return ApiSuccess(true, "Password reset successfully");
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet("roles")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<object>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleManager.Roles
            .Select(r => new { r.Id, r.Name, r.AspNetRoleGroup })
            .ToListAsync();

        return ApiSuccess(roles);
    }
}

public class CreateUserRequest
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? LockoutEnabled { get; set; }
    public bool? Verified { get; set; }
    public List<string>? Roles { get; set; }
}

public class UpdateUserRequest
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? AvatarUrl { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? LockoutEnabled { get; set; }
    public bool? Verified { get; set; }
    public List<string>? Roles { get; set; }
}

public class AdminResetPasswordRequest
{
    public string NewPassword { get; set; } = null!;
}
