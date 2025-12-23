using CMS.API.Models.DTOs;

namespace CMS.API.Services.Interfaces;

public interface IDashboardService
{
    /// <summary>
    /// Get dashboard summary statistics
    /// </summary>
    Task<DashboardSummaryDto> GetSummaryAsync(int? brandId = null);
    
    /// <summary>
    /// Get statistics for a specific time period
    /// </summary>
    Task<PeriodStatsDto> GetPeriodStatsAsync(DateTime startDate, DateTime endDate, int? brandId = null);
    
    /// <summary>
    /// Get order statistics grouped by status
    /// </summary>
    Task<List<OrderStatsDto>> GetOrderStatsByStatusAsync(int? brandId = null);
    
    /// <summary>
    /// Get product statistics grouped by category
    /// </summary>
    Task<List<ProductByCategoryDto>> GetProductsByCategoryAsync(int? brandId = null);
    
    /// <summary>
    /// Get top selling products
    /// </summary>
    Task<List<TopProductDto>> GetTopProductsAsync(int count = 10, int? brandId = null);
    
    /// <summary>
    /// Get recent orders
    /// </summary>
    Task<List<RecentOrderDto>> GetRecentOrdersAsync(int count = 10, int? brandId = null);
    
    /// <summary>
    /// Get revenue by brand
    /// </summary>
    Task<List<RevenuByBrandDto>> GetRevenueByBrandAsync(DateTime? startDate = null, DateTime? endDate = null);
}
