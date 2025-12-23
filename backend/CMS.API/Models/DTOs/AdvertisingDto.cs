using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Advertising DTO for public display
/// </summary>
public class AdvertisingDto
{
    public long Id { get; set; }
    public long? AdvertisingBlockId { get; set; }
    public long? AdvertisingTypeId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public int? Sort { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
}

/// <summary>
/// Advertising block DTO
/// </summary>
public class AdvertisingBlockDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Sort { get; set; }
    public List<AdvertisingDto> Advertisings { get; set; } = new();
}

/// <summary>
/// Advertising type DTO
/// </summary>
public class AdvertisingTypeDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

// ===================== Admin Advertising Block DTOs =====================

public class AdvertisingBlockListDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
    public int AdvertisingCount { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class AdvertisingBlockDetailDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
    public bool? CanDelete { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? LastEditedDate { get; set; }
    public List<AdvertisingListDto> Advertisings { get; set; } = new();
}

public class CreateAdvertisingBlockRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Sort { get; set; }
    public bool Active { get; set; } = true;
}

public class UpdateAdvertisingBlockRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Sort { get; set; }
    public bool Active { get; set; } = true;
}

// ===================== Admin Advertising DTOs =====================

public class AdvertisingListDto
{
    public long Id { get; set; }
    public long? AdvertisingBlockId { get; set; }
    public string? BlockName { get; set; }
    public long? AdvertisingTypeId { get; set; }
    public string? TypeName { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public int? Sort { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? Active { get; set; }
    public int? Counter { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class AdvertisingDetailDto
{
    public long Id { get; set; }
    public long? AdvertisingBlockId { get; set; }
    public string? BlockName { get; set; }
    public long? AdvertisingTypeId { get; set; }
    public string? TypeName { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public int? Sort { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? Active { get; set; }
    public int? Counter { get; set; }
    public bool? CanDelete { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTime? LastEditedDate { get; set; }
}

public class CreateAdvertisingRequest
{
    [Required]
    public long AdvertisingBlockId { get; set; }

    public long? AdvertisingTypeId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Image { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public string? Content { get; set; }

    public int? Sort { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Active { get; set; } = true;
}

public class UpdateAdvertisingRequest
{
    [Required]
    public long AdvertisingBlockId { get; set; }

    public long? AdvertisingTypeId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Image { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public string? Content { get; set; }

    public int? Sort { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Active { get; set; } = true;
}
