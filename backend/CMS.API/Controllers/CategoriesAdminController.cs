using CMS.API.Data;
using CMS.API.Models.DTOs;
using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Controllers;

/// <summary>
/// Categories Admin API controller for managing article and product categories
/// </summary>
[Route("api/admin/categories")]
[Authorize(Roles = "Admin,Editor")]
public class CategoriesAdminController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoriesAdminController> _logger;

    public CategoriesAdminController(ApplicationDbContext context, ILogger<CategoriesAdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Article Categories

    /// <summary>
    /// Get all article categories (tree structure)
    /// </summary>
    [HttpGet("articles")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ArticleCategoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleCategories([FromQuery] bool includeInactive = true)
    {
        var query = _context.ArticleCategories.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(c => c.Active == true);
        }

        var categories = await query
            .OrderBy(c => c.Sort)
            .ThenBy(c => c.Name)
            .Select(c => new ArticleCategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                Description = c.Description,
                Sort = c.Sort,
                Counter = c.Counter,
                DisplayMenu = c.DisplayMenu,
                Active = c.Active,
                CanDelete = c.CanDelete,
                CreateDate = c.CreateDate
            })
            .ToListAsync();

        return ApiSuccess(categories);
    }

    /// <summary>
    /// Get article category by ID
    /// </summary>
    [HttpGet("articles/{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticleCategoryById(long id)
    {
        var category = await _context.ArticleCategories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ArticleCategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                Description = c.Description,
                Sort = c.Sort,
                Counter = c.Counter,
                DisplayMenu = c.DisplayMenu,
                Active = c.Active,
                CanDelete = c.CanDelete,
                CreateDate = c.CreateDate
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return ApiNotFound("Article category not found");
        }

        return ApiSuccess(category);
    }

    /// <summary>
    /// Create a new article category
    /// </summary>
    [HttpPost("articles")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleCategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateArticleCategory([FromBody] CreateArticleCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var category = new ArticleCategory
        {
            ParentId = request.ParentId,
            Name = request.Name,
            Url = request.Url ?? GenerateSlug(request.Name),
            Image = request.Image,
            Description = request.Description,
            Sort = request.Sort ?? 0,
            DisplayMenu = request.DisplayMenu,
            Active = request.Active,
            CanDelete = true,
            CreateDate = DateTime.UtcNow
        };

        _context.ArticleCategories.Add(category);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Article category created: {Id} - {Name} by {UserId}", category.Id, category.Name, CurrentUserId);

        var result = new ArticleCategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Url = category.Url,
            Image = category.Image,
            Description = category.Description,
            Sort = category.Sort,
            DisplayMenu = category.DisplayMenu,
            Active = category.Active,
            CanDelete = category.CanDelete,
            CreateDate = category.CreateDate
        };

        return ApiCreated(result, $"/api/admin/categories/articles/{category.Id}", "Article category created successfully");
    }

    /// <summary>
    /// Update an article category
    /// </summary>
    [HttpPut("articles/{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ArticleCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateArticleCategory(long id, [FromBody] UpdateArticleCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var category = await _context.ArticleCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Article category not found");
        }

        category.ParentId = request.ParentId;
        category.Name = request.Name;
        category.Url = request.Url ?? GenerateSlug(request.Name);
        category.Image = request.Image;
        category.Description = request.Description;
        category.Sort = request.Sort ?? category.Sort;
        category.DisplayMenu = request.DisplayMenu;
        category.Active = request.Active;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Article category updated: {Id} by {UserId}", id, CurrentUserId);

        return ApiSuccess(new ArticleCategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Url = category.Url,
            Image = category.Image,
            Description = category.Description,
            Sort = category.Sort,
            DisplayMenu = category.DisplayMenu,
            Active = category.Active,
            CanDelete = category.CanDelete,
            CreateDate = category.CreateDate
        }, "Article category updated successfully");
    }

    /// <summary>
    /// Delete an article category
    /// </summary>
    [HttpDelete("articles/{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticleCategory(long id)
    {
        var category = await _context.ArticleCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Article category not found");
        }

        if (category.CanDelete == false)
        {
            return ApiError("This category cannot be deleted");
        }

        // Check if category has children
        var hasChildren = await _context.ArticleCategories.AnyAsync(c => c.ParentId == id);
        if (hasChildren)
        {
            return ApiError("Cannot delete category with subcategories. Please delete or move subcategories first.");
        }

        // Check if category has articles
        var hasArticles = await _context.ArticleCategoryArticles.AnyAsync(a => a.ArticleCategoryId == id);
        if (hasArticles)
        {
            return ApiError("Cannot delete category with articles. Please remove articles from this category first.");
        }

        _context.ArticleCategories.Remove(category);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Article category deleted: {Id} by {UserId}", id, CurrentUserId);

        return NoContent();
    }

    /// <summary>
    /// Toggle article category active status
    /// </summary>
    [HttpPost("articles/{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleArticleCategoryStatus(long id)
    {
        var category = await _context.ArticleCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Article category not found");
        }

        category.Active = !(category.Active ?? true);
        await _context.SaveChangesAsync();

        return ApiSuccess(category.Active ?? false, $"Category {(category.Active == true ? "activated" : "deactivated")} successfully");
    }

    #endregion

    #region Product Categories

    /// <summary>
    /// Get all product categories
    /// </summary>
    [HttpGet("products")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductCategoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductCategories([FromQuery] bool includeInactive = true)
    {
        var query = _context.ProductCategories.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(c => c.Active == true);
        }

        var categories = await query
            .OrderBy(c => c.Sort)
            .ThenBy(c => c.Name)
            .Select(c => new ProductCategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                Description = c.Description,
                Sort = c.Sort,
                Counter = c.Counter,
                DisplayMenu = c.DisplayMenu,
                MenuColor = c.MenuColor,
                Active = c.Active,
                CanDelete = c.CanDelete,
                CreateDate = c.CreateDate
            })
            .ToListAsync();

        return ApiSuccess(categories);
    }

    /// <summary>
    /// Get product category by ID
    /// </summary>
    [HttpGet("products/{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductCategoryById(long id)
    {
        var category = await _context.ProductCategories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ProductCategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                Description = c.Description,
                Sort = c.Sort,
                Counter = c.Counter,
                DisplayMenu = c.DisplayMenu,
                MenuColor = c.MenuColor,
                Active = c.Active,
                CanDelete = c.CanDelete,
                CreateDate = c.CreateDate
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return ApiNotFound("Product category not found");
        }

        return ApiSuccess(category);
    }

    /// <summary>
    /// Create a new product category
    /// </summary>
    [HttpPost("products")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductCategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductCategory([FromBody] CreateProductCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var category = new ProductCategory
        {
            ParentId = request.ParentId,
            Name = request.Name,
            Url = request.Url ?? GenerateSlug(request.Name),
            Image = request.Image,
            Description = request.Description,
            Sort = request.Sort ?? 0,
            DisplayMenu = request.DisplayMenu,
            MenuColor = request.MenuColor,
            Active = request.Active,
            CanDelete = true,
            CreateDate = DateTime.UtcNow
        };

        _context.ProductCategories.Add(category);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product category created: {Id} - {Name} by {UserId}", category.Id, category.Name, CurrentUserId);

        var result = new ProductCategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Url = category.Url,
            Image = category.Image,
            Description = category.Description,
            Sort = category.Sort,
            DisplayMenu = category.DisplayMenu,
            MenuColor = category.MenuColor,
            Active = category.Active,
            CanDelete = category.CanDelete,
            CreateDate = category.CreateDate
        };

        return ApiCreated(result, $"/api/admin/categories/products/{category.Id}", "Product category created successfully");
    }

    /// <summary>
    /// Update a product category
    /// </summary>
    [HttpPut("products/{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductCategory(long id, [FromBody] UpdateProductCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Product category not found");
        }

        category.ParentId = request.ParentId;
        category.Name = request.Name;
        category.Url = request.Url ?? GenerateSlug(request.Name);
        category.Image = request.Image;
        category.Description = request.Description;
        category.Sort = request.Sort ?? category.Sort;
        category.DisplayMenu = request.DisplayMenu;
        category.MenuColor = request.MenuColor;
        category.Active = request.Active;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Product category updated: {Id} by {UserId}", id, CurrentUserId);

        return ApiSuccess(new ProductCategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Url = category.Url,
            Image = category.Image,
            Description = category.Description,
            Sort = category.Sort,
            DisplayMenu = category.DisplayMenu,
            MenuColor = category.MenuColor,
            Active = category.Active,
            CanDelete = category.CanDelete,
            CreateDate = category.CreateDate
        }, "Product category updated successfully");
    }

    /// <summary>
    /// Delete a product category
    /// </summary>
    [HttpDelete("products/{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductCategory(long id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Product category not found");
        }

        if (category.CanDelete == false)
        {
            return ApiError("This category cannot be deleted");
        }

        // Check if category has children
        var hasChildren = await _context.ProductCategories.AnyAsync(c => c.ParentId == id);
        if (hasChildren)
        {
            return ApiError("Cannot delete category with subcategories. Please delete or move subcategories first.");
        }

        // Check if category has products
        var hasProducts = await _context.ProductCategoryProducts.AnyAsync(p => p.ProductCategoryId == id);
        if (hasProducts)
        {
            return ApiError("Cannot delete category with products. Please remove products from this category first.");
        }

        _context.ProductCategories.Remove(category);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product category deleted: {Id} by {UserId}", id, CurrentUserId);

        return NoContent();
    }

    /// <summary>
    /// Toggle product category active status
    /// </summary>
    [HttpPost("products/{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleProductCategoryStatus(long id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
        {
            return ApiNotFound("Product category not found");
        }

        category.Active = !(category.Active ?? true);
        await _context.SaveChangesAsync();

        return ApiSuccess(category.Active ?? false, $"Category {(category.Active == true ? "activated" : "deactivated")} successfully");
    }

    #endregion

    #region Helper Methods

    private static string GenerateSlug(string name)
    {
        var slug = name
            .ToLowerInvariant()
            .Normalize(System.Text.NormalizationForm.FormD);

        var sb = new System.Text.StringBuilder();
        foreach (var c in slug)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        slug = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        slug = slug.Replace('đ', 'd').Replace('Đ', 'd');
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
        slug = slug.Trim('-');

        return slug;
    }

    #endregion
}

