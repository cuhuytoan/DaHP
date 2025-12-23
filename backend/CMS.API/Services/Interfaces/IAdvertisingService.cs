using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Advertising service interface for public-facing advertising/banner functionality
/// </summary>
public interface IAdvertisingService
{
    /// <summary>
    /// Get all active advertisings by block ID
    /// </summary>
    Task<List<AdvertisingDto>> GetByBlockIdAsync(int blockId);

    /// <summary>
    /// Get all active advertising blocks
    /// </summary>
    Task<List<AdvertisingBlockDto>> GetAllBlocksAsync();

    /// <summary>
    /// Get advertising block by ID with its advertisings
    /// </summary>
    Task<AdvertisingBlockDto?> GetBlockByIdAsync(int blockId);

    /// <summary>
    /// Get single advertising by ID
    /// </summary>
    Task<AdvertisingDto?> GetByIdAsync(long id);

    /// <summary>
    /// Track click on advertising (increment counter)
    /// </summary>
    Task<bool> TrackClickAsync(long id);
}

/// <summary>
/// Admin Advertising service interface for advertising management
/// </summary>
public interface IAdvertisingAdminService
{
    // ===================== Advertising Block Operations =====================
    
    /// <summary>
    /// Get all advertising blocks with pagination
    /// </summary>
    Task<PagedResult<AdvertisingBlockListDto>> GetBlocksAsync(
        int page = 1, 
        int pageSize = 20, 
        string? keyword = null,
        bool? active = null);
    
    /// <summary>
    /// Get advertising block by ID with its advertisements
    /// </summary>
    Task<AdvertisingBlockDetailDto?> GetBlockDetailByIdAsync(long id);
    
    /// <summary>
    /// Create new advertising block
    /// </summary>
    Task<AdvertisingBlockDetailDto> CreateBlockAsync(CreateAdvertisingBlockRequest request, string userId);
    
    /// <summary>
    /// Update advertising block
    /// </summary>
    Task<AdvertisingBlockDetailDto?> UpdateBlockAsync(long id, UpdateAdvertisingBlockRequest request, string userId);
    
    /// <summary>
    /// Delete advertising block
    /// </summary>
    Task<bool> DeleteBlockAsync(long id);
    
    /// <summary>
    /// Toggle advertising block status
    /// </summary>
    Task<bool> ToggleBlockStatusAsync(long id, string userId);

    // ===================== Advertising Operations =====================
    
    /// <summary>
    /// Get all advertisings with pagination
    /// </summary>
    Task<PagedResult<AdvertisingListDto>> GetAdvertisingsAsync(
        int page = 1,
        int pageSize = 20,
        int? blockId = null,
        int? typeId = null,
        string? keyword = null,
        bool? active = null);
    
    /// <summary>
    /// Get advertising by ID
    /// </summary>
    Task<AdvertisingDetailDto?> GetAdvertisingByIdAsync(long id);
    
    /// <summary>
    /// Create new advertising
    /// </summary>
    Task<AdvertisingDetailDto> CreateAdvertisingAsync(CreateAdvertisingRequest request, string userId);
    
    /// <summary>
    /// Update advertising
    /// </summary>
    Task<AdvertisingDetailDto?> UpdateAdvertisingAsync(long id, UpdateAdvertisingRequest request, string userId);
    
    /// <summary>
    /// Delete advertising
    /// </summary>
    Task<bool> DeleteAdvertisingAsync(long id);
    
    /// <summary>
    /// Toggle advertising status
    /// </summary>
    Task<bool> ToggleAdvertisingStatusAsync(long id, string userId);
    
    /// <summary>
    /// Update sort order for advertisings in a block
    /// </summary>
    Task<bool> UpdateSortOrderAsync(long blockId, List<long> advertisingIds, string userId);

    // ===================== Advertising Type Operations =====================
    
    /// <summary>
    /// Get all advertising types
    /// </summary>
    Task<List<AdvertisingTypeDto>> GetTypesAsync();
}
