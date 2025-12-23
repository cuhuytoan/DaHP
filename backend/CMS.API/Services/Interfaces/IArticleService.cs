using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Article service interface
/// </summary>
public interface IArticleService
{
    // Article CRUD
    Task<ArticleDto?> GetByIdAsync(long id);
    Task<ArticleDto?> GetByUrlAsync(string url);
    Task<PagedResult<ArticleListDto>> SearchAsync(ArticleSearchRequest request);
    Task<ArticleDto> CreateAsync(CreateArticleRequest request, string userId);
    Task<ArticleDto?> UpdateAsync(long id, UpdateArticleRequest request, string userId);
    Task<bool> DeleteAsync(long id, string userId);

    // Article status management
    Task<bool> ApproveAsync(long id, string userId);
    Task<bool> RejectAsync(long id, string userId, string? reason = null);
    Task<bool> PublishAsync(long id, string userId);
    Task<bool> UnpublishAsync(long id, string userId);

    // Article categories
    Task<List<ArticleCategoryDto>> GetCategoriesAsync(long articleId);
    Task<bool> UpdateCategoriesAsync(long articleId, List<int> categoryIds, string userId);

    // Article comments
    Task<PagedResult<ArticleCommentDto>> GetCommentsAsync(long articleId, int page = 1, int pageSize = 20);
    Task<ArticleCommentDto> CreateCommentAsync(CreateArticleCommentRequest request, string? userId = null);
    Task<bool> ApproveCommentAsync(long commentId, string userId);
    Task<bool> DeleteCommentAsync(long commentId, string userId);

    // Article attach files
    Task<List<ArticleAttachFileDto>> GetAttachFilesAsync(long articleId);
    Task<ArticleAttachFileDto> AddAttachFileAsync(long articleId, string fileName, string filePath, string fileType, int fileSize, string userId);
    Task<bool> DeleteAttachFileAsync(long attachFileId, string userId);

    // Statistics
    Task<int> GetTotalCountAsync();
    Task<int> GetPublishedCountAsync();
    Task<List<ArticleListDto>> GetTopArticlesAsync(int count = 10);
    Task<List<ArticleListDto>> GetRecentArticlesAsync(int count = 10);
    Task<List<ArticleListDto>> GetRelatedArticlesAsync(long articleId, int count = 5);
    Task IncrementCounterAsync(long id);
}

/// <summary>
/// Article category service interface
/// </summary>
public interface IArticleCategoryService
{
    Task<ArticleCategoryDto?> GetByIdAsync(long id);
    Task<ArticleCategoryDto?> GetByUrlAsync(string url);
    Task<List<ArticleCategoryDto>> GetAllAsync(bool includeInactive = false);
    Task<List<ArticleCategoryDto>> GetTreeAsync(bool includeInactive = false);
    Task<List<ArticleCategoryDto>> GetChildrenAsync(int parentId);
    Task<ArticleCategoryDto> CreateAsync(CreateArticleCategoryRequest request, string userId);
    Task<ArticleCategoryDto?> UpdateAsync(int id, UpdateArticleCategoryRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> CanDeleteAsync(long id);
    Task<List<int>> GetBreadcrumbIdsAsync(int categoryId);
}

/// <summary>
/// Article block service interface
/// </summary>
public interface IArticleBlockService
{
    Task<List<ArticleListDto>> GetArticlesByBlockIdAsync(int blockId, int count = 10);
    Task<bool> AddArticleToBlockAsync(int blockId, int articleId, int sort, string userId);
    Task<bool> RemoveArticleFromBlockAsync(int blockId, int articleId, string userId);
    Task<bool> UpdateArticleSortAsync(int blockId, int articleId, int sort, string userId);
}