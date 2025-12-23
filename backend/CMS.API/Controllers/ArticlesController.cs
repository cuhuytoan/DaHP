using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Articles API controller
/// </summary>
[Route("api/[controller]")]
public class ArticlesController : BaseController
{
    private readonly IArticleService _articleService;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
    {
        _articleService = articleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all articles with pagination and filtering
    /// </summary>
    /// <param name="request">Search parameters</param>
    /// <returns>Paginated list of articles</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticles([FromQuery] ArticleSearchRequest request)
    {
        var result = await _articleService.SearchAsync(request);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Get article by ID
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <returns>Article details</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticle(long id)
    {
        var article = await _articleService.GetByIdAsync(id);
        if (article == null)
        {
            return ApiNotFound("Article not found");
        }
        return ApiSuccess(article);
    }

    /// <summary>
    /// Get article by URL slug
    /// </summary>
    /// <param name="url">Article URL slug</param>
    /// <returns>Article details</returns>
    [HttpGet("url/{url}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticleByUrl(string url)
    {
        var article = await _articleService.GetByUrlAsync(url);
        if (article == null)
        {
            return ApiNotFound("Article not found");
        }
        
        // Increment view counter
        await _articleService.IncrementCounterAsync(article.Id);
        
        return ApiSuccess(article);
    }

    /// <summary>
    /// Create a new article
    /// </summary>
    /// <param name="request">Article data</param>
    /// <returns>Created article</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var article = await _articleService.CreateAsync(request, userId);
        return ApiCreated(article, $"/api/articles/{article.Id}", "Article created successfully");
    }

    /// <summary>
    /// Update an existing article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <param name="request">Updated article data</param>
    /// <returns>Updated article</returns>
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateArticle(long id, [FromBody] UpdateArticleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var article = await _articleService.UpdateAsync(id, request, userId);
        if (article == null)
        {
            return ApiNotFound("Article not found");
        }

        return ApiSuccess(article, "Article updated successfully");
    }

    /// <summary>
    /// Delete an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteArticle(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _articleService.DeleteAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Article not found");
        }

        return NoContent();
    }

    /// <summary>
    /// Approve an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveArticle(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _articleService.ApproveAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Article not found");
        }

        return ApiSuccess(true, "Article approved successfully");
    }

    /// <summary>
    /// Publish an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/publish")]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishArticle(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _articleService.PublishAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Article not found");
        }

        return ApiSuccess(true, "Article published successfully");
    }

    /// <summary>
    /// Unpublish an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/unpublish")]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnpublishArticle(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _articleService.UnpublishAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Article not found");
        }

        return ApiSuccess(true, "Article unpublished successfully");
    }

    /// <summary>
    /// Get article comments
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated list of comments</returns>
    [HttpGet("{id:long}/comments")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleCommentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleComments(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _articleService.GetCommentsAsync(id, page, pageSize);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Add a comment to an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <param name="request">Comment data</param>
    /// <returns>Created comment</returns>
    [HttpPost("{id:long}/comments")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleCommentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddArticleComment(long id, [FromBody] CreateArticleCommentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        request.ArticleId = id;
        var comment = await _articleService.CreateCommentAsync(request, CurrentUserId);
        return ApiCreated(comment, message: "Comment added successfully");
    }

    /// <summary>
    /// Get top articles
    /// </summary>
    /// <param name="count">Number of articles to return</param>
    /// <returns>List of top articles</returns>
    [HttpGet("top")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopArticles([FromQuery] int count = 10)
    {
        var articles = await _articleService.GetTopArticlesAsync(count);
        return ApiSuccess(articles);
    }

    /// <summary>
    /// Get recent articles
    /// </summary>
    /// <param name="count">Number of articles to return</param>
    /// <returns>List of recent articles</returns>
    [HttpGet("recent")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentArticles([FromQuery] int count = 10)
    {
        var articles = await _articleService.GetRecentArticlesAsync(count);
        return ApiSuccess(articles);
    }

    /// <summary>
    /// Get related articles
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <param name="count">Number of articles to return</param>
    /// <returns>List of related articles</returns>
    [HttpGet("{id:long}/related")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelatedArticles(long id, [FromQuery] int count = 5)
    {
        var articles = await _articleService.GetRelatedArticlesAsync(id, count);
        return ApiSuccess(articles);
    }
}