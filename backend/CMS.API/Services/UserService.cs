using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<UserService> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == id);

        return MapToUserDto(user, roles.ToList(), profile);
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        return MapToUserDto(user, roles.ToList(), profile);
    }

    public async Task<PagedResult<UserListDto>> SearchAsync(UserSearchRequest request)
    {
        var query = _userManager.Users.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(u =>
                (u.Email != null && u.Email.ToLower().Contains(keyword)) ||
                (u.UserName != null && u.UserName.ToLower().Contains(keyword)) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(keyword)));
        }

        if (request.FromDate.HasValue)
        {
            // Filter by profile registration date
        }

        var totalCount = await query.CountAsync();

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "email" => request.SortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "username" => request.SortDescending ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
            _ => query.OrderByDescending(u => u.Id)
        };

        // Apply pagination
        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var items = new List<UserListDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            items.Add(new UserListDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = profile?.FullName,
                PhoneNumber = user.PhoneNumber,
                Avatar = profile?.AvatarUrl,
                Active = profile?.Verified,
                CreateDate = profile?.RegisterDate,
                LastLoginDate = profile?.LastLoginDate,
                Roles = roles.ToList()
            });
        }

        return new PagedResult<UserListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            }
        };
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, string createdBy)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true // Admin-created users are pre-confirmed
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        // Add roles
        if (request.Roles.Any())
        {
            await _userManager.AddToRolesAsync(user, request.Roles);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        // Create profile
        var profile = new AspNetUserProfile
        {
            UserId = user.Id,
            FullName = request.FullName,
            Address = request.Address,
            RegisterDate = DateTime.UtcNow,
            Verified = request.Active
        };

        await _context.UserProfiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(user);
        return MapToUserDto(user, roles.ToList(), profile);
    }

    public async Task<UserDto?> UpdateAsync(string id, UpdateUserRequest request, string updatedBy)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to update user {UserId}: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
            return null;
        }

        // Update profile
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile != null)
        {
            profile.FullName = request.FullName;
            profile.Address = request.Address;
            profile.AvatarUrl = request.Avatar;
            profile.BirthDate = request.DateOfBirth;
            profile.Gender = request.Gender == 1;
            profile.LocationId = request.LocationId;
            profile.DistrictId = request.DistrictId;
            profile.WardId = request.WardId;
            profile.LastActivityDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return MapToUserDto(user, roles.ToList(), profile);
    }

    public async Task<UserDto?> AdminUpdateAsync(string id, AdminUpdateUserRequest request, string updatedBy)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to update user {UserId}: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
            return null;
        }

        // Update profile
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile != null)
        {
            profile.FullName = request.FullName;
            profile.Address = request.Address;
            profile.AvatarUrl = request.Avatar;
            profile.BirthDate = request.DateOfBirth;
            profile.Gender = request.Gender == 1;
            profile.LocationId = request.LocationId;
            profile.DistrictId = request.DistrictId;
            profile.WardId = request.WardId;
            profile.Verified = request.Active;
            profile.LastActivityDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // Update roles
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (request.Roles.Any())
        {
            await _userManager.AddToRolesAsync(user, request.Roles);
        }

        var roles = await _userManager.GetRolesAsync(user);
        return MapToUserDto(user, roles.ToList(), profile);
    }

    public async Task<bool> DeleteAsync(string id, string deletedBy)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        // Soft delete - deactivate profile
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile != null)
        {
            profile.Verified = false;
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> ActivateAsync(string id, string activatedBy)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile == null) return false;

        profile.Verified = true;
        profile.VerifiedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeactivateAsync(string id, string deactivatedBy)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile == null) return false;

        profile.Verified = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserProfileDto?> GetProfileAsync(string userId)
    {
        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null) return null;

        return new UserProfileDto
        {
            Id = (int)profile.Id,
            UserId = profile.UserId,
            ProductBrandId = profile.ProductBrandId,
            DepartmentId = profile.DepartmentId
        };
    }

    public async Task<UserProfileDto?> UpdateProfileAsync(string userId, UpdateUserProfileRequest request)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return null;

        profile.ProductBrandId = request.ProductBrandId;
        profile.DepartmentId = request.DepartmentId;
        profile.LastActivityDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new UserProfileDto
        {
            Id = (int)profile.Id,
            UserId = profile.UserId,
            ProductBrandId = profile.ProductBrandId,
            DepartmentId = profile.DepartmentId
        };
    }

    public async Task<List<string>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<bool> AddToRoleAsync(string userId, string role, string addedBy)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(string userId, string role, string removedBy)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> UpdateRolesAsync(string userId, List<string> roles, string updatedBy)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRolesAsync(user, roles);

        return result.Succeeded;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await _context.UserProfiles.CountAsync(p => p.Verified == true);
    }

    public async Task<List<UserListDto>> GetRecentUsersAsync(int count = 10)
    {
        var profiles = await _context.UserProfiles
            .AsNoTracking()
            .OrderByDescending(p => p.RegisterDate)
            .Take(count)
            .ToListAsync();

        var items = new List<UserListDto>();
        foreach (var profile in profiles)
        {
            var user = await _userManager.FindByIdAsync(profile.UserId ?? "");
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                items.Add(new UserListDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FullName = profile.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Avatar = profile.AvatarUrl,
                    Active = profile.Verified,
                    CreateDate = profile.RegisterDate,
                    LastLoginDate = profile.LastLoginDate,
                    Roles = roles.ToList()
                });
            }
        }

        return items;
    }

    private UserDto MapToUserDto(ApplicationUser user, List<string> roles, AspNetUserProfile? profile)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FullName = profile?.FullName,
            PhoneNumber = user.PhoneNumber,
            Address = profile?.Address,
            Avatar = profile?.AvatarUrl,
            DateOfBirth = profile?.BirthDate,
            Gender = profile?.Gender == true ? 1 : 0,
            LocationId = profile?.LocationId,
            DistrictId = profile?.DistrictId,
            WardId = profile?.WardId,
            DepartmentId = profile?.DepartmentId,
            Active = profile?.Verified,
            CreateDate = profile?.RegisterDate,
            LastLoginDate = profile?.LastLoginDate,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnd = user.LockoutEnd,
            LockoutEnabled = user.LockoutEnabled,
            AccessFailedCount = user.AccessFailedCount,
            Roles = roles
        };
    }
}
