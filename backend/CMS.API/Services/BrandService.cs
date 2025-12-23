using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

/// <summary>
/// Brand management service implementation
/// </summary>
public class BrandService : IBrandService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BrandService> _logger;

    public BrandService(ApplicationDbContext context, ILogger<BrandService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<BrandListDto>> SearchAsync(BrandSearchRequest request)
    {
        var query = _context.ProductBrands.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(b =>
                (b.Name != null && b.Name.ToLower().Contains(keyword)) ||
                (b.Code != null && b.Code.ToLower().Contains(keyword)) ||
                (b.TradingName != null && b.TradingName.ToLower().Contains(keyword)) ||
                (b.Email != null && b.Email.ToLower().Contains(keyword)) ||
                (b.Telephone != null && b.Telephone.Contains(keyword)));
        }

        if (request.ProductBrandTypeId.HasValue)
        {
            query = query.Where(b => b.ProductBrandTypeId == request.ProductBrandTypeId.Value);
        }

        if (request.ProductBrandStatusId.HasValue)
        {
            query = query.Where(b => b.ProductBrandStatusId == request.ProductBrandStatusId.Value);
        }

        if (request.ProductBrandLevelId.HasValue)
        {
            query = query.Where(b => b.ProductBrandLevelId == request.ProductBrandLevelId.Value);
        }

        if (request.ProductBrandCategoryId.HasValue)
        {
            query = query.Where(b => b.ProductBrandCategoryId == request.ProductBrandCategoryId.Value);
        }

        if (request.LocationId.HasValue)
        {
            query = query.Where(b => b.LocationId == request.LocationId.Value);
        }

        if (request.Active.HasValue)
        {
            query = query.Where(b => b.Active == request.Active.Value);
        }

        if (request.Approved.HasValue)
        {
            query = query.Where(b => b.Approved == request.Approved.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(b => b.Name) : query.OrderBy(b => b.Name),
            "code" => request.SortDescending ? query.OrderByDescending(b => b.Code) : query.OrderBy(b => b.Code),
            "viewcount" => request.SortDescending ? query.OrderByDescending(b => b.ViewCount) : query.OrderBy(b => b.ViewCount),
            "sort" => request.SortDescending ? query.OrderByDescending(b => b.Sort) : query.OrderBy(b => b.Sort),
            _ => request.SortDescending ? query.OrderByDescending(b => b.CreateDate) : query.OrderBy(b => b.CreateDate)
        };

        // Apply pagination
        var brands = await query
            .Include(b => b.ProductBrandType)
            .Include(b => b.ProductBrandStatus)
            .Include(b => b.ProductBrandLevel)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        // Map to DTOs
        var items = brands.Select(b => new BrandListDto
        {
            Id = b.Id,
            Code = b.Code,
            Name = b.Name,
            TradingName = b.TradingName,
            Image = b.Image,
            Url = b.Url,
            ProductBrandTypeId = b.ProductBrandTypeId,
            ProductBrandTypeName = b.ProductBrandType?.Name,
            ProductBrandStatusId = b.ProductBrandStatusId,
            ProductBrandStatusName = b.ProductBrandStatus?.Name,
            ProductBrandLevelId = b.ProductBrandLevelId,
            ProductBrandLevelName = b.ProductBrandLevel?.Name,
            Telephone = b.Telephone,
            Email = b.Email,
            Active = b.Active,
            ViewCount = b.ViewCount,
            CreateDate = b.CreateDate
        }).ToList();

        return new PagedResult<BrandListDto>(items, totalCount, request.Page, request.PageSize);
    }

    public async Task<BrandDto?> GetByIdAsync(long id)
    {
        var brand = await _context.ProductBrands
            .Include(b => b.ProductBrandType)
            .Include(b => b.ProductBrandStatus)
            .Include(b => b.ProductBrandLevel)
            .FirstOrDefaultAsync(b => b.Id == id);

        return brand == null ? null : MapToBrandDto(brand);
    }

    public async Task<BrandDto?> GetByUrlAsync(string url)
    {
        var brand = await _context.ProductBrands
            .Include(b => b.ProductBrandType)
            .Include(b => b.ProductBrandStatus)
            .Include(b => b.ProductBrandLevel)
            .FirstOrDefaultAsync(b => b.Url == url);

        return brand == null ? null : MapToBrandDto(brand);
    }

    public async Task<BrandDto?> GetByCodeAsync(string code)
    {
        var brand = await _context.ProductBrands
            .Include(b => b.ProductBrandType)
            .Include(b => b.ProductBrandStatus)
            .Include(b => b.ProductBrandLevel)
            .FirstOrDefaultAsync(b => b.Code == code);

        return brand == null ? null : MapToBrandDto(brand);
    }

    public async Task<BrandDto> CreateAsync(CreateBrandRequest request, string createdBy)
    {
        var brand = new ProductBrand
        {
            ProductBrandCategoryId = request.ProductBrandCategoryId,
            ProductBrandTypeId = request.ProductBrandTypeId,
            DepartmentManId = request.DepartmentManId,
            ProductBrandLevelId = request.ProductBrandLevelId,
            ProductBrandStatusId = request.ProductBrandStatusId ?? 1, // Default: Pending
            CountryId = request.CountryId,
            LocationId = request.LocationId,
            DistrictId = request.DistrictId,
            WardId = request.WardId,
            BankId = request.BankId,
            Code = request.Code,
            Name = request.Name,
            TradingName = request.TradingName,
            BrandName = request.BrandName,
            TaxCode = request.TaxCode,
            RegistrationNumber = request.RegistrationNumber,
            IssuedDate = request.IssuedDate,
            BusinessArea = request.BusinessArea,
            Address = request.Address,
            Telephone = request.Telephone,
            Fax = request.Fax,
            Mobile = request.Mobile,
            Email = request.Email,
            Website = request.Website,
            Facebook = request.Facebook,
            Zalo = request.Zalo,
            Hotline = request.Hotline,
            Skype = request.Skype,
            BankAcc = request.BankAcc,
            PrInfo = request.PrInfo,
            Agency = request.Agency,
            Description = request.Description,
            LegalDocument = request.LegalDocument,
            Image = request.Image,
            PersonSurname = request.PersonSurname,
            PersonName = request.PersonName,
            PersonAddress = request.PersonAddress,
            PersonMobile = request.PersonMobile,
            PersonZalo = request.PersonZalo,
            PersonEmail = request.PersonEmail,
            PersonPosition = request.PersonPosition,
            DirectorName = request.DirectorName,
            DirectorBirthday = request.DirectorBirthday,
            DirectorAddress = request.DirectorAddress,
            DirectorMobile = request.DirectorMobile,
            DirectorEmail = request.DirectorEmail,
            DirectorPosition = request.DirectorPosition,
            Sort = request.Sort,
            Url = request.Url ?? GenerateUrl(request.Name),
            Active = request.Active ?? true,
            GoogleMapCode = request.GoogleMapCode,
            CreateBy = createdBy,
            CreateDate = DateTime.UtcNow,
            ViewCount = 0,
            Approved = 0 // Pending approval
        };

        _context.ProductBrands.Add(brand);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} created by {CreatedBy}", brand.Id, createdBy);

        return (await GetByIdAsync(brand.Id))!;
    }

    public async Task<BrandDto?> UpdateAsync(long id, UpdateBrandRequest request, string updatedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return null;

        // Update fields if provided
        if (request.ProductBrandCategoryId.HasValue) brand.ProductBrandCategoryId = request.ProductBrandCategoryId;
        if (request.ProductBrandTypeId.HasValue) brand.ProductBrandTypeId = request.ProductBrandTypeId;
        if (request.DepartmentManId.HasValue) brand.DepartmentManId = request.DepartmentManId;
        if (request.ProductBrandLevelId.HasValue) brand.ProductBrandLevelId = request.ProductBrandLevelId;
        if (request.ProductBrandStatusId.HasValue) brand.ProductBrandStatusId = request.ProductBrandStatusId;
        if (request.CountryId.HasValue) brand.CountryId = request.CountryId;
        if (request.LocationId.HasValue) brand.LocationId = request.LocationId;
        if (request.DistrictId.HasValue) brand.DistrictId = request.DistrictId;
        if (request.WardId.HasValue) brand.WardId = request.WardId;
        if (request.BankId.HasValue) brand.BankId = request.BankId;
        if (request.Code != null) brand.Code = request.Code;
        if (request.Name != null) brand.Name = request.Name;
        if (request.TradingName != null) brand.TradingName = request.TradingName;
        if (request.BrandName != null) brand.BrandName = request.BrandName;
        if (request.TaxCode != null) brand.TaxCode = request.TaxCode;
        if (request.RegistrationNumber != null) brand.RegistrationNumber = request.RegistrationNumber;
        if (request.IssuedDate.HasValue) brand.IssuedDate = request.IssuedDate;
        if (request.BusinessArea != null) brand.BusinessArea = request.BusinessArea;
        if (request.Address != null) brand.Address = request.Address;
        if (request.Telephone != null) brand.Telephone = request.Telephone;
        if (request.Fax != null) brand.Fax = request.Fax;
        if (request.Mobile != null) brand.Mobile = request.Mobile;
        if (request.Email != null) brand.Email = request.Email;
        if (request.Website != null) brand.Website = request.Website;
        if (request.Facebook != null) brand.Facebook = request.Facebook;
        if (request.Zalo != null) brand.Zalo = request.Zalo;
        if (request.Hotline != null) brand.Hotline = request.Hotline;
        if (request.Skype != null) brand.Skype = request.Skype;
        if (request.BankAcc != null) brand.BankAcc = request.BankAcc;
        if (request.PrInfo != null) brand.PrInfo = request.PrInfo;
        if (request.Agency != null) brand.Agency = request.Agency;
        if (request.Description != null) brand.Description = request.Description;
        if (request.LegalDocument != null) brand.LegalDocument = request.LegalDocument;
        if (request.Image != null) brand.Image = request.Image;
        if (request.PersonSurname != null) brand.PersonSurname = request.PersonSurname;
        if (request.PersonName != null) brand.PersonName = request.PersonName;
        if (request.PersonAddress != null) brand.PersonAddress = request.PersonAddress;
        if (request.PersonMobile != null) brand.PersonMobile = request.PersonMobile;
        if (request.PersonZalo != null) brand.PersonZalo = request.PersonZalo;
        if (request.PersonEmail != null) brand.PersonEmail = request.PersonEmail;
        if (request.PersonPosition != null) brand.PersonPosition = request.PersonPosition;
        if (request.DirectorName != null) brand.DirectorName = request.DirectorName;
        if (request.DirectorBirthday.HasValue) brand.DirectorBirthday = request.DirectorBirthday;
        if (request.DirectorAddress != null) brand.DirectorAddress = request.DirectorAddress;
        if (request.DirectorMobile != null) brand.DirectorMobile = request.DirectorMobile;
        if (request.DirectorEmail != null) brand.DirectorEmail = request.DirectorEmail;
        if (request.DirectorPosition != null) brand.DirectorPosition = request.DirectorPosition;
        if (request.Sort.HasValue) brand.Sort = request.Sort;
        if (request.Url != null) brand.Url = request.Url;
        if (request.Active.HasValue) brand.Active = request.Active;
        if (request.GoogleMapCode != null) brand.GoogleMapCode = request.GoogleMapCode;

        brand.LastEditBy = updatedBy;
        brand.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} updated by {UpdatedBy}", id, updatedBy);

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(long id, string deletedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return false;

        _context.ProductBrands.Remove(brand);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} deleted by {DeletedBy}", id, deletedBy);

        return true;
    }

    public async Task<bool> ApproveAsync(long id, string approvedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return false;

        brand.Approved = 1;
        brand.ApproveBy = approvedBy;
        brand.ApproveDate = DateTime.UtcNow;
        brand.ProductBrandStatusId = 2; // Approved status

        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} approved by {ApprovedBy}", id, approvedBy);

        return true;
    }

    public async Task<bool> RejectAsync(long id, string rejectedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return false;

        brand.Approved = -1;
        brand.ApproveBy = rejectedBy;
        brand.ApproveDate = DateTime.UtcNow;
        brand.ProductBrandStatusId = 3; // Rejected status

        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} rejected by {RejectedBy}", id, rejectedBy);

        return true;
    }

    public async Task<bool> UpdateStatusAsync(long id, int statusId, string updatedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return false;

        brand.ProductBrandStatusId = statusId;
        brand.LastEditBy = updatedBy;
        brand.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} status updated to {StatusId} by {UpdatedBy}", id, statusId, updatedBy);

        return true;
    }

    public async Task<bool> ToggleActiveAsync(long id, string updatedBy)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand == null) return false;

        brand.Active = !(brand.Active ?? true);
        brand.LastEditBy = updatedBy;
        brand.LastEditDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Brand {BrandId} active status toggled by {UpdatedBy}", id, updatedBy);

        return true;
    }

    public async Task IncrementViewCountAsync(long id)
    {
        var brand = await _context.ProductBrands.FindAsync(id);
        if (brand != null)
        {
            brand.ViewCount = (brand.ViewCount ?? 0) + 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<BrandTypeDto>> GetTypesAsync()
    {
        return await _context.ProductBrandTypes
            .Select(t => new BrandTypeDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
    }

    public async Task<List<BrandStatusDto>> GetStatusesAsync()
    {
        return await _context.ProductBrandStatuses
            .Select(s => new BrandStatusDto
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();
    }

    public async Task<List<BrandLevelDto>> GetLevelsAsync()
    {
        return await _context.ProductBrandLevels
            .Select(l => new BrandLevelDto
            {
                Id = l.Id,
                Name = l.Name
            })
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await _context.ProductBrands.AnyAsync(b => b.Id == id);
    }

    public async Task<bool> CodeExistsAsync(string code, long? excludeBrandId = null)
    {
        var query = _context.ProductBrands.Where(b => b.Code == code);
        if (excludeBrandId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBrandId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task<bool> UrlExistsAsync(string url, long? excludeBrandId = null)
    {
        var query = _context.ProductBrands.Where(b => b.Url == url);
        if (excludeBrandId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBrandId.Value);
        }
        return await query.AnyAsync();
    }

    private static BrandDto MapToBrandDto(ProductBrand brand)
    {
        return new BrandDto
        {
            Id = brand.Id,
            ProductBrandCategoryId = brand.ProductBrandCategoryId,
            ProductBrandTypeId = brand.ProductBrandTypeId,
            ProductBrandTypeName = brand.ProductBrandType?.Name,
            DepartmentManId = brand.DepartmentManId,
            ProductBrandModelManagementId = brand.ProductBrandModelManagementId,
            ProductBrandLevelId = brand.ProductBrandLevelId,
            ProductBrandLevelName = brand.ProductBrandLevel?.Name,
            ProductBrandStatusId = brand.ProductBrandStatusId,
            ProductBrandStatusName = brand.ProductBrandStatus?.Name,
            CountryId = brand.CountryId,
            LocationId = brand.LocationId,
            DistrictId = brand.DistrictId,
            WardId = brand.WardId,
            BankId = brand.BankId,
            Code = brand.Code,
            QrCodePublic = brand.QrCodePublic,
            Name = brand.Name,
            TradingName = brand.TradingName,
            BrandName = brand.BrandName,
            TaxCode = brand.TaxCode,
            RegistrationNumber = brand.RegistrationNumber,
            IssuedDate = brand.IssuedDate,
            BusinessArea = brand.BusinessArea,
            Address = brand.Address,
            Telephone = brand.Telephone,
            Fax = brand.Fax,
            Mobile = brand.Mobile,
            Email = brand.Email,
            Website = brand.Website,
            Facebook = brand.Facebook,
            Zalo = brand.Zalo,
            Hotline = brand.Hotline,
            Skype = brand.Skype,
            BankAcc = brand.BankAcc,
            PrInfo = brand.PrInfo,
            Agency = brand.Agency,
            Description = brand.Description,
            LegalDocument = brand.LegalDocument,
            Image = brand.Image,
            PersonSurname = brand.PersonSurname,
            PersonName = brand.PersonName,
            PersonAddress = brand.PersonAddress,
            PersonMobile = brand.PersonMobile,
            PersonZalo = brand.PersonZalo,
            PersonEmail = brand.PersonEmail,
            PersonPosition = brand.PersonPosition,
            Sort = brand.Sort,
            Url = brand.Url,
            Active = brand.Active,
            HasQrCode = brand.HasQrCode,
            ViewCount = brand.ViewCount,
            ViewPageCount = brand.ViewPageCount,
            FollowCount = brand.FollowCount,
            SellCount = brand.SellCount,
            AccountUsername = brand.AccountUsername,
            AccountEmail = brand.AccountEmail,
            DirectorName = brand.DirectorName,
            DirectorBirthday = brand.DirectorBirthday,
            DirectorAddress = brand.DirectorAddress,
            DirectorMobile = brand.DirectorMobile,
            DirectorEmail = brand.DirectorEmail,
            DirectorPosition = brand.DirectorPosition,
            CreateBy = brand.CreateBy,
            CreateDate = brand.CreateDate,
            LastEditBy = brand.LastEditBy,
            LastEditDate = brand.LastEditDate,
            Checked = brand.Checked,
            CheckBy = brand.CheckBy,
            CheckDate = brand.CheckDate,
            Approved = brand.Approved,
            ApproveBy = brand.ApproveBy,
            ApproveDate = brand.ApproveDate,
            GoogleMapCode = brand.GoogleMapCode
        };
    }

    private static string GenerateUrl(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;
        
        // Simple URL generation - remove diacritics and special chars
        var url = name.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("đ", "d")
            .Replace("Đ", "d");
        
        // Remove special characters
        url = System.Text.RegularExpressions.Regex.Replace(url, @"[^a-z0-9\-]", "");
        url = System.Text.RegularExpressions.Regex.Replace(url, @"-+", "-");
        url = url.Trim('-');
        
        return url;
    }
}
