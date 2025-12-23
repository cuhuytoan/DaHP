using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class MasterDataService : IMasterDataService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MasterDataService> _logger;

    public MasterDataService(ApplicationDbContext context, ILogger<MasterDataService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Units

    public async Task<List<UnitDto>> GetUnitsAsync()
    {
        return await _context.Units
            .Select(u => new UnitDto
            {
                Id = u.Id,
                Name = u.Name
            })
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public async Task<UnitDto?> GetUnitByIdAsync(long id)
    {
        var unit = await _context.Units.FindAsync(id);
        if (unit == null) return null;

        return new UnitDto
        {
            Id = unit.Id,
            Name = unit.Name
        };
    }

    public async Task<UnitDto> CreateUnitAsync(CreateUnitRequest request)
    {
        var unit = new Unit
        {
            Name = request.Name
        };

        _context.Units.Add(unit);
        await _context.SaveChangesAsync();

        return new UnitDto
        {
            Id = unit.Id,
            Name = unit.Name
        };
    }

    public async Task<UnitDto?> UpdateUnitAsync(long id, UpdateUnitRequest request)
    {
        var unit = await _context.Units.FindAsync(id);
        if (unit == null) return null;

        unit.Name = request.Name;
        await _context.SaveChangesAsync();

        return new UnitDto
        {
            Id = unit.Id,
            Name = unit.Name
        };
    }

    public async Task<bool> DeleteUnitAsync(long id)
    {
        var unit = await _context.Units.FindAsync(id);
        if (unit == null) return false;

        // Check if unit is in use
        var inUse = await _context.Products.AnyAsync(p => p.UnitId == id);
        if (inUse) return false;

        _context.Units.Remove(unit);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Manufacturers

    public async Task<List<ManufacturerDto>> GetManufacturersAsync()
    {
        return await _context.ProductManufactures
            .Select(m => new ManufacturerDto
            {
                Id = m.Id,
                Name = m.Name,
                Sort = m.Sort
            })
            .OrderBy(m => m.Sort)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<ManufacturerDto?> GetManufacturerByIdAsync(long id)
    {
        var manufacturer = await _context.ProductManufactures.FindAsync(id);
        if (manufacturer == null) return null;

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Sort = manufacturer.Sort
        };
    }

    public async Task<ManufacturerDto> CreateManufacturerAsync(CreateManufacturerRequest request)
    {
        var manufacturer = new ProductManufacture
        {
            Name = request.Name,
            Sort = request.Sort ?? await GetNextManufacturerSortAsync()
        };

        _context.ProductManufactures.Add(manufacturer);
        await _context.SaveChangesAsync();

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Sort = manufacturer.Sort
        };
    }

    public async Task<ManufacturerDto?> UpdateManufacturerAsync(long id, UpdateManufacturerRequest request)
    {
        var manufacturer = await _context.ProductManufactures.FindAsync(id);
        if (manufacturer == null) return null;

        manufacturer.Name = request.Name;
        manufacturer.Sort = request.Sort;
        await _context.SaveChangesAsync();

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Sort = manufacturer.Sort
        };
    }

    public async Task<bool> DeleteManufacturerAsync(long id)
    {
        var manufacturer = await _context.ProductManufactures.FindAsync(id);
        if (manufacturer == null) return false;

        // Check if manufacturer is in use
        var inUse = await _context.Products.AnyAsync(p => p.ProductManufactureId == id);
        if (inUse) return false;

        _context.ProductManufactures.Remove(manufacturer);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<int> GetNextManufacturerSortAsync()
    {
        var maxSort = await _context.ProductManufactures.MaxAsync(m => (int?)m.Sort) ?? 0;
        return maxSort + 1;
    }

    #endregion

    #region Order Statuses

    public async Task<List<OrderStatusDto>> GetOrderStatusesAsync()
    {
        return await _context.ProductOrderStatuses
            .Select(s => new OrderStatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
                Sort = s.Sort
            })
            .OrderBy(s => s.Sort)
            .ToListAsync();
    }

    #endregion

    #region Payment Methods

    public async Task<List<PaymentMethodDto>> GetPaymentMethodsAsync(bool? activeOnly = null)
    {
        var query = _context.ProductOrderPaymentMethods.AsQueryable();

        if (activeOnly == true)
            query = query.Where(p => p.Active == true);

        return await query
            .Select(p => new PaymentMethodDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Sort = p.Sort,
                Active = p.Active
            })
            .OrderBy(p => p.Sort)
            .ToListAsync();
    }

    public async Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(long id)
    {
        var method = await _context.ProductOrderPaymentMethods.FindAsync(id);
        if (method == null) return null;

        return new PaymentMethodDto
        {
            Id = method.Id,
            Name = method.Name,
            Description = method.Description,
            Image = method.Image,
            Sort = method.Sort,
            Active = method.Active
        };
    }

    public async Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodRequest request)
    {
        var method = new ProductOrderPaymentMethod
        {
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Sort = request.Sort ?? await GetNextPaymentMethodSortAsync(),
            Active = request.Active
        };

        _context.ProductOrderPaymentMethods.Add(method);
        await _context.SaveChangesAsync();

        return new PaymentMethodDto
        {
            Id = method.Id,
            Name = method.Name,
            Description = method.Description,
            Image = method.Image,
            Sort = method.Sort,
            Active = method.Active
        };
    }

    public async Task<PaymentMethodDto?> UpdatePaymentMethodAsync(long id, UpdatePaymentMethodRequest request)
    {
        var method = await _context.ProductOrderPaymentMethods.FindAsync(id);
        if (method == null) return null;

        method.Name = request.Name;
        method.Description = request.Description;
        method.Image = request.Image;
        method.Sort = request.Sort;
        method.Active = request.Active;

        await _context.SaveChangesAsync();

        return new PaymentMethodDto
        {
            Id = method.Id,
            Name = method.Name,
            Description = method.Description,
            Image = method.Image,
            Sort = method.Sort,
            Active = method.Active
        };
    }

    public async Task<bool> DeletePaymentMethodAsync(long id)
    {
        var method = await _context.ProductOrderPaymentMethods.FindAsync(id);
        if (method == null) return false;

        // Check if payment method is in use
        var inUse = await _context.ProductOrders.AnyAsync(o => o.ProductOrderPaymentMethodId == id);
        if (inUse) return false;

        _context.ProductOrderPaymentMethods.Remove(method);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TogglePaymentMethodStatusAsync(long id)
    {
        var method = await _context.ProductOrderPaymentMethods.FindAsync(id);
        if (method == null) return false;

        method.Active = !(method.Active ?? false);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<int> GetNextPaymentMethodSortAsync()
    {
        var maxSort = await _context.ProductOrderPaymentMethods.MaxAsync(p => (int?)p.Sort) ?? 0;
        return maxSort + 1;
    }

    #endregion

    #region Payment Statuses

    public async Task<List<PaymentStatusDto>> GetPaymentStatusesAsync()
    {
        return await _context.ProductOrderPaymentStatuses
            .Select(s => new PaymentStatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color
            })
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    #endregion

    #region Product Statuses

    public async Task<List<ProductStatusDto>> GetProductStatusesAsync()
    {
        return await _context.ProductStatuses
            .Select(s => new ProductStatusDto
            {
                Id = s.Id,
                Name = s.Name
            })
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    #endregion

    #region Article Types

    public async Task<List<ArticleTypeDto>> GetArticleTypesAsync()
    {
        return await _context.ArticleTypes
            .Select(t => new ArticleTypeDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    #endregion

    #region Article Statuses

    public async Task<List<ArticleStatusDto>> GetArticleStatusesAsync()
    {
        return await _context.ArticleStatuses
            .Select(s => new ArticleStatusDto
            {
                Id = s.Id,
                Name = s.Name
            })
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    #endregion

    #region Banks

    public async Task<List<BankDto>> GetBanksAsync(bool? activeOnly = null)
    {
        var query = _context.Banks.AsQueryable();

        if (activeOnly == true)
            query = query.Where(b => b.Active == true);

        return await query
            .Select(b => new BankDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                ShortName = b.ShortName,
                Logo = b.Logo,
                Active = b.Active,
                Sort = b.Sort
            })
            .OrderBy(b => b.Sort)
            .ThenBy(b => b.Name)
            .ToListAsync();
    }

    #endregion

    #region Product Properties

    public async Task<List<PropertyCategoryDto>> GetPropertyCategoriesAsync(int? productCategoryId = null)
    {
        var query = _context.ProductPropertyCategories.AsQueryable();

        if (productCategoryId.HasValue)
            query = query.Where(c => c.ProductCategoryId == productCategoryId.Value);

        return await query
            .Select(c => new PropertyCategoryDto
            {
                Id = c.Id,
                ProductCategoryId = c.ProductCategoryId,
                Name = c.Name,
                Sort = c.Sort
            })
            .OrderBy(c => c.Sort)
            .ToListAsync();
    }

    public async Task<List<PropertyTypeDto>> GetPropertyTypesAsync()
    {
        return await _context.ProductPropertyTypes
            .Select(t => new PropertyTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                TemplateDisplay = t.TemplateDisplay,
                TemplateEdit = t.TemplateEdit
            })
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    public async Task<List<PropertyDto>> GetPropertiesAsync(int? productCategoryId = null, int? propertyCategoryId = null)
    {
        var query = _context.ProductProperties.AsQueryable();

        if (productCategoryId.HasValue)
            query = query.Where(p => p.ProductCategoryId == productCategoryId.Value);

        if (propertyCategoryId.HasValue)
            query = query.Where(p => p.ProductPropertyCategoryId == propertyCategoryId.Value);

        var properties = await query
            .Include(p => p.ProductPropertyCategory)
            .Include(p => p.ProductPropertyType)
            .Select(p => new PropertyDto
            {
                Id = p.Id,
                ProductCategoryId = p.ProductCategoryId,
                ProductPropertyCategoryId = p.ProductPropertyCategoryId,
                CategoryName = p.ProductPropertyCategory != null ? p.ProductPropertyCategory.Name : null,
                ProductPropertyTypeId = p.ProductPropertyTypeId,
                TypeName = p.ProductPropertyType != null ? p.ProductPropertyType.Name : null,
                Name = p.Name,
                UnitId = p.UnitId,
                Sort = p.Sort
            })
            .OrderBy(p => p.Sort)
            .ToListAsync();

        // Get unit names
        var unitIds = properties.Where(p => p.UnitId.HasValue).Select(p => p.UnitId!.Value).Distinct().ToList();
        var units = await _context.Units
            .Where(u => unitIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Name);

        foreach (var property in properties)
        {
            if (property.UnitId.HasValue && units.ContainsKey(property.UnitId.Value))
                property.UnitName = units[property.UnitId.Value];
        }

        return properties;
    }

    #endregion
}
