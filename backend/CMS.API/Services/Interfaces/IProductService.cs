using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Product service interface
/// </summary>
public interface IProductService
{
    // Product CRUD
    Task<ProductDto?> GetByIdAsync(long id);
    Task<ProductDto?> GetByUrlAsync(string url);
    Task<ProductDto?> GetByCodeAsync(string code);
    Task<PagedResult<ProductListDto>> SearchAsync(ProductSearchRequest request);
    Task<ProductDto> CreateAsync(CreateProductRequest request, string userId);
    Task<ProductDto?> UpdateAsync(long id, UpdateProductRequest request, string userId);
    Task<bool> DeleteAsync(long id, string userId);

    // Product status management
    Task<bool> ApproveAsync(long id, string userId);
    Task<bool> RejectAsync(long id, string userId, string? reason = null);
    Task<bool> PublishAsync(long id, string userId);
    Task<bool> UnpublishAsync(long id, string userId);

    // Product categories
    Task<List<ProductCategoryDto>> GetCategoriesAsync(long productId);
    Task<bool> UpdateCategoriesAsync(long productId, List<int> categoryIds, string userId);

    // Product pictures
    Task<List<ProductPictureDto>> GetPicturesAsync(long productId);
    Task<ProductPictureDto> AddPictureAsync(long productId, string name, string image, int sort, string userId);
    Task<bool> UpdatePictureSortAsync(int pictureId, int sort, string userId);
    Task<bool> DeletePictureAsync(int pictureId, string userId);

    // Product attach files
    Task<List<ProductAttachFileDto>> GetAttachFilesAsync(long productId);
    Task<ProductAttachFileDto> AddAttachFileAsync(long productId, string fileName, string filePath, string fileType, int fileSize, string userId);
    Task<bool> DeleteAttachFileAsync(int attachFileId, string userId);

    // Product properties
    Task<List<ProductPropertyValueDto>> GetPropertiesAsync(long productId);
    Task<bool> UpdatePropertiesAsync(long productId, List<ProductPropertyValueRequest> properties, string userId);

    // Product reviews
    Task<PagedResult<ProductReviewDto>> GetReviewsAsync(long productId, int page = 1, int pageSize = 20);
    Task<ProductReviewDto> CreateReviewAsync(CreateProductReviewRequest request, string? userId = null);
    Task<bool> ApproveReviewAsync(int reviewId, string userId);
    Task<bool> DeleteReviewAsync(int reviewId, string userId);

    // Product comments
    Task<PagedResult<ProductCommentDto>> GetCommentsAsync(long productId, int page = 1, int pageSize = 20);
    Task<ProductCommentDto> CreateCommentAsync(CreateProductCommentRequest request, string? userId = null);
    Task<bool> ApproveCommentAsync(int commentId, string userId);
    Task<bool> DeleteCommentAsync(int commentId, string userId);

    // Inventory management
    Task<bool> UpdateQuantityAsync(long productId, int quantity, string userId);
    Task<bool> IncrementQuantitySoldAsync(long productId, int quantity);
    Task<bool> DecrementQuantityAsync(long productId, int quantity);

    // Statistics
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
    Task<int> GetOutOfStockCountAsync();
    Task<List<ProductListDto>> GetTopProductsAsync(int count = 10);
    Task<List<ProductListDto>> GetRecentProductsAsync(int count = 10);
    Task<List<ProductListDto>> GetRelatedProductsAsync(long productId, int count = 5);
    Task<List<ProductListDto>> GetBestSellersAsync(int count = 10);
    Task IncrementCounterAsync(long id);
}

/// <summary>
/// Product category service interface
/// </summary>
public interface IProductCategoryService
{
    Task<ProductCategoryDto?> GetByIdAsync(int id);
    Task<ProductCategoryDto?> GetByUrlAsync(string url);
    Task<List<ProductCategoryDto>> GetAllAsync(bool includeInactive = false);
    Task<List<ProductCategoryDto>> GetTreeAsync(bool includeInactive = false);
    Task<List<ProductCategoryDto>> GetChildrenAsync(int parentId);
    Task<ProductCategoryDto> CreateAsync(CreateProductCategoryRequest request, string userId);
    Task<ProductCategoryDto?> UpdateAsync(int id, UpdateProductCategoryRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> CanDeleteAsync(int id);
    Task<List<int>> GetBreadcrumbIdsAsync(int categoryId);
}

/// <summary>
/// Product brand service interface
/// </summary>
public interface IProductBrandService
{
    Task<ProductBrandDto?> GetByIdAsync(int id);
    Task<ProductBrandDto?> GetByUrlAsync(string url);
    Task<PagedResult<ProductBrandDto>> SearchAsync(string? keyword, int? statusId, bool? active, int page = 1, int pageSize = 20);
    Task<List<ProductBrandDto>> GetAllAsync(bool includeInactive = false);
    Task<ProductBrandDto> CreateAsync(ProductBrandDto request, string userId);
    Task<ProductBrandDto?> UpdateAsync(int id, ProductBrandDto request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> ApproveAsync(int id, string userId);
    Task<bool> RejectAsync(int id, string userId);
}

/// <summary>
/// Product block service interface
/// </summary>
public interface IProductBlockService
{
    Task<List<ProductListDto>> GetProductsByBlockIdAsync(int blockId, int count = 10);
    Task<bool> AddProductToBlockAsync(int blockId, int productId, int sort, string userId);
    Task<bool> RemoveProductFromBlockAsync(int blockId, int productId, string userId);
    Task<bool> UpdateProductSortAsync(int blockId, int productId, int sort, string userId);
}