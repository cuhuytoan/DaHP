using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class BlockService : IBlockService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlockService> _logger;

    public BlockService(ApplicationDbContext context, ILogger<BlockService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== Article Block Operations =====================

    public async Task<PagedResult<ArticleBlockListDto>> GetArticleBlocksAsync(
        int page = 1,
        int pageSize = 20,
        int? categoryId = null,
        string? keyword = null,
        bool? active = null)
    {
        var query = _context.ArticleBlocks
            .Include(b => b.ArticleBlockArticles)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.ArticleCategoryId == categoryId);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(b => b.Name != null && b.Name.Contains(keyword));
        }

        if (active.HasValue)
        {
            query = query.Where(b => b.Active == active);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Sort)
            .ThenByDescending(b => b.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ArticleBlockListDto
            {
                Id = b.Id,
                ArticleCategoryId = b.ArticleCategoryId,
                Name = b.Name,
                Description = b.Description,
                Image = b.Image,
                Active = b.Active,
                Sort = b.Sort,
                StyleId = b.StyleId,
                ArticleCount = b.ArticleBlockArticles.Count,
                CreateDate = b.CreateDate
            })
            .ToListAsync();

        return new PagedResult<ArticleBlockListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ArticleBlockDetailDto?> GetArticleBlockByIdAsync(long id)
    {
        var block = await _context.ArticleBlocks
            .Include(b => b.ArticleBlockArticles)
                .ThenInclude(ba => ba.Article)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return null;

        return new ArticleBlockDetailDto
        {
            Id = block.Id,
            ArticleCategoryId = block.ArticleCategoryId,
            Name = block.Name,
            Description = block.Description,
            Image = block.Image,
            Active = block.Active,
            Sort = block.Sort,
            StyleId = block.StyleId,
            CanDelete = block.CanDelete,
            CreateBy = block.CreateBy,
            CreateDate = block.CreateDate,
            LastEditedBy = block.LastEditedBy,
            LastEditedDate = block.LastEditedDate,
            Articles = block.ArticleBlockArticles
                .Where(ba => ba.Article != null)
                .Select(ba => new BlockArticleDto
                {
                    Id = ba.Article!.Id,
                    Name = ba.Article.Name,
                    Image = ba.Article.Image,
                    Url = ba.Article.Url,
                    Active = ba.Article.Active,
                    CreateDate = ba.Article.CreateDate
                })
                .ToList()
        };
    }

    public async Task<ArticleBlockDetailDto> CreateArticleBlockAsync(CreateArticleBlockRequest request, string userId)
    {
        var block = new ArticleBlock
        {
            ArticleCategoryId = request.ArticleCategoryId,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Sort = request.Sort,
            StyleId = request.StyleId,
            Active = request.Active,
            CanDelete = true,
            CreateBy = userId,
            CreateDate = DateTime.Now
        };

        _context.ArticleBlocks.Add(block);
        await _context.SaveChangesAsync();

        return (await GetArticleBlockByIdAsync(block.Id))!;
    }

    public async Task<ArticleBlockDetailDto?> UpdateArticleBlockAsync(long id, UpdateArticleBlockRequest request, string userId)
    {
        var block = await _context.ArticleBlocks.FindAsync(id);
        if (block == null) return null;

        block.ArticleCategoryId = request.ArticleCategoryId;
        block.Name = request.Name;
        block.Description = request.Description;
        block.Image = request.Image;
        block.Sort = request.Sort;
        block.StyleId = request.StyleId;
        block.Active = request.Active;
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetArticleBlockByIdAsync(id);
    }

    public async Task<bool> DeleteArticleBlockAsync(long id)
    {
        var block = await _context.ArticleBlocks
            .Include(b => b.ArticleBlockArticles)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return false;
        if (block.CanDelete == false) return false;

        _context.ArticleBlockArticles.RemoveRange(block.ArticleBlockArticles);
        _context.ArticleBlocks.Remove(block);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleArticleBlockStatusAsync(long id, string userId)
    {
        var block = await _context.ArticleBlocks.FindAsync(id);
        if (block == null) return false;

        block.Active = !(block.Active ?? false);
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddArticlesToBlockAsync(long blockId, List<int> articleIds)
    {
        var block = await _context.ArticleBlocks.FindAsync(blockId);
        if (block == null) return false;

        // Get existing article IDs in the block
        var existingIds = await _context.ArticleBlockArticles
            .Where(ba => ba.ArticleBlockId == (int)blockId)
            .Select(ba => ba.ArticleId)
            .ToListAsync();

        // Add only new articles
        foreach (var articleId in articleIds.Where(id => !existingIds.Contains(id)))
        {
            _context.ArticleBlockArticles.Add(new ArticleBlockArticle
            {
                ArticleBlockId = (int)blockId,
                ArticleId = articleId
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveArticleFromBlockAsync(long blockId, int articleId)
    {
        var item = await _context.ArticleBlockArticles
            .FirstOrDefaultAsync(ba => ba.ArticleBlockId == (int)blockId && ba.ArticleId == articleId);

        if (item == null) return false;

        _context.ArticleBlockArticles.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Product Block Operations =====================

    public async Task<PagedResult<ProductBlockListDto>> GetProductBlocksAsync(
        int page = 1,
        int pageSize = 20,
        int? categoryId = null,
        string? keyword = null,
        bool? active = null)
    {
        var query = _context.ProductBlocks
            .Include(b => b.ProductBlockProducts)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.ProductCategoryId == categoryId);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(b => b.Name != null && b.Name.Contains(keyword));
        }

        if (active.HasValue)
        {
            query = query.Where(b => b.Active == active);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Sort)
            .ThenByDescending(b => b.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ProductBlockListDto
            {
                Id = b.Id,
                ProductCategoryId = b.ProductCategoryId,
                Name = b.Name,
                Description = b.Description,
                Image = b.Image,
                Active = b.Active,
                Sort = b.Sort,
                StyleId = b.StyleId,
                ProductCount = b.ProductBlockProducts.Count,
                CreateDate = b.CreateDate
            })
            .ToListAsync();

        return new PagedResult<ProductBlockListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ProductBlockDetailDto?> GetProductBlockByIdAsync(long id)
    {
        var block = await _context.ProductBlocks
            .Include(b => b.ProductBlockProducts)
                .ThenInclude(bp => bp.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return null;

        return new ProductBlockDetailDto
        {
            Id = block.Id,
            ProductCategoryId = block.ProductCategoryId,
            Name = block.Name,
            Description = block.Description,
            Image = block.Image,
            Active = block.Active,
            Sort = block.Sort,
            StyleId = block.StyleId,
            CanDelete = block.CanDelete,
            CreateBy = block.CreateBy,
            CreateDate = block.CreateDate,
            LastEditedBy = block.LastEditedBy,
            LastEditedDate = block.LastEditedDate,
            Products = block.ProductBlockProducts
                .Where(bp => bp.Product != null)
                .Select(bp => new BlockProductDto
                {
                    Id = bp.Product!.Id,
                    Name = bp.Product.Name,
                    Image = bp.Product.Image,
                    Sku = bp.Product.Sku,
                    Price = bp.Product.Price,
                    Active = bp.Product.Active,
                    CreateDate = bp.Product.CreateDate
                })
                .ToList()
        };
    }

    public async Task<ProductBlockDetailDto> CreateProductBlockAsync(CreateProductBlockRequest request, string userId)
    {
        var block = new ProductBlock
        {
            ProductCategoryId = request.ProductCategoryId,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Sort = request.Sort,
            StyleId = request.StyleId,
            Active = request.Active,
            CanDelete = true,
            CreateBy = userId,
            CreateDate = DateTime.Now
        };

        _context.ProductBlocks.Add(block);
        await _context.SaveChangesAsync();

        return (await GetProductBlockByIdAsync(block.Id))!;
    }

    public async Task<ProductBlockDetailDto?> UpdateProductBlockAsync(long id, UpdateProductBlockRequest request, string userId)
    {
        var block = await _context.ProductBlocks.FindAsync(id);
        if (block == null) return null;

        block.ProductCategoryId = request.ProductCategoryId;
        block.Name = request.Name;
        block.Description = request.Description;
        block.Image = request.Image;
        block.Sort = request.Sort;
        block.StyleId = request.StyleId;
        block.Active = request.Active;
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetProductBlockByIdAsync(id);
    }

    public async Task<bool> DeleteProductBlockAsync(long id)
    {
        var block = await _context.ProductBlocks
            .Include(b => b.ProductBlockProducts)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return false;
        if (block.CanDelete == false) return false;

        _context.ProductBlockProducts.RemoveRange(block.ProductBlockProducts);
        _context.ProductBlocks.Remove(block);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleProductBlockStatusAsync(long id, string userId)
    {
        var block = await _context.ProductBlocks.FindAsync(id);
        if (block == null) return false;

        block.Active = !(block.Active ?? false);
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddProductsToBlockAsync(long blockId, List<int> productIds)
    {
        var block = await _context.ProductBlocks.FindAsync(blockId);
        if (block == null) return false;

        // Get existing product IDs in the block
        var existingIds = await _context.ProductBlockProducts
            .Where(bp => bp.ProductBlockId == (int)blockId)
            .Select(bp => bp.ProductId)
            .ToListAsync();

        // Add only new products
        foreach (var productId in productIds.Where(id => !existingIds.Contains(id)))
        {
            _context.ProductBlockProducts.Add(new ProductBlockProduct
            {
                ProductBlockId = (int)blockId,
                ProductId = productId
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveProductFromBlockAsync(long blockId, int productId)
    {
        var item = await _context.ProductBlockProducts
            .FirstOrDefaultAsync(bp => bp.ProductBlockId == (int)blockId && bp.ProductId == productId);

        if (item == null) return false;

        _context.ProductBlockProducts.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
