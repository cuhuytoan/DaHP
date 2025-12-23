using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Article response DTO
/// </summary>
public class ArticleDto
{
    public long Id { get; set; }
    public long? ArticleTypeId { get; set; }
    public string? ArticleTypeName { get; set; }
    public long? ArticleStatusId { get; set; }
    public string? ArticleStatusName { get; set; }
    public long? ProductBrandId { get; set; }
    public string? ProductBrandName { get; set; }
    public string? Name { get; set; }
    public string? SubTitle { get; set; }
    public string? Image { get; set; }
    public string? ImageDescription { get; set; }
    public string? BannerImage { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? Active { get; set; }
    public int? Counter { get; set; }
    public string? Url { get; set; }
    public string? Tags { get; set; }
    public bool? CanCopy { get; set; }
    public bool? CanComment { get; set; }
    public bool? CanDelete { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditBy { get; set; }
    public DateTime? LastEditDate { get; set; }
    public int? Checked { get; set; }
    public int? Approved { get; set; }
    public List<ArticleCategoryDto> Categories { get; set; } = new();
    public List<ArticleAttachFileDto> AttachFiles { get; set; } = new();
}

/// <summary>
/// Article list item DTO (for search results)
/// </summary>
public class ArticleListDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? SubTitle { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Url { get; set; }
    public long? ArticleStatusId { get; set; }
    public string? ArticleStatusName { get; set; }
    public int? Counter { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? StartDate { get; set; }
}

/// <summary>
/// Create article request DTO
/// </summary>
public class CreateArticleRequest
{
    [Required]
    [StringLength(1000, ErrorMessage = "Article name cannot exceed 1000 characters")]
    public string Name { get; set; } = string.Empty;

    public long? ArticleTypeId { get; set; }

    public long? ProductBrandId { get; set; }

    [StringLength(200)]
    public string? SubTitle { get; set; }

    [StringLength(200)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? ImageDescription { get; set; }

    [StringLength(200)]
    public string? BannerImage { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Article content is required")]
    public string Content { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Author { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Active { get; set; } = true;

    [StringLength(1000)]
    public string? Url { get; set; }

    public string? Tags { get; set; }

    public bool CanCopy { get; set; } = true;

    public bool CanComment { get; set; } = true;

    [StringLength(500)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public List<int> CategoryIds { get; set; } = new();
}

/// <summary>
/// Update article request DTO
/// </summary>
public class UpdateArticleRequest
{
    [Required]
    [StringLength(1000, ErrorMessage = "Article name cannot exceed 1000 characters")]
    public string Name { get; set; } = string.Empty;

    public long? ArticleTypeId { get; set; }

    public long? ProductBrandId { get; set; }

    [StringLength(200)]
    public string? SubTitle { get; set; }

    [StringLength(200)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? ImageDescription { get; set; }

    [StringLength(200)]
    public string? BannerImage { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Article content is required")]
    public string Content { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Author { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Active { get; set; } = true;

    [StringLength(1000)]
    public string? Url { get; set; }

    public string? Tags { get; set; }

    public bool CanCopy { get; set; } = true;

    public bool CanComment { get; set; } = true;

    [StringLength(500)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public List<int> CategoryIds { get; set; } = new();
}

/// <summary>
/// Article search request DTO
/// </summary>
public class ArticleSearchRequest
{
    public string? Keyword { get; set; }
    public long? ArticleTypeId { get; set; }
    public long? ArticleCategoryId { get; set; }
    public long? ArticleStatusId { get; set; }
    public long? ProductBrandId { get; set; }
    public bool? Active { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Article category DTO
/// </summary>
public class ArticleCategoryDto
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public int? Sort { get; set; }
    public int? Counter { get; set; }
    public bool? DisplayMenu { get; set; }
    public bool? Active { get; set; }
    public bool? CanDelete { get; set; }
    public DateTime? CreateDate { get; set; }
    public List<ArticleCategoryDto> Children { get; set; } = new();
}

/// <summary>
/// Create article category request
/// </summary>
public class CreateArticleCategoryRequest
{
    [Required]
    [StringLength(500)]
    public string Name { get; set; } = string.Empty;

    public long? ParentId { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public string? Description { get; set; }

    public int? Sort { get; set; }

    public bool DisplayMenu { get; set; } = true;

    public bool Active { get; set; } = true;
}

/// <summary>
/// Update article category request
/// </summary>
public class UpdateArticleCategoryRequest
{
    [Required]
    [StringLength(500)]
    public string Name { get; set; } = string.Empty;

    public long? ParentId { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public string? Description { get; set; }

    public int? Sort { get; set; }

    public bool DisplayMenu { get; set; } = true;

    public bool Active { get; set; } = true;
}

/// <summary>
/// Article attach file DTO
/// </summary>
public class ArticleAttachFileDto
{
    public long Id { get; set; }
    public long? ArticleId { get; set; }
    public string? Name { get; set; }
    public string? FilePath { get; set; }
    public string? FileType { get; set; }
    public int? FileSize { get; set; }
    public int? Sort { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Article comment DTO
/// </summary>
public class ArticleCommentDto
{
    public long Id { get; set; }
    public long? ArticleId { get; set; }
    public long? ParentId { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Content { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public List<ArticleCommentDto> Replies { get; set; } = new();
}

/// <summary>
/// Create article comment request
/// </summary>
public class CreateArticleCommentRequest
{
    [Required]
    public long ArticleId { get; set; }

    public long? ParentId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}