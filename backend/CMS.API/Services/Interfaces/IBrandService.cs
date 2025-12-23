using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Brand management service interface
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// Search brands with pagination and filtering
    /// </summary>
    Task<PagedResult<BrandListDto>> SearchAsync(BrandSearchRequest request);

    /// <summary>
    /// Get brand by ID
    /// </summary>
    Task<BrandDto?> GetByIdAsync(long id);

    /// <summary>
    /// Get brand by URL slug
    /// </summary>
    Task<BrandDto?> GetByUrlAsync(string url);

    /// <summary>
    /// Get brand by code
    /// </summary>
    Task<BrandDto?> GetByCodeAsync(string code);

    /// <summary>
    /// Create a new brand
    /// </summary>
    Task<BrandDto> CreateAsync(CreateBrandRequest request, string createdBy);

    /// <summary>
    /// Update an existing brand
    /// </summary>
    Task<BrandDto?> UpdateAsync(long id, UpdateBrandRequest request, string updatedBy);

    /// <summary>
    /// Delete a brand
    /// </summary>
    Task<bool> DeleteAsync(long id, string deletedBy);

    /// <summary>
    /// Approve a brand
    /// </summary>
    Task<bool> ApproveAsync(long id, string approvedBy);

    /// <summary>
    /// Reject a brand
    /// </summary>
    Task<bool> RejectAsync(long id, string rejectedBy);

    /// <summary>
    /// Update brand status
    /// </summary>
    Task<bool> UpdateStatusAsync(long id, int statusId, string updatedBy);

    /// <summary>
    /// Toggle brand active status
    /// </summary>
    Task<bool> ToggleActiveAsync(long id, string updatedBy);

    /// <summary>
    /// Increment view count
    /// </summary>
    Task IncrementViewCountAsync(long id);

    /// <summary>
    /// Get all brand types
    /// </summary>
    Task<List<BrandTypeDto>> GetTypesAsync();

    /// <summary>
    /// Get all brand statuses
    /// </summary>
    Task<List<BrandStatusDto>> GetStatusesAsync();

    /// <summary>
    /// Get all brand levels
    /// </summary>
    Task<List<BrandLevelDto>> GetLevelsAsync();

    /// <summary>
    /// Check if brand exists
    /// </summary>
    Task<bool> ExistsAsync(long id);

    /// <summary>
    /// Check if code is already used
    /// </summary>
    Task<bool> CodeExistsAsync(string code, long? excludeBrandId = null);

    /// <summary>
    /// Check if URL is already used
    /// </summary>
    Task<bool> UrlExistsAsync(string url, long? excludeBrandId = null);
}
