using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProductDto?> GetByIdAsync(long id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product != null ? MapToProductDto(product) : null;
    }

    public async Task<ProductDto?> GetByUrlAsync(string url)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Url == url);
        return product != null ? MapToProductDto(product) : null;
    }

    public async Task<ProductDto?> GetByCodeAsync(string code)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Sku == code);
        return product != null ? MapToProductDto(product) : null;
    }

    public async Task<PagedResult<ProductListDto>> SearchAsync(ProductSearchRequest request)
    {
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(p => (p.Name != null && p.Name.ToLower().Contains(keyword)) || (p.Sku != null && p.Sku.ToLower().Contains(keyword)));
        }

        if (request.ProductCategoryId.HasValue)
            query = query.Where(p => p.ProductCategoryIds != null && p.ProductCategoryIds.Contains(request.ProductCategoryId.Value.ToString()));

        if (request.ProductBrandId.HasValue) query = query.Where(p => p.ProductBrandId == request.ProductBrandId);
        if (request.Active.HasValue) query = query.Where(p => p.Active == request.Active);
        if (request.MinPrice.HasValue) query = query.Where(p => p.Price >= request.MinPrice);
        if (request.MaxPrice.HasValue) query = query.Where(p => p.Price <= request.MaxPrice);

        var totalCount = await query.CountAsync();
        var products = await query.OrderByDescending(p => p.CreateDate).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

        return new PagedResult<ProductListDto>
        {
            Items = products.Select(MapToProductListDto).ToList(),
            Pagination = new PaginationInfo { CurrentPage = request.Page, PageSize = request.PageSize, TotalCount = totalCount, TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize) }
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, string userId)
    {
        var product = new Product
        {
            Name = request.Name, SubTitle = request.SubTitle, Image = request.Image, Description = request.Description, Content = request.Content,
            Price = request.Price, PriceOld = request.PriceOld, ProductBrandId = request.ProductBrandId,
            ProductCategoryIds = request.CategoryIds != null ? string.Join(",", request.CategoryIds) : null,
            Url = request.Url ?? GenerateUrl(request.Name), Active = false, Approved = 0, CreateBy = userId, CreateDate = DateTime.UtcNow,
            MetaTitle = request.MetaTitle, MetaDescription = request.MetaDescription, MetaKeywords = request.MetaKeywords
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return MapToProductDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(long id, UpdateProductRequest request, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return null;

        product.Name = request.Name; product.SubTitle = request.SubTitle; product.Image = request.Image;
        product.Description = request.Description; product.Content = request.Content;
        product.Price = request.Price; product.PriceOld = request.PriceOld; product.ProductBrandId = request.ProductBrandId;
        product.ProductCategoryIds = request.CategoryIds != null ? string.Join(",", request.CategoryIds) : null;
        product.Url = request.Url ?? product.Url; product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        product.MetaTitle = request.MetaTitle; product.MetaDescription = request.MetaDescription; product.MetaKeywords = request.MetaKeywords;

        await _context.SaveChangesAsync();
        return MapToProductDto(product);
    }

    public async Task<bool> DeleteAsync(long id, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null || product.CanDelete != true) return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveAsync(long id, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;
        product.Approved = 1; product.Active = true; product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectAsync(long id, string userId, string? reason = null)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;
        product.Approved = 2; product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishAsync(long id, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;
        product.Active = true; product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnpublishAsync(long id, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;
        product.Active = false; product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductCategoryDto>> GetCategoriesAsync(long productId)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null || string.IsNullOrEmpty(product.ProductCategoryIds)) return new List<ProductCategoryDto>();

        var categoryIds = product.ProductCategoryIds.Split(',').Select(int.Parse).ToList();
        var categories = await _context.ProductCategories.AsNoTracking().Where(c => categoryIds.Contains((int)c.Id)).ToListAsync();

        return categories.Select(c => new ProductCategoryDto { Id = (int)c.Id, Name = c.Name, Url = c.Url, Image = c.Image }).ToList();
    }

    public async Task<bool> UpdateCategoriesAsync(long productId, List<int> categoryIds, string userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null) return false;
        product.ProductCategoryIds = string.Join(",", categoryIds); product.LastEditBy = userId; product.LastEditDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductPictureDto>> GetPicturesAsync(long productId)
    {
        var pictures = await _context.ProductPictures.AsNoTracking().Where(p => p.ProductId == productId).OrderBy(p => p.Sort).ToListAsync();
        return pictures.Select(p => new ProductPictureDto { Id = int.TryParse(p.Id, out var id) ? id : 0, ProductId = p.ProductId ?? 0, Image = p.Image, Sort = p.Sort ?? 0 }).ToList();
    }

    public async Task<ProductPictureDto> AddPictureAsync(long productId, string name, string image, int sort, string userId)
    {
        var picture = new ProductPicture { Id = Guid.NewGuid().ToString(), ProductId = productId, Image = image, Sort = sort, CreateBy = userId, CreateDate = DateTime.UtcNow };
        await _context.ProductPictures.AddAsync(picture);
        await _context.SaveChangesAsync();
        return new ProductPictureDto { Id = int.TryParse(picture.Id, out var id) ? id : 0, ProductId = productId, Image = image, Sort = sort };
    }

    public async Task<bool> UpdatePictureSortAsync(int pictureId, int sort, string userId)
    {
        var picture = await _context.ProductPictures.FirstOrDefaultAsync(p => p.Id == pictureId.ToString());
        if (picture == null) return false;
        picture.Sort = sort;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePictureAsync(int pictureId, string userId)
    {
        var picture = await _context.ProductPictures.FirstOrDefaultAsync(p => p.Id == pictureId.ToString());
        if (picture == null) return false;
        _context.ProductPictures.Remove(picture);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductAttachFileDto>> GetAttachFilesAsync(long productId)
    {
        var files = await _context.ProductAttachFiles.AsNoTracking().Where(f => f.ProductId == productId).ToListAsync();
        return files.Select(f => new ProductAttachFileDto { Id = (int)f.Id, ProductId = f.ProductId ?? 0, Name = f.AttachFileName, FileType = f.FileType, FileSize = (int?)(f.FileSize) }).ToList();
    }

    public async Task<ProductAttachFileDto> AddAttachFileAsync(long productId, string fileName, string filePath, string fileType, int fileSize, string userId)
    {
        var file = new ProductAttachFile { ProductId = productId, AttachFileName = fileName, FileType = fileType, FileSize = fileSize, CreateBy = userId, CreateDate = DateTime.UtcNow };
        await _context.ProductAttachFiles.AddAsync(file);
        await _context.SaveChangesAsync();
        return new ProductAttachFileDto { Id = (int)file.Id, ProductId = productId, Name = fileName, FileType = fileType, FileSize = fileSize };
    }

    public async Task<bool> DeleteAttachFileAsync(int attachFileId, string userId)
    {
        var file = await _context.ProductAttachFiles.FirstOrDefaultAsync(f => f.Id == attachFileId);
        if (file == null) return false;
        _context.ProductAttachFiles.Remove(file);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductPropertyValueDto>> GetPropertiesAsync(long productId)
    {
        var properties = await _context.ProductPropertyValues.AsNoTracking().Where(p => p.ProductId == productId).ToListAsync();
        return properties.Select(p => new ProductPropertyValueDto { Id = p.Id, ProductId = p.ProductId, Value = p.Value }).ToList();
    }

    public async Task<bool> UpdatePropertiesAsync(long productId, List<ProductPropertyValueRequest> properties, string userId)
    {
        var existing = await _context.ProductPropertyValues.Where(p => p.ProductId == productId).ToListAsync();
        _context.ProductPropertyValues.RemoveRange(existing);

        foreach (var prop in properties)
        {
            await _context.ProductPropertyValues.AddAsync(new ProductPropertyValue { ProductId = productId, Value = prop.Value });
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResult<ProductReviewDto>> GetReviewsAsync(long productId, int page = 1, int pageSize = 20)
    {
        var query = _context.ProductReviews.AsNoTracking().Where(r => r.ProductId == productId);
        var totalCount = await query.CountAsync();
        var reviews = await query.OrderByDescending(r => r.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<ProductReviewDto>
        {
            Items = reviews.Select(r => new ProductReviewDto { Id = (int)r.Id, ProductId = r.ProductId ?? 0, Name = r.CustomerName, Email = r.Email, Content = r.Content, Rating = r.Star, Active = r.Active, CreateDate = r.CreateDate }).ToList(),
            Pagination = new PaginationInfo { CurrentPage = page, PageSize = pageSize, TotalCount = totalCount, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) }
        };
    }

    public async Task<ProductReviewDto> CreateReviewAsync(CreateProductReviewRequest request, string? userId = null)
    {
        var review = new ProductReview { ProductId = request.ProductId, CustomerName = request.Name, Email = request.Email, Content = request.Content, Star = request.Rating, Active = false, CreateDate = DateTime.UtcNow };
        await _context.ProductReviews.AddAsync(review);
        await _context.SaveChangesAsync();
        return new ProductReviewDto { Id = (int)review.Id, ProductId = review.ProductId ?? 0, Name = review.CustomerName, Email = review.Email, Content = review.Content, Rating = review.Star, Active = review.Active, CreateDate = review.CreateDate };
    }

    public async Task<bool> ApproveReviewAsync(int reviewId, string userId)
    {
        var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null) return false;
        review.Active = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId, string userId)
    {
        var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null) return false;
        _context.ProductReviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResult<ProductCommentDto>> GetCommentsAsync(long productId, int page = 1, int pageSize = 20)
    {
        var query = _context.ProductComments.AsNoTracking().Where(c => c.ProductId == productId && c.ParentId == null);
        var totalCount = await query.CountAsync();
        var comments = await query.OrderByDescending(c => c.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<ProductCommentDto>
        {
            Items = comments.Select(c => new ProductCommentDto { Id = (int)c.Id, ProductId = c.ProductId ?? 0, Name = c.Name, Email = c.Email, Content = c.Content, Active = c.Active, CreateDate = c.CreateDate }).ToList(),
            Pagination = new PaginationInfo { CurrentPage = page, PageSize = pageSize, TotalCount = totalCount, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) }
        };
    }

    public async Task<ProductCommentDto> CreateCommentAsync(CreateProductCommentRequest request, string? userId = null)
    {
        var comment = new ProductComment { ProductId = request.ProductId, Name = request.Name, Email = request.Email, Content = request.Content, Active = false, CreateBy = userId, CreateDate = DateTime.UtcNow };
        await _context.ProductComments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return new ProductCommentDto { Id = (int)comment.Id, ProductId = comment.ProductId ?? 0, Name = comment.Name, Email = comment.Email, Content = comment.Content, Active = comment.Active, CreateDate = comment.CreateDate };
    }

    public async Task<bool> ApproveCommentAsync(int commentId, string userId)
    {
        var comment = await _context.ProductComments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;
        comment.Active = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _context.ProductComments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;
        _context.ProductComments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    // Inventory - Simplified (Product entity doesn't have Quantity/QuantitySold)
    public Task<bool> UpdateQuantityAsync(long productId, int quantity, string userId) => Task.FromResult(true);
    public Task<bool> IncrementQuantitySoldAsync(long productId, int quantity) => Task.FromResult(true);
    public Task<bool> DecrementQuantityAsync(long productId, int quantity) => Task.FromResult(true);

    // Statistics
    public async Task<int> GetTotalCountAsync() => await _context.Products.CountAsync();
    public async Task<int> GetActiveCountAsync() => await _context.Products.CountAsync(p => p.Active == true);
    public async Task<int> GetOutOfStockCountAsync() => await _context.Products.CountAsync(p => p.IsOutStock == true);
    public async Task IncrementCounterAsync(long id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product != null) { product.Counter = (product.Counter ?? 0) + 1; await _context.SaveChangesAsync(); }
    }
    public async Task<List<ProductListDto>> GetTopProductsAsync(int count = 10) =>
        (await _context.Products.AsNoTracking().Where(p => p.Active == true).OrderByDescending(p => p.Counter).Take(count).ToListAsync()).Select(MapToProductListDto).ToList();
    public async Task<List<ProductListDto>> GetRecentProductsAsync(int count = 10) =>
        (await _context.Products.AsNoTracking().Where(p => p.Active == true).OrderByDescending(p => p.CreateDate).Take(count).ToListAsync()).Select(MapToProductListDto).ToList();
    public async Task<List<ProductListDto>> GetRelatedProductsAsync(long productId, int count = 5) => new List<ProductListDto>();
    public async Task<List<ProductListDto>> GetBestSellersAsync(int count = 10) =>
        (await _context.Products.AsNoTracking().Where(p => p.Active == true).OrderByDescending(p => p.SellCount).Take(count).ToListAsync()).Select(MapToProductListDto).ToList();

    private ProductDto MapToProductDto(Product p) => new ProductDto
    {
        Id = (int)p.Id, Name = p.Name, SubTitle = p.SubTitle, Image = p.Image, Description = p.Description, Content = p.Content,
        Code = p.Sku, Price = p.Price, PriceOld = p.PriceOld, Url = p.Url,
        Tags = p.Tags, Counter = p.Counter, Active = p.Active, Approved = p.Approved, StartDate = p.StartDate, EndDate = p.EndDate,
        CreateBy = p.CreateBy, CreateDate = p.CreateDate, LastEditBy = p.LastEditBy, LastEditDate = p.LastEditDate,
        MetaTitle = p.MetaTitle, MetaDescription = p.MetaDescription, MetaKeywords = p.MetaKeywords
    };

    private ProductListDto MapToProductListDto(Product p) => new ProductListDto
    {
        Id = (int)p.Id, Name = p.Name, Code = p.Sku, Image = p.Image, Description = p.Description, Price = p.Price, PriceOld = p.PriceOld,
        Url = p.Url, Counter = p.Counter, Active = p.Active, CreateDate = p.CreateDate
    };

    private string GenerateUrl(string? name)
    {
        if (string.IsNullOrEmpty(name)) return Guid.NewGuid().ToString();
        return name.ToLower().Replace(" ", "-").Replace("đ", "d")
            .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
            .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
            .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
            .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
            .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
            .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
            .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
            .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
            .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
            .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
            .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
            .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");
    }
}
