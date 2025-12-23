using CMS.API.Models.Requests;
using CMS.API.Models.Responses;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Comment service interface
/// </summary>
public interface ICommentService
{
    // Article comments
    Task<PagedResult<CommentResponse>> GetArticleCommentsAsync(long articleId, int page = 1, int pageSize = 20, bool? active = null);
    Task<CommentResponse?> GetArticleCommentByIdAsync(int id);
    Task<CommentResponse> CreateArticleCommentAsync(CreateCommentRequest request);
    Task<CommentResponse> ReplyToArticleCommentAsync(int parentId, CreateCommentRequest request);
    Task<bool> ApproveArticleCommentAsync(int id, string approvedBy);
    Task<bool> DeleteArticleCommentAsync(int id, string deletedBy);

    // Product comments
    Task<PagedResult<CommentResponse>> GetProductCommentsAsync(long productId, int page = 1, int pageSize = 20, bool? active = null);
    Task<CommentResponse?> GetProductCommentByIdAsync(int id);
    Task<CommentResponse> CreateProductCommentAsync(CreateCommentRequest request);
    Task<CommentResponse> ReplyToProductCommentAsync(int parentId, CreateCommentRequest request);
    Task<bool> ApproveProductCommentAsync(int id, string approvedBy);
    Task<bool> DeleteProductCommentAsync(int id, string deletedBy);
}
