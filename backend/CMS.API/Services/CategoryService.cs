using CMS.API.Data;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public interface ICategoryService
{
    // Article Categories
    Task<List<CategoryResponse>> GetArticleCategoriesAsync(int? parentId = null, bool includeInactive = false);
    Task<List<CategoryTreeResponse>> GetArticleCategoryTreeAsync(bool includeInactive = false);
    Task<CategoryResponse?> GetArticleCategoryByIdAsync(int id);
    Task<CategoryResponse?> GetArticleCategoryByUrlAsync(string url);
    Task<List<CategoryResponse>> GetArticleCategoryBreadcrumbAsync(int categoryId);

    // Product Categories
    Task<List<CategoryResponse>> GetProductCategoriesAsync(int? parentId = null, bool includeInactive = false);
    Task<List<CategoryTreeResponse>> GetProductCategoryTreeAsync(bool includeInactive = false);
    Task<CategoryResponse?> GetProductCategoryByIdAsync(int id);
    Task<CategoryResponse?> GetProductCategoryByUrlAsync(string url);
    Task<List<CategoryResponse>> GetProductCategoryBreadcrumbAsync(int categoryId);
}

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<CategoryResponse>> GetArticleCategoriesAsync(int? parentId = null, bool includeInactive = false)
    {
        var query = _context.ArticleCategories.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(c => c.Active == true);
        }

        if (parentId.HasValue)
        {
            query = query.Where(c => c.ParentId == parentId.Value);
        }

        return await query
            .OrderBy(c => c.Sort)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .ToListAsync();
    }

    public async Task<List<CategoryTreeResponse>> GetArticleCategoryTreeAsync(bool includeInactive = false)
    {
        var categories = await GetArticleCategoriesAsync(null, includeInactive);
        return BuildCategoryTree(categories, null);
    }

    public async Task<CategoryResponse?> GetArticleCategoryByIdAsync(int id)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryResponse?> GetArticleCategoryByUrlAsync(string url)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .Where(c => c.Url == url && c.Active == true)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CategoryResponse>> GetArticleCategoryBreadcrumbAsync(int categoryId)
    {
        var breadcrumb = new List<CategoryResponse>();
        long currentId = categoryId;

        while (currentId > 0)
        {
            var category = await GetArticleCategoryByIdAsync((int)currentId);
            if (category == null) break;

            breadcrumb.Insert(0, category);
            currentId = category.ParentId ?? 0;
        }

        return breadcrumb;
    }

    public async Task<List<CategoryResponse>> GetProductCategoriesAsync(int? parentId = null, bool includeInactive = false)
    {
        var query = _context.ProductCategories.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(c => c.Active == true);
        }

        if (parentId.HasValue)
        {
            query = query.Where(c => c.ParentId == parentId.Value);
        }

        return await query
            .OrderBy(c => c.Sort)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .ToListAsync();
    }

    public async Task<List<CategoryTreeResponse>> GetProductCategoryTreeAsync(bool includeInactive = false)
    {
        var categories = await GetProductCategoriesAsync(null, includeInactive);
        return BuildCategoryTree(categories, null);
    }

    public async Task<CategoryResponse?> GetProductCategoryByIdAsync(int id)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryResponse?> GetProductCategoryByUrlAsync(string url)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .Where(c => c.Url == url && c.Active == true)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                ParentId = c.ParentId,
                Sort = c.Sort ?? 0,
                Active = c.Active ?? false
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CategoryResponse>> GetProductCategoryBreadcrumbAsync(int categoryId)
    {
        var breadcrumb = new List<CategoryResponse>();
        long currentId = categoryId;

        while (currentId > 0)
        {
            var category = await GetProductCategoryByIdAsync((int)currentId);
            if (category == null) break;

            breadcrumb.Insert(0, category);
            currentId = category.ParentId ?? 0;
        }

        return breadcrumb;
    }

    private List<CategoryTreeResponse> BuildCategoryTree(List<CategoryResponse> categories, long? parentId)
    {
        return categories
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.Sort)
            .Select(c => new CategoryTreeResponse
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url,
                Image = c.Image,
                ParentId = c.ParentId,
                Sort = c.Sort,
                Active = c.Active,
                Children = BuildCategoryTree(categories, c.Id)
            })
            .ToList();
    }
}
