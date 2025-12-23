using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.Requests;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CommentService> _logger;

    public CommentService(ApplicationDbContext context, ILogger<CommentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== Article Comments =====================

    public async Task<PagedResult<CommentResponse>> GetArticleCommentsAsync(long articleId, int page = 1, int pageSize = 20, bool? active = null)
    {
        var query = _context.ArticleComments.AsNoTracking().Where(c => c.ArticleId == articleId && c.ParentId == null);
        if (active.HasValue) query = query.Where(c => c.Active == active);

        var totalCount = await query.CountAsync();
        var comments = await query.OrderByDescending(c => c.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var items = new List<CommentResponse>();
        foreach (var comment in comments)
        {
            var replies = await _context.ArticleComments.AsNoTracking().Where(c => c.ParentId == comment.Id).ToListAsync();
            items.Add(new CommentResponse
            {
                Id = comment.Id, EntityId = comment.ArticleId ?? 0, ParentId = comment.ParentId,
                Name = comment.Name ?? "", Email = comment.Email, Content = comment.Content ?? "",
                Active = comment.Active ?? false, CreateDate = comment.CreateDate ?? DateTime.MinValue, CreateBy = comment.CreateBy,
                Replies = replies.Select(r => new CommentResponse
                {
                    Id = r.Id, EntityId = r.ArticleId ?? 0, ParentId = r.ParentId,
                    Name = r.Name ?? "", Email = r.Email, Content = r.Content ?? "",
                    Active = r.Active ?? false, CreateDate = r.CreateDate ?? DateTime.MinValue, CreateBy = r.CreateBy
                }).ToList()
            });
        }

        return new PagedResult<CommentResponse>
        {
            Items = items,
            Pagination = new PaginationInfo { CurrentPage = page, PageSize = pageSize, TotalCount = totalCount, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) }
        };
    }

    public async Task<CommentResponse?> GetArticleCommentByIdAsync(int id)
    {
        var comment = await _context.ArticleComments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return null;

        var replies = await _context.ArticleComments.AsNoTracking().Where(c => c.ParentId == comment.Id).ToListAsync();

        return new CommentResponse
        {
            Id = comment.Id, EntityId = comment.ArticleId ?? 0, ParentId = comment.ParentId,
            Name = comment.Name ?? "", Email = comment.Email, Content = comment.Content ?? "",
            Active = comment.Active ?? false, CreateDate = comment.CreateDate ?? DateTime.MinValue, CreateBy = comment.CreateBy,
            Replies = replies.Select(r => new CommentResponse
            {
                Id = r.Id, EntityId = r.ArticleId ?? 0, ParentId = r.ParentId,
                Name = r.Name ?? "", Email = r.Email, Content = r.Content ?? "",
                Active = r.Active ?? false, CreateDate = r.CreateDate ?? DateTime.MinValue, CreateBy = r.CreateBy
            }).ToList()
        };
    }

    public async Task<CommentResponse> CreateArticleCommentAsync(CreateCommentRequest request)
    {
        var comment = new ArticleComment
        {
            ArticleId = request.EntityId, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = DateTime.UtcNow
        };
        await _context.ArticleComments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new CommentResponse
        {
            Id = comment.Id, EntityId = request.EntityId, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = comment.CreateDate ?? DateTime.MinValue
        };
    }

    public async Task<CommentResponse> ReplyToArticleCommentAsync(int parentId, CreateCommentRequest request)
    {
        var parent = await _context.ArticleComments.FirstOrDefaultAsync(c => c.Id == parentId);
        if (parent == null) throw new InvalidOperationException("Parent comment not found");

        var reply = new ArticleComment
        {
            ArticleId = parent.ArticleId, ParentId = (int)parent.Id, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = DateTime.UtcNow
        };
        await _context.ArticleComments.AddAsync(reply);
        await _context.SaveChangesAsync();

        return new CommentResponse
        {
            Id = reply.Id, EntityId = parent.ArticleId ?? 0, ParentId = parentId, Name = request.Name,
            Email = request.Email, Content = request.Content, Active = false, CreateDate = reply.CreateDate ?? DateTime.MinValue
        };
    }

    public async Task<bool> ApproveArticleCommentAsync(int id, string approvedBy)
    {
        var comment = await _context.ArticleComments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return false;
        comment.Active = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteArticleCommentAsync(int id, string deletedBy)
    {
        var comment = await _context.ArticleComments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return false;

        var replies = await _context.ArticleComments.Where(c => c.ParentId == id).ToListAsync();
        _context.ArticleComments.RemoveRange(replies);
        _context.ArticleComments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Product Comments =====================

    public async Task<PagedResult<CommentResponse>> GetProductCommentsAsync(long productId, int page = 1, int pageSize = 20, bool? active = null)
    {
        var query = _context.ProductComments.AsNoTracking().Where(c => c.ProductId == productId && c.ParentId == null);
        if (active.HasValue) query = query.Where(c => c.Active == active);

        var totalCount = await query.CountAsync();
        var comments = await query.OrderByDescending(c => c.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var items = new List<CommentResponse>();
        foreach (var comment in comments)
        {
            var replies = await _context.ProductComments.AsNoTracking().Where(c => c.ParentId == comment.Id).ToListAsync();
            items.Add(new CommentResponse
            {
                Id = comment.Id, EntityId = comment.ProductId ?? 0, ParentId = comment.ParentId,
                Name = comment.Name ?? "", Email = comment.Email, Content = comment.Content ?? "",
                Active = comment.Active ?? false, CreateDate = comment.CreateDate ?? DateTime.MinValue, CreateBy = comment.CreateBy,
                Replies = replies.Select(r => new CommentResponse
                {
                    Id = r.Id, EntityId = r.ProductId ?? 0, ParentId = r.ParentId,
                    Name = r.Name ?? "", Email = r.Email, Content = r.Content ?? "",
                    Active = r.Active ?? false, CreateDate = r.CreateDate ?? DateTime.MinValue, CreateBy = r.CreateBy
                }).ToList()
            });
        }

        return new PagedResult<CommentResponse>
        {
            Items = items,
            Pagination = new PaginationInfo { CurrentPage = page, PageSize = pageSize, TotalCount = totalCount, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) }
        };
    }

    public async Task<CommentResponse?> GetProductCommentByIdAsync(int id)
    {
        var comment = await _context.ProductComments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return null;

        var replies = await _context.ProductComments.AsNoTracking().Where(c => c.ParentId == comment.Id).ToListAsync();

        return new CommentResponse
        {
            Id = comment.Id, EntityId = comment.ProductId ?? 0, ParentId = comment.ParentId,
            Name = comment.Name ?? "", Email = comment.Email, Content = comment.Content ?? "",
            Active = comment.Active ?? false, CreateDate = comment.CreateDate ?? DateTime.MinValue, CreateBy = comment.CreateBy,
            Replies = replies.Select(r => new CommentResponse
            {
                Id = r.Id, EntityId = r.ProductId ?? 0, ParentId = r.ParentId,
                Name = r.Name ?? "", Email = r.Email, Content = r.Content ?? "",
                Active = r.Active ?? false, CreateDate = r.CreateDate ?? DateTime.MinValue, CreateBy = r.CreateBy
            }).ToList()
        };
    }

    public async Task<CommentResponse> CreateProductCommentAsync(CreateCommentRequest request)
    {
        var comment = new ProductComment
        {
            ProductId = request.EntityId, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = DateTime.UtcNow
        };
        await _context.ProductComments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new CommentResponse
        {
            Id = comment.Id, EntityId = request.EntityId, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = comment.CreateDate ?? DateTime.MinValue
        };
    }

    public async Task<CommentResponse> ReplyToProductCommentAsync(int parentId, CreateCommentRequest request)
    {
        var parent = await _context.ProductComments.FirstOrDefaultAsync(c => c.Id == parentId);
        if (parent == null) throw new InvalidOperationException("Parent comment not found");

        var reply = new ProductComment
        {
            ProductId = parent.ProductId, ParentId = (int)parent.Id, Name = request.Name, Email = request.Email,
            Content = request.Content, Active = false, CreateDate = DateTime.UtcNow
        };
        await _context.ProductComments.AddAsync(reply);
        await _context.SaveChangesAsync();

        return new CommentResponse
        {
            Id = reply.Id, EntityId = parent.ProductId ?? 0, ParentId = parentId, Name = request.Name,
            Email = request.Email, Content = request.Content, Active = false, CreateDate = reply.CreateDate ?? DateTime.MinValue
        };
    }

    public async Task<bool> ApproveProductCommentAsync(int id, string approvedBy)
    {
        var comment = await _context.ProductComments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return false;
        comment.Active = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductCommentAsync(int id, string deletedBy)
    {
        var comment = await _context.ProductComments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return false;

        var replies = await _context.ProductComments.Where(c => c.ParentId == id).ToListAsync();
        _context.ProductComments.RemoveRange(replies);
        _context.ProductComments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}
