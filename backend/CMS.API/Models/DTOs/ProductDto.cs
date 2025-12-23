using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Product response DTO
/// </summary>
public class ProductDto
{
    public long Id { get; set; }
    public long? ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }
    public long? ProductBrandId { get; set; }
    public string? ProductBrandName { get; set; }
    public long? ProductManufactureId { get; set; }
    public string? ProductManufactureName { get; set; }
    public long? ProductStatusId { get; set; }
    public string? ProductStatusName { get; set; }
    public long? UnitId { get; set; }
    public string? UnitName { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? SubTitle { get; set; }
    public string? Image { get; set; }
    public string? ImageDescription { get; set; }
    public string? BannerImage { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public decimal? Price { get; set; }
    public decimal? PriceOld { get; set; }
    public int? Quantity { get; set; }
    public int? QuantitySold { get; set; }
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
    public bool? AllowOrder { get; set; }
    public bool? AllowReview { get; set; }
    public int? TotalReview { get; set; }
    public int? TotalRating { get; set; }
    public decimal? AverageRating { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditBy { get; set; }
    public DateTime? LastEditDate { get; set; }
    public int? Checked { get; set; }
    public int? Approved { get; set; }
    public List<ProductCategoryDto> Categories { get; set; } = new();
    public List<ProductPictureDto> Pictures { get; set; } = new();
    public List<ProductAttachFileDto> AttachFiles { get; set; } = new();
    public List<ProductPropertyValueDto> Properties { get; set; } = new();
}

/// <summary>
/// Product list item DTO (for search results)
/// </summary>
public class ProductListDto
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? SubTitle { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public decimal? PriceOld { get; set; }
    public int? Quantity { get; set; }
    public int? QuantitySold { get; set; }
    public string? Url { get; set; }
    public long? ProductStatusId { get; set; }
    public string? ProductStatusName { get; set; }
    public string? ProductBrandName { get; set; }
    public int? Counter { get; set; }
    public bool? Active { get; set; }
    public decimal? AverageRating { get; set; }
    public int? TotalReview { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Create product request DTO
/// </summary>
public class CreateProductRequest
{
    [Required]
    [StringLength(1000, ErrorMessage = "Product name cannot exceed 1000 characters")]
    public string Name { get; set; } = string.Empty;

    public long? ProductTypeId { get; set; }

    public long? ProductBrandId { get; set; }

    public long? ProductManufactureId { get; set; }

    public long? UnitId { get; set; }

    [StringLength(200)]
    public string? Code { get; set; }

    [StringLength(200)]
    public string? SubTitle { get; set; }

    [StringLength(200)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? ImageDescription { get; set; }

    [StringLength(200)]
    public string? BannerImage { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal? Price { get; set; }

    public decimal? PriceOld { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public int? Quantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Active { get; set; } = true;

    [StringLength(1000)]
    public string? Url { get; set; }

    public string? Tags { get; set; }

    public bool CanCopy { get; set; } = true;

    public bool CanComment { get; set; } = true;

    public bool AllowOrder { get; set; } = true;

    public bool AllowReview { get; set; } = true;

    [StringLength(500)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    public List<ProductPropertyValueRequest> Properties { get; set; } = new();
}

/// <summary>
/// Update product request DTO
/// </summary>
public class UpdateProductRequest
{
    [Required]
    [StringLength(1000, ErrorMessage = "Product name cannot exceed 1000 characters")]
    public string Name { get; set; } = string.Empty;

    public long? ProductTypeId { get; set; }

    public long? ProductBrandId { get; set; }

    public long? ProductManufactureId { get; set; }

    public long? UnitId { get; set; }

    [StringLength(200)]
    public string? Code { get; set; }

    [StringLength(200)]
    public string? SubTitle { get; set; }

    [StringLength(200)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? ImageDescription { get; set; }

    [StringLength(200)]
    public string? BannerImage { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal? Price { get; set; }

    public decimal? PriceOld { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public int? Quantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Active { get; set; } = true;

    [StringLength(1000)]
    public string? Url { get; set; }

    public string? Tags { get; set; }

    public bool CanCopy { get; set; } = true;

    public bool CanComment { get; set; } = true;

    public bool AllowOrder { get; set; } = true;

    public bool AllowReview { get; set; } = true;

    [StringLength(500)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    public List<ProductPropertyValueRequest> Properties { get; set; } = new();
}

/// <summary>
/// Product search request DTO
/// </summary>
public class ProductSearchRequest
{
    public string? Keyword { get; set; }
    public long? ProductTypeId { get; set; }
    public long? ProductCategoryId { get; set; }
    public long? ProductStatusId { get; set; }
    public long? ProductBrandId { get; set; }
    public long? ProductManufactureId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? Active { get; set; }
    public bool? InStock { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Product category DTO
/// </summary>
public class ProductCategoryDto
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
    public string? MenuColor { get; set; }
    public bool? Active { get; set; }
    public bool? CanDelete { get; set; }
    public DateTime? CreateDate { get; set; }
    public List<ProductCategoryDto> Children { get; set; } = new();
}

/// <summary>
/// Create product category request
/// </summary>
public class CreateProductCategoryRequest
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

    [StringLength(50)]
    public string? MenuColor { get; set; }

    public bool Active { get; set; } = true;
}

/// <summary>
/// Update product category request
/// </summary>
public class UpdateProductCategoryRequest
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

    [StringLength(50)]
    public string? MenuColor { get; set; }

    public bool Active { get; set; } = true;
}

/// <summary>
/// Product picture DTO
/// </summary>
public class ProductPictureDto
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public int? Sort { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Product attach file DTO
/// </summary>
public class ProductAttachFileDto
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public string? Name { get; set; }
    public string? FilePath { get; set; }
    public string? FileType { get; set; }
    public int? FileSize { get; set; }
    public int? Sort { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Product property value DTO
/// </summary>
public class ProductPropertyValueDto
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public long? ProductPropertyId { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyCategoryName { get; set; }
    public string? Value { get; set; }
}

/// <summary>
/// Product property value request
/// </summary>
public class ProductPropertyValueRequest
{
    public long ProductPropertyId { get; set; }
    public string? Value { get; set; }
}

/// <summary>
/// Product review DTO
/// </summary>
public class ProductReviewDto
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Create product review request
/// </summary>
public class CreateProductReviewRequest
{
    [Required]
    public long ProductId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
}

/// <summary>
/// Product comment DTO
/// </summary>
public class ProductCommentDto
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public long? ParentId { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Content { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public List<ProductCommentDto> Replies { get; set; } = new();
}

/// <summary>
/// Create product comment request
/// </summary>
public class CreateProductCommentRequest
{
    [Required]
    public long ProductId { get; set; }

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

/// <summary>
/// Product brand DTO
/// </summary>
public class ProductBrandDto
{
    public long Id { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public string? ProductBrandTypeName { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public string? ProductBrandStatusName { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public string? ProductBrandLevelName { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? Image { get; set; }
    public string? BannerImage { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Youtube { get; set; }
    public string? Zalo { get; set; }
    public string? TaxCode { get; set; }
    public string? BankAccount { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public bool? Active { get; set; }
    public int? Counter { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}