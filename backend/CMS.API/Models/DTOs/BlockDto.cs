using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

// ===================== Article Block DTOs =====================

public class ArticleBlockListDto
{
    public long Id { get; set; }
    public long? ArticleCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public bool? Active { get; set; }
    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public int ArticleCount { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class ArticleBlockDetailDto
{
    public long Id { get; set; }
    public long? ArticleCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public bool? Active { get; set; }
    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool? CanDelete { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? LastEditedDate { get; set; }
    public List<BlockArticleDto> Articles { get; set; } = new();
}

public class CreateArticleBlockRequest
{
    public long? ArticleCategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool Active { get; set; } = true;
}

public class UpdateArticleBlockRequest
{
    public long? ArticleCategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool Active { get; set; } = true;
}

// ===================== Product Block DTOs =====================

public class ProductBlockListDto
{
    public long Id { get; set; }
    public long? ProductCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public bool? Active { get; set; }
    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public int ProductCount { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class ProductBlockDetailDto
{
    public long Id { get; set; }
    public long? ProductCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public bool? Active { get; set; }
    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool? CanDelete { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? LastEditedDate { get; set; }
    public List<BlockProductDto> Products { get; set; } = new();
}

public class CreateProductBlockRequest
{
    public long? ProductCategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool Active { get; set; } = true;
}

public class UpdateProductBlockRequest
{
    public long? ProductCategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    public int? Sort { get; set; }
    public long? StyleId { get; set; }
    public bool Active { get; set; } = true;
}

// ===================== Block Content DTOs =====================

public class BlockArticleDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class BlockProductDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Sku { get; set; }
    public decimal? Price { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class AddItemsToBlockRequest
{
    [Required]
    public List<int> ItemIds { get; set; } = new();
}
