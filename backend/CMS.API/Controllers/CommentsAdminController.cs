using CMS.API.Data;
using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Controllers;

/// <summary>
/// Comments Admin API controller for managing comments and reviews
/// </summary>
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin,Editor")]
public class CommentsAdminController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CommentsAdminController> _logger;

    public CommentsAdminController(ApplicationDbContext context, ILogger<CommentsAdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Article Comments

    /// <summary>
    /// Get article comments with pagination
    /// </summary>
    [HttpGet("articles")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleComments(
        [FromQuery] long? articleId = null,
        [FromQuery] bool? active = null,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Set<ArticleComment>()
            .Include(c => c.Article)
            .AsQueryable();

        if (articleId.HasValue)
        {
            query = query.Where(c => c.ArticleId == articleId.Value);
        }

        if (active.HasValue)
        {
            query = query.Where(c => c.Active == active.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => 
                (c.Name != null && c.Name.Contains(search)) ||
                (c.Email != null && c.Email.Contains(search)) ||
                (c.Content != null && c.Content.Contains(search)));
        }

        var totalCount = await query.CountAsync();
        var comments = await query
            .OrderByDescending(c => c.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                c.Id,
                c.ArticleId,
                ArticleName = c.Article != null ? c.Article.Name : null,
                c.ParentId,
                c.Name,
                c.Email,
                c.Content,
                c.Active,
                c.CreateDate,
                c.CreateBy
            })
            .ToListAsync();

        return ApiSuccess(new
        {
            items = comments,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>
    /// Toggle article comment status
    /// </summary>
    [HttpPost("articles/{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleArticleCommentStatus(long id)
    {
        var comment = await _context.Set<ArticleComment>().FindAsync(id);
        if (comment == null)
            return ApiNotFound("Article comment not found");

        comment.Active = !(comment.Active ?? false);
        await _context.SaveChangesAsync();

        return ApiSuccess(comment.Active, "Article comment status toggled successfully");
    }

    /// <summary>
    /// Delete article comment
    /// </summary>
    [HttpDelete("articles/{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticleComment(long id)
    {
        var comment = await _context.Set<ArticleComment>().FindAsync(id);
        if (comment == null)
            return ApiNotFound("Article comment not found");

        // Delete child comments first
        var children = await _context.Set<ArticleComment>()
            .Where(c => c.ParentId == id)
            .ToListAsync();
        _context.Set<ArticleComment>().RemoveRange(children);
        
        _context.Set<ArticleComment>().Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Product Comments

    /// <summary>
    /// Get product comments with pagination
    /// </summary>
    [HttpGet("products")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductComments(
        [FromQuery] long? productId = null,
        [FromQuery] bool? active = null,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Set<ProductComment>()
            .Include(c => c.Product)
            .AsQueryable();

        if (productId.HasValue)
        {
            query = query.Where(c => c.ProductId == productId.Value);
        }

        if (active.HasValue)
        {
            query = query.Where(c => c.Active == active.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => 
                (c.Name != null && c.Name.Contains(search)) ||
                (c.Email != null && c.Email.Contains(search)) ||
                (c.Content != null && c.Content.Contains(search)));
        }

        var totalCount = await query.CountAsync();
        var comments = await query
            .OrderByDescending(c => c.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                c.Id,
                c.ProductId,
                ProductName = c.Product != null ? c.Product.Name : null,
                c.ParentId,
                c.Name,
                c.Email,
                c.Content,
                c.Active,
                c.CreateDate,
                c.CreateBy
            })
            .ToListAsync();

        return ApiSuccess(new
        {
            items = comments,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>
    /// Toggle product comment status
    /// </summary>
    [HttpPost("products/{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleProductCommentStatus(long id)
    {
        var comment = await _context.Set<ProductComment>().FindAsync(id);
        if (comment == null)
            return ApiNotFound("Product comment not found");

        comment.Active = !(comment.Active ?? false);
        await _context.SaveChangesAsync();

        return ApiSuccess(comment.Active, "Product comment status toggled successfully");
    }

    /// <summary>
    /// Delete product comment
    /// </summary>
    [HttpDelete("products/{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductComment(long id)
    {
        var comment = await _context.Set<ProductComment>().FindAsync(id);
        if (comment == null)
            return ApiNotFound("Product comment not found");

        // Delete child comments first
        var children = await _context.Set<ProductComment>()
            .Where(c => c.ParentId == id)
            .ToListAsync();
        _context.Set<ProductComment>().RemoveRange(children);
        
        _context.Set<ProductComment>().Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Product Reviews

    /// <summary>
    /// Get product reviews with pagination
    /// </summary>
    [HttpGet("reviews")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductReviews(
        [FromQuery] long? productId = null,
        [FromQuery] long? productBrandId = null,
        [FromQuery] bool? active = null,
        [FromQuery] int? minStar = null,
        [FromQuery] int? maxStar = null,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Set<ProductReview>()
            .Include(r => r.Product)
            .Include(r => r.ProductBrand)
            .AsQueryable();

        if (productId.HasValue)
        {
            query = query.Where(r => r.ProductId == productId.Value);
        }

        if (productBrandId.HasValue)
        {
            query = query.Where(r => r.ProductBrandId == productBrandId.Value);
        }

        if (active.HasValue)
        {
            query = query.Where(r => r.Active == active.Value);
        }

        if (minStar.HasValue)
        {
            query = query.Where(r => r.Star >= minStar.Value);
        }

        if (maxStar.HasValue)
        {
            query = query.Where(r => r.Star <= maxStar.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => 
                (r.CustomerName != null && r.CustomerName.Contains(search)) ||
                (r.Email != null && r.Email.Contains(search)) ||
                (r.Content != null && r.Content.Contains(search)));
        }

        var totalCount = await query.CountAsync();
        var reviews = await query
            .OrderByDescending(r => r.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id,
                r.ProductId,
                ProductName = r.Product != null ? r.Product.Name : null,
                r.ProductBrandId,
                ProductBrandName = r.ProductBrand != null ? r.ProductBrand.Name : null,
                r.CustomerName,
                r.Phone,
                r.Email,
                r.Address,
                r.Star,
                r.Content,
                r.Active,
                r.CreateDate,
                r.CreateBy
            })
            .ToListAsync();

        return ApiSuccess(new
        {
            items = reviews,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>
    /// Toggle product review status
    /// </summary>
    [HttpPost("reviews/{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleProductReviewStatus(long id)
    {
        var review = await _context.Set<ProductReview>().FindAsync(id);
        if (review == null)
            return ApiNotFound("Product review not found");

        review.Active = !(review.Active ?? false);
        review.LastEditDate = DateTime.UtcNow;
        review.LastEditBy = User.Identity?.Name;
        await _context.SaveChangesAsync();

        return ApiSuccess(review.Active, "Product review status toggled successfully");
    }

    /// <summary>
    /// Delete product review
    /// </summary>
    [HttpDelete("reviews/{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductReview(long id)
    {
        var review = await _context.Set<ProductReview>().FindAsync(id);
        if (review == null)
            return ApiNotFound("Product review not found");

        _context.Set<ProductReview>().Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion
}
