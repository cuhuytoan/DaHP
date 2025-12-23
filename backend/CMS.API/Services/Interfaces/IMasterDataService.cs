using CMS.API.Models.DTOs;

namespace CMS.API.Services.Interfaces;

public interface IMasterDataService
{
    // ===================== Units =====================
    Task<List<UnitDto>> GetUnitsAsync();
    Task<UnitDto?> GetUnitByIdAsync(long id);
    Task<UnitDto> CreateUnitAsync(CreateUnitRequest request);
    Task<UnitDto?> UpdateUnitAsync(long id, UpdateUnitRequest request);
    Task<bool> DeleteUnitAsync(long id);

    // ===================== Manufacturers =====================
    Task<List<ManufacturerDto>> GetManufacturersAsync();
    Task<ManufacturerDto?> GetManufacturerByIdAsync(long id);
    Task<ManufacturerDto> CreateManufacturerAsync(CreateManufacturerRequest request);
    Task<ManufacturerDto?> UpdateManufacturerAsync(long id, UpdateManufacturerRequest request);
    Task<bool> DeleteManufacturerAsync(long id);

    // ===================== Order Statuses =====================
    Task<List<OrderStatusDto>> GetOrderStatusesAsync();

    // ===================== Payment Methods =====================
    Task<List<PaymentMethodDto>> GetPaymentMethodsAsync(bool? activeOnly = null);
    Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(long id);
    Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodRequest request);
    Task<PaymentMethodDto?> UpdatePaymentMethodAsync(long id, UpdatePaymentMethodRequest request);
    Task<bool> DeletePaymentMethodAsync(long id);
    Task<bool> TogglePaymentMethodStatusAsync(long id);

    // ===================== Payment Statuses =====================
    Task<List<PaymentStatusDto>> GetPaymentStatusesAsync();

    // ===================== Product Statuses =====================
    Task<List<ProductStatusDto>> GetProductStatusesAsync();

    // ===================== Article Types =====================
    Task<List<ArticleTypeDto>> GetArticleTypesAsync();

    // ===================== Article Statuses =====================
    Task<List<ArticleStatusDto>> GetArticleStatusesAsync();

    // ===================== Banks =====================
    Task<List<BankDto>> GetBanksAsync(bool? activeOnly = null);

    // ===================== Product Properties =====================
    Task<List<PropertyCategoryDto>> GetPropertyCategoriesAsync(int? productCategoryId = null);
    Task<List<PropertyTypeDto>> GetPropertyTypesAsync();
    Task<List<PropertyDto>> GetPropertiesAsync(int? productCategoryId = null, int? propertyCategoryId = null);
}
