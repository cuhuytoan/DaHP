using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class ArticleService : IArticleService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(ApplicationDbContext context, ILogger<ArticleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ArticleDto?> GetByIdAsync(long id)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return null;

        return MapToArticleDto(article);
    }

    public async Task<ArticleDto?> GetByUrlAsync(string url)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Url == url && a.Active == true);

        if (article == null) return null;

        return MapToArticleDto(article);
    }

    public async Task<PagedResult<ArticleListDto>> SearchAsync(ArticleSearchRequest request)
    {
        var query = _context.Articles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(a => 
                (a.Name != null && a.Name.ToLower().Contains(keyword)) ||
                (a.Description != null && a.Description.ToLower().Contains(keyword)));
        }

        if (request.ArticleCategoryId.HasValue)
        {
            var categoryId = request.ArticleCategoryId.Value.ToString();
            query = query.Where(a => a.ArticleCategoryIds != null && a.ArticleCategoryIds.Contains(categoryId));
        }

        if (request.Active.HasValue)
        {
            query = query.Where(a => a.Active == request.Active);
        }

        if (request.ArticleStatusId.HasValue)
        {
            query = query.Where(a => a.ArticleStatusId == request.ArticleStatusId);
        }

        var totalCount = await query.CountAsync();

        var articles = await query
            .OrderByDescending(a => a.CreateDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var items = articles.Select(a => new ArticleListDto
        {
            Id = (int)a.Id,
            Name = a.Name,
            SubTitle = a.SubTitle,
            Image = a.Image,
            Description = a.Description,
            Url = a.Url,
            Counter = a.Counter ?? 0,
            Active = a.Active ?? false,
            ArticleStatusId = a.ArticleStatusId,
            CreateDate = a.CreateDate
        }).ToList();

        return new PagedResult<ArticleListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            }
        };
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleRequest request, string userId)
    {
        var article = new Article
        {
            Name = request.Name,
            SubTitle = request.SubTitle,
            Image = request.Image,
            Description = request.Description,
            Content = request.Content,
            ArticleCategoryIds = request.CategoryIds != null ? string.Join(",", request.CategoryIds) : null,
            ArticleTypeId = request.ArticleTypeId,
            Url = request.Url ?? GenerateUrl(request.Name),
            Active = false,
            Approved = 0,
            CreateBy = userId,
            CreateDate = DateTime.UtcNow,
            MetaTitle = request.MetaTitle,
            MetaDescription = request.MetaDescription,
            MetaKeywords = request.MetaKeywords
        };

        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();

        return MapToArticleDto(article);
    }

    public async Task<ArticleDto?> UpdateAsync(long id, UpdateArticleRequest request, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return null;

        article.Name = request.Name;
        article.SubTitle = request.SubTitle;
        article.Image = request.Image;
        article.Description = request.Description;
        article.Content = request.Content;
        article.ArticleCategoryIds = request.CategoryIds != null ? string.Join(",", request.CategoryIds) : null;
        article.Url = request.Url ?? article.Url;
        article.LastEditBy = userId;
        article.LastEditDate = DateTime.UtcNow;
        article.MetaTitle = request.MetaTitle;
        article.MetaDescription = request.MetaDescription;
        article.MetaKeywords = request.MetaKeywords;

        await _context.SaveChangesAsync();

        return MapToArticleDto(article);
    }

    public async Task<bool> DeleteAsync(long id, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ApproveAsync(long id, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;

        article.Approved = 1;
        article.ApproveBy = userId;
        article.ApproveDate = DateTime.UtcNow;
        article.Active = true;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectAsync(long id, string userId, string? reason = null)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;

        article.Approved = -1;
        article.ApproveBy = userId;
        article.ApproveDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishAsync(long id, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;

        article.Active = true;
        article.StartDate = DateTime.UtcNow;
        article.LastEditBy = userId;
        article.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnpublishAsync(long id, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;

        article.Active = false;
        article.EndDate = DateTime.UtcNow;
        article.LastEditBy = userId;
        article.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ArticleCategoryDto>> GetCategoriesAsync(long articleId)
    {
        var article = await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null || string.IsNullOrEmpty(article.ArticleCategoryIds))
            return new List<ArticleCategoryDto>();

        var categoryIds = article.ArticleCategoryIds.Split(',').Select(int.Parse).ToList();
        var categories = await _context.ArticleCategories
            .AsNoTracking()
            .Where(c => categoryIds.Contains((int)c.Id))
            .Select(c => new ArticleCategoryDto
            {
                Id = (int)c.Id,
                Name = c.Name,
                Url = c.Url
            })
            .ToListAsync();

        return categories;
    }

    public async Task<bool> UpdateCategoriesAsync(long articleId, List<int> categoryIds, string userId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null) return false;

        article.ArticleCategoryIds = string.Join(",", categoryIds);
        article.LastEditBy = userId;
        article.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResult<ArticleCommentDto>> GetCommentsAsync(long articleId, int page = 1, int pageSize = 20)
    {
        var query = _context.ArticleComments
            .AsNoTracking()
            .Where(c => c.ArticleId == articleId);

        var totalCount = await query.CountAsync();
        var comments = await query
            .OrderByDescending(c => c.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ArticleCommentDto
            {
                Id = (int)c.Id,
                ArticleId = c.ArticleId ?? 0,
                Name = c.Name,
                Email = c.Email,
                Content = c.Content,
                Active = c.Active ?? false,
                CreateDate = c.CreateDate
            })
            .ToListAsync();

        return new PagedResult<ArticleCommentDto>
        {
            Items = comments,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }
        };
    }

    public async Task<ArticleCommentDto> CreateCommentAsync(CreateArticleCommentRequest request, string? userId = null)
    {
        var comment = new ArticleComment
        {
            ArticleId = request.ArticleId,
            Name = request.Name,
            Email = request.Email,
            Content = request.Content,
            Active = false,
            CreateBy = userId,
            CreateDate = DateTime.UtcNow
        };

        await _context.ArticleComments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new ArticleCommentDto
        {
            Id = (int)comment.Id,
            ArticleId = comment.ArticleId ?? 0,
            Name = comment.Name,
            Email = comment.Email,
            Content = comment.Content,
            Active = comment.Active ?? false,
            CreateDate = comment.CreateDate
        };
    }

    public async Task<bool> ApproveCommentAsync(long commentId, string userId)
    {
        var comment = await _context.ArticleComments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;

        comment.Active = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCommentAsync(long commentId, string userId)
    {
        var comment = await _context.ArticleComments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;

        _context.ArticleComments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ArticleAttachFileDto>> GetAttachFilesAsync(long articleId)
    {
        return await _context.ArticleAttachFiles
            .AsNoTracking()
            .Where(f => f.ArticleId == articleId)
            .Select(f => new ArticleAttachFileDto
            {
                Id = (int)f.Id,
                ArticleId = f.ArticleId ?? 0,
                Name = f.AttachFileName,
                FileType = f.FileType,
                FileSize = (int?)(f.FileSize)
            })
            .ToListAsync();
    }

    public async Task<ArticleAttachFileDto> AddAttachFileAsync(long articleId, string fileName, string filePath, string fileType, int fileSize, string userId)
    {
        var attachFile = new ArticleAttachFile
        {
            ArticleId = articleId,
            AttachFileName = fileName,
            FileType = fileType,
            FileSize = fileSize,
            CreateBy = userId,
            CreateDate = DateTime.UtcNow
        };

        await _context.ArticleAttachFiles.AddAsync(attachFile);
        await _context.SaveChangesAsync();

        return new ArticleAttachFileDto
        {
            Id = (int)attachFile.Id,
            ArticleId = attachFile.ArticleId ?? 0,
            Name = attachFile.AttachFileName,
            FileType = attachFile.FileType,
            FileSize = (int?)(attachFile.FileSize)
        };
    }

    public async Task<bool> DeleteAttachFileAsync(long attachFileId, string userId)
    {
        var attachFile = await _context.ArticleAttachFiles.FirstOrDefaultAsync(f => f.Id == attachFileId);
        if (attachFile == null) return false;

        _context.ArticleAttachFiles.Remove(attachFile);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Articles.CountAsync();
    }

    public async Task<int> GetPublishedCountAsync()
    {
        return await _context.Articles.CountAsync(a => a.Active == true);
    }

    public async Task<List<ArticleListDto>> GetTopArticlesAsync(int count = 10)
    {
        return await _context.Articles
            .AsNoTracking()
            .Where(a => a.Active == true)
            .OrderByDescending(a => a.Counter)
            .Take(count)
            .Select(a => new ArticleListDto
            {
                Id = (int)a.Id,
                Name = a.Name,
                Image = a.Image,
                Description = a.Description,
                Url = a.Url,
                Counter = a.Counter ?? 0,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
    }

    public async Task<List<ArticleListDto>> GetRecentArticlesAsync(int count = 10)
    {
        return await _context.Articles
            .AsNoTracking()
            .Where(a => a.Active == true)
            .OrderByDescending(a => a.CreateDate)
            .Take(count)
            .Select(a => new ArticleListDto
            {
                Id = (int)a.Id,
                Name = a.Name,
                Image = a.Image,
                Description = a.Description,
                Url = a.Url,
                Counter = a.Counter ?? 0,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
    }

    public async Task<List<ArticleListDto>> GetRelatedArticlesAsync(long articleId, int count = 5)
    {
        var article = await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null || string.IsNullOrEmpty(article.ArticleCategoryIds))
            return new List<ArticleListDto>();

        var firstCategoryId = article.ArticleCategoryIds.Split(',')[0];

        return await _context.Articles
            .AsNoTracking()
            .Where(a => a.Active == true && a.Id != articleId &&
                       a.ArticleCategoryIds != null &&
                       a.ArticleCategoryIds.Contains(firstCategoryId))
            .OrderByDescending(a => a.CreateDate)
            .Take(count)
            .Select(a => new ArticleListDto
            {
                Id = (int)a.Id,
                Name = a.Name,
                Image = a.Image,
                Description = a.Description,
                Url = a.Url,
                Counter = a.Counter ?? 0,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
    }

    public async Task IncrementCounterAsync(long id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article != null)
        {
            article.Counter = (article.Counter ?? 0) + 1;
            await _context.SaveChangesAsync();
        }
    }

    private ArticleDto MapToArticleDto(Article article)
    {
        return new ArticleDto
        {
            Id = (int)article.Id,
            Name = article.Name,
            SubTitle = article.SubTitle,
            Image = article.Image,
            ImageDescription = article.ImageDescription,
            BannerImage = article.BannerImage,
            Description = article.Description,
            Content = article.Content,
            Author = article.Author,
            ArticleTypeId = article.ArticleTypeId,
            ArticleStatusId = article.ArticleStatusId,
            Url = article.Url,
            Tags = article.Tags,
            Counter = article.Counter ?? 0,
            Active = article.Active ?? false,
            Approved = article.Approved,
            StartDate = article.StartDate,
            EndDate = article.EndDate,
            CreateBy = article.CreateBy,
            CreateDate = article.CreateDate,
            LastEditBy = article.LastEditBy,
            LastEditDate = article.LastEditDate,
            MetaTitle = article.MetaTitle,
            MetaDescription = article.MetaDescription,
            MetaKeywords = article.MetaKeywords
        };
    }

    private string GenerateUrl(string? name)
    {
        if (string.IsNullOrEmpty(name)) return Guid.NewGuid().ToString();
        return name.ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d")
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
