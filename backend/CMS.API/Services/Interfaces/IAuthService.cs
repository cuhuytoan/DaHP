using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<LoginResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<bool> LogoutAsync(string userId);
    Task<bool> RevokeTokenAsync(string userId);
    Task<UserDto?> GetCurrentUserAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
}

/// <summary>
/// User service interface
/// </summary>
public interface IUserService
{
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<PagedResult<UserListDto>> SearchAsync(UserSearchRequest request);
    Task<UserDto> CreateAsync(CreateUserRequest request, string createdBy);
    Task<UserDto?> UpdateAsync(string id, UpdateUserRequest request, string updatedBy);
    Task<UserDto?> AdminUpdateAsync(string id, AdminUpdateUserRequest request, string updatedBy);
    Task<bool> DeleteAsync(string id, string deletedBy);
    Task<bool> ActivateAsync(string id, string activatedBy);
    Task<bool> DeactivateAsync(string id, string deactivatedBy);
    
    // User profile
    Task<UserProfileDto?> GetProfileAsync(string userId);
    Task<UserProfileDto?> UpdateProfileAsync(string userId, UpdateUserProfileRequest request);
    
    // User roles
    Task<List<string>> GetRolesAsync(string userId);
    Task<bool> AddToRoleAsync(string userId, string role, string addedBy);
    Task<bool> RemoveFromRoleAsync(string userId, string role, string removedBy);
    Task<bool> UpdateRolesAsync(string userId, List<string> roles, string updatedBy);
    
    // Statistics
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
    Task<List<UserListDto>> GetRecentUsersAsync(int count = 10);
}

/// <summary>
/// Token service interface
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(UserDto user, IList<string> roles);
    string GenerateRefreshToken();
    Task<bool> SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt);
    Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string userId);
    Task<bool> RevokeAllRefreshTokensAsync(string userId);
}

/// <summary>
/// Order service interface
/// </summary>
public interface IOrderService
{
    // Order CRUD
    Task<ProductOrderDto?> GetByIdAsync(long id);
    Task<ProductOrderDto?> GetByOrderCodeAsync(string orderCode);
    Task<PagedResult<ProductOrderListDto>> SearchAsync(OrderSearchRequest request);
    Task<ProductOrderDto> CreateAsync(CreateOrderRequest request, string? userId = null);
    Task<bool> DeleteAsync(long id, string userId);

    // Order status management
    Task<bool> UpdateStatusAsync(long id, UpdateOrderStatusRequest request, string userId);
    Task<bool> UpdatePaymentStatusAsync(long id, UpdatePaymentStatusRequest request, string userId);
    Task<bool> CancelAsync(long id, CancelOrderRequest request, string userId);
    Task<bool> ConfirmAsync(long id, string userId);
    Task<bool> ShipAsync(long id, string userId);
    Task<bool> DeliverAsync(long id, string userId);
    Task<bool> CompleteAsync(long id, string userId);

    // Order details
    Task<List<ProductOrderDetailDto>> GetOrderDetailsAsync(long orderId);

    // User orders
    Task<PagedResult<ProductOrderListDto>> GetUserOrdersAsync(string userId, int page = 1, int pageSize = 20);
    Task<ProductOrderDto?> GetUserOrderAsync(string userId, long orderId);
    
    // Statistics
    Task<OrderStatisticsDto> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<int> GetTotalCountAsync();
    Task<int> GetPendingCountAsync();
    Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);
    
    // Lookup data
    Task<List<ProductOrderStatusDto>> GetOrderStatusesAsync();
    Task<List<ProductOrderPaymentMethodDto>> GetPaymentMethodsAsync();
    Task<List<ProductOrderPaymentStatusDto>> GetPaymentStatusesAsync();
}

/// <summary>
/// File upload service interface
/// </summary>
public interface IFileUploadService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder = "images");
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder = "files");
    Task<bool> DeleteFileAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);
    string GetFileUrl(string filePath);
    Task<(string FileName, string FileType, long FileSize)> GetFileInfoAsync(string filePath);
}

/// <summary>
/// Email service interface
/// </summary>
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendEmailConfirmationAsync(string email, string confirmationLink);
    Task SendPasswordResetAsync(string email, string resetLink);
    Task SendOrderConfirmationAsync(string email, ProductOrderDto order);
    Task SendOrderStatusUpdateAsync(string email, ProductOrderDto order);
}

/// <summary>
/// Cache service interface
/// </summary>
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
    Task<bool> ExistsAsync(string key);
}

/// <summary>
/// Setting service interface
/// </summary>
public interface ISettingService
{
    Task<string?> GetValueAsync(string key);
    Task<T?> GetValueAsync<T>(string key);
    Task<bool> SetValueAsync(string key, string value, string userId);
    Task<Dictionary<string, string>> GetAllAsync();
    Task<Dictionary<string, string>> GetByGroupAsync(string groupName);
}