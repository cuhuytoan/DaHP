using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

public interface IBlockService
{
    // ===================== Article Block Operations =====================
    
    /// <summary>
    /// Get all article blocks with pagination
    /// </summary>
    Task<PagedResult<ArticleBlockListDto>> GetArticleBlocksAsync(
        int page = 1,
        int pageSize = 20,
        int? categoryId = null,
        string? keyword = null,
        bool? active = null);
    
    /// <summary>
    /// Get article block by ID with its articles
    /// </summary>
    Task<ArticleBlockDetailDto?> GetArticleBlockByIdAsync(long id);
    
    /// <summary>
    /// Create new article block
    /// </summary>
    Task<ArticleBlockDetailDto> CreateArticleBlockAsync(CreateArticleBlockRequest request, string userId);
    
    /// <summary>
    /// Update article block
    /// </summary>
    Task<ArticleBlockDetailDto?> UpdateArticleBlockAsync(long id, UpdateArticleBlockRequest request, string userId);
    
    /// <summary>
    /// Delete article block
    /// </summary>
    Task<bool> DeleteArticleBlockAsync(long id);
    
    /// <summary>
    /// Toggle article block status
    /// </summary>
    Task<bool> ToggleArticleBlockStatusAsync(long id, string userId);
    
    /// <summary>
    /// Add articles to block
    /// </summary>
    Task<bool> AddArticlesToBlockAsync(long blockId, List<int> articleIds);
    
    /// <summary>
    /// Remove article from block
    /// </summary>
    Task<bool> RemoveArticleFromBlockAsync(long blockId, int articleId);

    // ===================== Product Block Operations =====================
    
    /// <summary>
    /// Get all product blocks with pagination
    /// </summary>
    Task<PagedResult<ProductBlockListDto>> GetProductBlocksAsync(
        int page = 1,
        int pageSize = 20,
        int? categoryId = null,
        string? keyword = null,
        bool? active = null);
    
    /// <summary>
    /// Get product block by ID with its products
    /// </summary>
    Task<ProductBlockDetailDto?> GetProductBlockByIdAsync(long id);
    
    /// <summary>
    /// Create new product block
    /// </summary>
    Task<ProductBlockDetailDto> CreateProductBlockAsync(CreateProductBlockRequest request, string userId);
    
    /// <summary>
    /// Update product block
    /// </summary>
    Task<ProductBlockDetailDto?> UpdateProductBlockAsync(long id, UpdateProductBlockRequest request, string userId);
    
    /// <summary>
    /// Delete product block
    /// </summary>
    Task<bool> DeleteProductBlockAsync(long id);
    
    /// <summary>
    /// Toggle product block status
    /// </summary>
    Task<bool> ToggleProductBlockStatusAsync(long id, string userId);
    
    /// <summary>
    /// Add products to block
    /// </summary>
    Task<bool> AddProductsToBlockAsync(long blockId, List<int> productIds);
    
    /// <summary>
    /// Remove product from block
    /// </summary>
    Task<bool> RemoveProductFromBlockAsync(long blockId, int productId);
}
