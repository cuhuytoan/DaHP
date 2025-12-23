using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Brand list item DTO
/// </summary>
public class BrandListDto
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? TradingName { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public string? ProductBrandTypeName { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public string? ProductBrandStatusName { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public string? ProductBrandLevelName { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public bool? Active { get; set; }
    public int? ViewCount { get; set; }
    public int? ProductCount { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Brand detail DTO
/// </summary>
public class BrandDto
{
    public long Id { get; set; }
    public long? ProductBrandCategoryId { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public string? ProductBrandTypeName { get; set; }
    public long? DepartmentManId { get; set; }
    public long? ProductBrandModelManagementId { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public string? ProductBrandLevelName { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public string? ProductBrandStatusName { get; set; }
    public long? CountryId { get; set; }
    public long? LocationId { get; set; }
    public long? DistrictId { get; set; }
    public long? WardId { get; set; }
    public long? BankId { get; set; }
    public string? Code { get; set; }
    public string? QrCodePublic { get; set; }
    public string? Name { get; set; }
    public string? TradingName { get; set; }
    public string? BrandName { get; set; }
    public string? TaxCode { get; set; }
    public string? RegistrationNumber { get; set; }
    public DateTime? IssuedDate { get; set; }
    public string? BusinessArea { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Zalo { get; set; }
    public string? Hotline { get; set; }
    public string? Skype { get; set; }
    public string? BankAcc { get; set; }
    public string? PrInfo { get; set; }
    public string? Agency { get; set; }
    public string? Description { get; set; }
    public string? LegalDocument { get; set; }
    public string? Image { get; set; }
    public string? PersonSurname { get; set; }
    public string? PersonName { get; set; }
    public string? PersonAddress { get; set; }
    public string? PersonMobile { get; set; }
    public string? PersonZalo { get; set; }
    public string? PersonEmail { get; set; }
    public string? PersonPosition { get; set; }
    public int? Sort { get; set; }
    public string? Url { get; set; }
    public bool? Active { get; set; }
    public bool? HasQrCode { get; set; }
    public int? ViewCount { get; set; }
    public int? ViewPageCount { get; set; }
    public int? FollowCount { get; set; }
    public int? SellCount { get; set; }
    public string? AccountUsername { get; set; }
    public string? AccountEmail { get; set; }
    public string? DirectorName { get; set; }
    public DateTime? DirectorBirthday { get; set; }
    public string? DirectorAddress { get; set; }
    public string? DirectorMobile { get; set; }
    public string? DirectorEmail { get; set; }
    public string? DirectorPosition { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditBy { get; set; }
    public DateTime? LastEditDate { get; set; }
    public int? Checked { get; set; }
    public string? CheckBy { get; set; }
    public DateTime? CheckDate { get; set; }
    public int? Approved { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? GoogleMapCode { get; set; }
}

/// <summary>
/// Brand search request
/// </summary>
public class BrandSearchRequest
{
    public string? Keyword { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public long? ProductBrandCategoryId { get; set; }
    public long? LocationId { get; set; }
    public bool? Active { get; set; }
    public int? Approved { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "CreateDate";
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Create brand request
/// </summary>
public class CreateBrandRequest
{
    public long? ProductBrandCategoryId { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public long? DepartmentManId { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public long? CountryId { get; set; }
    public long? LocationId { get; set; }
    public long? DistrictId { get; set; }
    public long? WardId { get; set; }
    public long? BankId { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? TradingName { get; set; }

    [StringLength(200)]
    public string? BrandName { get; set; }

    [StringLength(50)]
    public string? TaxCode { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    public DateTime? IssuedDate { get; set; }

    public string? BusinessArea { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Telephone { get; set; }

    [StringLength(50)]
    public string? Fax { get; set; }

    [StringLength(50)]
    public string? Mobile { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    [StringLength(200)]
    public string? Facebook { get; set; }

    [StringLength(50)]
    public string? Zalo { get; set; }

    [StringLength(50)]
    public string? Hotline { get; set; }

    [StringLength(50)]
    public string? Skype { get; set; }

    [StringLength(100)]
    public string? BankAcc { get; set; }

    public string? PrInfo { get; set; }

    public string? Agency { get; set; }

    public string? Description { get; set; }

    public string? LegalDocument { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    // Contact person info
    [StringLength(100)]
    public string? PersonSurname { get; set; }

    [StringLength(100)]
    public string? PersonName { get; set; }

    [StringLength(500)]
    public string? PersonAddress { get; set; }

    [StringLength(50)]
    public string? PersonMobile { get; set; }

    [StringLength(50)]
    public string? PersonZalo { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? PersonEmail { get; set; }

    [StringLength(100)]
    public string? PersonPosition { get; set; }

    // Director info
    [StringLength(200)]
    public string? DirectorName { get; set; }

    public DateTime? DirectorBirthday { get; set; }

    [StringLength(500)]
    public string? DirectorAddress { get; set; }

    [StringLength(50)]
    public string? DirectorMobile { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? DirectorEmail { get; set; }

    [StringLength(100)]
    public string? DirectorPosition { get; set; }

    public int? Sort { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    public bool? Active { get; set; } = true;

    public string? GoogleMapCode { get; set; }
}

/// <summary>
/// Update brand request (same as create but all optional)
/// </summary>
public class UpdateBrandRequest
{
    public long? ProductBrandCategoryId { get; set; }
    public long? ProductBrandTypeId { get; set; }
    public long? DepartmentManId { get; set; }
    public long? ProductBrandLevelId { get; set; }
    public long? ProductBrandStatusId { get; set; }
    public long? CountryId { get; set; }
    public long? LocationId { get; set; }
    public long? DistrictId { get; set; }
    public long? WardId { get; set; }
    public long? BankId { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(500)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? TradingName { get; set; }

    [StringLength(200)]
    public string? BrandName { get; set; }

    [StringLength(50)]
    public string? TaxCode { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    public DateTime? IssuedDate { get; set; }

    public string? BusinessArea { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Telephone { get; set; }

    [StringLength(50)]
    public string? Fax { get; set; }

    [StringLength(50)]
    public string? Mobile { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    [StringLength(200)]
    public string? Facebook { get; set; }

    [StringLength(50)]
    public string? Zalo { get; set; }

    [StringLength(50)]
    public string? Hotline { get; set; }

    [StringLength(50)]
    public string? Skype { get; set; }

    [StringLength(100)]
    public string? BankAcc { get; set; }

    public string? PrInfo { get; set; }

    public string? Agency { get; set; }

    public string? Description { get; set; }

    public string? LegalDocument { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    [StringLength(100)]
    public string? PersonSurname { get; set; }

    [StringLength(100)]
    public string? PersonName { get; set; }

    [StringLength(500)]
    public string? PersonAddress { get; set; }

    [StringLength(50)]
    public string? PersonMobile { get; set; }

    [StringLength(50)]
    public string? PersonZalo { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? PersonEmail { get; set; }

    [StringLength(100)]
    public string? PersonPosition { get; set; }

    [StringLength(200)]
    public string? DirectorName { get; set; }

    public DateTime? DirectorBirthday { get; set; }

    [StringLength(500)]
    public string? DirectorAddress { get; set; }

    [StringLength(50)]
    public string? DirectorMobile { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? DirectorEmail { get; set; }

    [StringLength(100)]
    public string? DirectorPosition { get; set; }

    public int? Sort { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    public bool? Active { get; set; }

    public string? GoogleMapCode { get; set; }
}

/// <summary>
/// Brand type DTO
/// </summary>
public class BrandTypeDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

/// <summary>
/// Brand status DTO
/// </summary>
public class BrandStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

/// <summary>
/// Brand level DTO
/// </summary>
public class BrandLevelDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}
