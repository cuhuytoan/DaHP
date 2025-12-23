using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

// ===================== Country DTOs =====================

public class CountryDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
}

// ===================== Unit DTOs =====================

public class UnitDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

public class CreateUnitRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}

public class UpdateUnitRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}

// ===================== Manufacturer DTOs =====================

public class ManufacturerDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int? Sort { get; set; }
}

public class CreateManufacturerRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public int? Sort { get; set; }
}

public class UpdateManufacturerRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public int? Sort { get; set; }
}

// ===================== Order Status DTOs =====================

public class OrderStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public int? Sort { get; set; }
}

// ===================== Payment Method DTOs =====================

public class PaymentMethodDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
}

public class CreatePaymentMethodRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(500)]
    public string? Image { get; set; }
    
    public int? Sort { get; set; }
    public bool Active { get; set; } = true;
}

public class UpdatePaymentMethodRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(500)]
    public string? Image { get; set; }
    
    public int? Sort { get; set; }
    public bool Active { get; set; } = true;
}

// ===================== Payment Status DTOs =====================

public class PaymentStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
}

// ===================== Product Status DTOs =====================

public class ProductStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

// ===================== Article Type/Status DTOs =====================

public class ArticleTypeDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

public class ArticleStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

// ===================== Bank DTOs =====================

public class BankDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? ShortName { get; set; }
    public string? Logo { get; set; }
    public bool? Active { get; set; }
    public int? Sort { get; set; }
}

// ===================== Property DTOs =====================

public class PropertyCategoryDto
{
    public long Id { get; set; }
    public long? ProductCategoryId { get; set; }
    public string? Name { get; set; }
    public int? Sort { get; set; }
}

public class PropertyTypeDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? TemplateDisplay { get; set; }
    public string? TemplateEdit { get; set; }
}

public class PropertyDto
{
    public long Id { get; set; }
    public long? ProductCategoryId { get; set; }
    public long? ProductPropertyCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public long? ProductPropertyTypeId { get; set; }
    public string? TypeName { get; set; }
    public string? Name { get; set; }
    public long? UnitId { get; set; }
    public string? UnitName { get; set; }
    public int? Sort { get; set; }
}
