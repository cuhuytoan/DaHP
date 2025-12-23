using CMS.API.Data;
using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Controllers;

/// <summary>
/// ProductBrands API controller for managing product brands
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Editor")]
public class ProductBrandsController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductBrandsController> _logger;

    public ProductBrandsController(ApplicationDbContext context, ILogger<ProductBrandsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all product brands with optional search and pagination
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] bool? active = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Set<ProductBrand>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b => b.Name != null && b.Name.Contains(search));
        }

        if (active.HasValue)
        {
            query = query.Where(b => b.Active == active.Value);
        }

        var totalCount = await query.CountAsync();
        var brands = await query
            .OrderByDescending(b => b.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new
            {
                b.Id,
                b.Code,
                b.Name,
                b.TradingName,
                b.BrandName,
                b.TaxCode,
                b.Address,
                b.Telephone,
                b.Mobile,
                b.Email,
                b.Website,
                b.Image,
                b.Description,
                b.Active,
                b.ViewCount,
                b.CreateDate,
                b.CreateBy
            })
            .ToListAsync();

        return ApiSuccess(new
        {
            items = brands,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    /// <summary>
    /// Get product brand by ID
    /// </summary>
    [HttpGet("{id:long}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var brand = await _context.Set<ProductBrand>()
            .Where(b => b.Id == id)
            .Select(b => new
            {
                b.Id,
                b.Code,
                b.Name,
                b.TradingName,
                b.BrandName,
                b.TaxCode,
                b.RegistrationNumber,
                b.IssuedDate,
                b.BusinessArea,
                b.Address,
                b.Telephone,
                b.Fax,
                b.Mobile,
                b.Email,
                b.Website,
                b.Facebook,
                b.Zalo,
                b.Hotline,
                b.Skype,
                b.BankAcc,
                b.PrInfo,
                b.Agency,
                b.Description,
                b.LegalDocument,
                b.Image,
                b.PersonSurname,
                b.PersonName,
                b.PersonAddress,
                b.PersonMobile,
                b.PersonZalo,
                b.PersonEmail,
                b.PersonPosition,
                b.DirectorName,
                b.DirectorBirthday,
                b.DirectorAddress,
                b.DirectorMobile,
                b.DirectorEmail,
                b.DirectorPosition,
                b.GoogleMapCode,
                b.Url,
                b.Sort,
                b.Active,
                b.ViewCount,
                b.ViewPageCount,
                b.FollowCount,
                b.SellCount,
                b.CreateDate,
                b.CreateBy,
                b.LastEditDate,
                b.LastEditBy,
                b.Approved,
                b.ApproveBy,
                b.ApproveDate
            })
            .FirstOrDefaultAsync();

        if (brand == null)
            return ApiNotFound("Product brand not found");

        return ApiSuccess(brand);
    }

    /// <summary>
    /// Create a new product brand
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductBrandRequest request)
    {
        if (!ModelState.IsValid)
            return ApiError("Invalid request", GetModelErrors());

        var brand = new ProductBrand
        {
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
            Description = request.Description,
            Image = request.Image,
            Url = request.Url,
            Sort = request.Sort ?? 0,
            Active = request.Active ?? true,
            CreateDate = DateTime.UtcNow,
            CreateBy = User.Identity?.Name
        };

        _context.Set<ProductBrand>().Add(brand);
        await _context.SaveChangesAsync();

        return ApiCreated(new { brand.Id, brand.Name }, message: "Product brand created successfully");
    }

    /// <summary>
    /// Update a product brand
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateProductBrandRequest request)
    {
        if (!ModelState.IsValid)
            return ApiError("Invalid request", GetModelErrors());

        var brand = await _context.Set<ProductBrand>().FindAsync(id);
        if (brand == null)
            return ApiNotFound("Product brand not found");

        brand.Code = request.Code ?? brand.Code;
        brand.Name = request.Name ?? brand.Name;
        brand.TradingName = request.TradingName ?? brand.TradingName;
        brand.BrandName = request.BrandName ?? brand.BrandName;
        brand.TaxCode = request.TaxCode ?? brand.TaxCode;
        brand.RegistrationNumber = request.RegistrationNumber ?? brand.RegistrationNumber;
        brand.IssuedDate = request.IssuedDate ?? brand.IssuedDate;
        brand.BusinessArea = request.BusinessArea ?? brand.BusinessArea;
        brand.Address = request.Address ?? brand.Address;
        brand.Telephone = request.Telephone ?? brand.Telephone;
        brand.Fax = request.Fax ?? brand.Fax;
        brand.Mobile = request.Mobile ?? brand.Mobile;
        brand.Email = request.Email ?? brand.Email;
        brand.Website = request.Website ?? brand.Website;
        brand.Facebook = request.Facebook ?? brand.Facebook;
        brand.Zalo = request.Zalo ?? brand.Zalo;
        brand.Hotline = request.Hotline ?? brand.Hotline;
        brand.Skype = request.Skype ?? brand.Skype;
        brand.BankAcc = request.BankAcc ?? brand.BankAcc;
        brand.Description = request.Description ?? brand.Description;
        brand.Image = request.Image ?? brand.Image;
        brand.Url = request.Url ?? brand.Url;
        if (request.Sort.HasValue) brand.Sort = request.Sort.Value;
        if (request.Active.HasValue) brand.Active = request.Active.Value;
        brand.LastEditDate = DateTime.UtcNow;
        brand.LastEditBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        return ApiSuccess(new { brand.Id, brand.Name }, "Product brand updated successfully");
    }

    /// <summary>
    /// Delete a product brand
    /// </summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var brand = await _context.Set<ProductBrand>().FindAsync(id);
        if (brand == null)
            return ApiNotFound("Product brand not found");

        // Check if brand has products
        var hasProducts = await _context.Set<Product>().AnyAsync(p => p.ProductBrandId == id);
        if (hasProducts)
            return ApiError("Cannot delete brand with existing products");

        _context.Set<ProductBrand>().Remove(brand);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Toggle product brand active status
    /// </summary>
    [HttpPost("{id:long}/toggle-status")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        var brand = await _context.Set<ProductBrand>().FindAsync(id);
        if (brand == null)
            return ApiNotFound("Product brand not found");

        brand.Active = !(brand.Active ?? false);
        brand.LastEditDate = DateTime.UtcNow;
        brand.LastEditBy = User.Identity?.Name;
        
        await _context.SaveChangesAsync();

        return ApiSuccess(brand.Active, "Product brand status toggled successfully");
    }
}

public class CreateProductBrandRequest
{
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
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
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
}

public class UpdateProductBrandRequest
{
    public string? Code { get; set; }
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
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
}
