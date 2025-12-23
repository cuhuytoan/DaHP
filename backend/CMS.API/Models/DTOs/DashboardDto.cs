using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Dashboard summary statistics
/// </summary>
public class DashboardSummaryDto
{
    // Product stats
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int PendingProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    
    // Article stats
    public int TotalArticles { get; set; }
    public int ActiveArticles { get; set; }
    public int PendingArticles { get; set; }
    
    // Order stats
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
    
    // Revenue stats
    public decimal TotalRevenue { get; set; }
    public decimal TodayRevenue { get; set; }
    public decimal ThisMonthRevenue { get; set; }
    
    // User stats
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsersToday { get; set; }
    public int NewUsersThisMonth { get; set; }
    
    // Brand stats
    public int TotalBrands { get; set; }
    public int ActiveBrands { get; set; }
    public int PendingBrands { get; set; }
    
    // Comment stats
    public int TotalComments { get; set; }
    public int PendingComments { get; set; }
}

/// <summary>
/// Time period based statistics
/// </summary>
public class PeriodStatsDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<DailyStatsDto> DailyStats { get; set; } = new();
    public PeriodSummaryDto Summary { get; set; } = new();
}

public class DailyStatsDto
{
    public DateTime Date { get; set; }
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
    public int NewUsers { get; set; }
    public int NewProducts { get; set; }
    public int NewArticles { get; set; }
    public int PageViews { get; set; }
}

public class PeriodSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalNewUsers { get; set; }
    public int TotalNewProducts { get; set; }
    public int TotalNewArticles { get; set; }
    public int TotalPageViews { get; set; }
    
    // Compared to previous period
    public decimal OrderGrowthRate { get; set; }
    public decimal RevenueGrowthRate { get; set; }
    public decimal UserGrowthRate { get; set; }
}

/// <summary>
/// Order statistics by status
/// </summary>
public class OrderStatsDto
{
    public long StatusId { get; set; }
    public string? StatusName { get; set; }
    public string? StatusColor { get; set; }
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Product statistics by category
/// </summary>
public class ProductByCategoryDto
{
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int ProductCount { get; set; }
    public int ActiveCount { get; set; }
}

/// <summary>
/// Top selling products
/// </summary>
public class TopProductDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Sku { get; set; }
    public decimal? Price { get; set; }
    public int SellCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

/// <summary>
/// Recent orders
/// </summary>
public class RecentOrderDto
{
    public long Id { get; set; }
    public string? OrderCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal? Total { get; set; }
    public long? StatusId { get; set; }
    public string? StatusName { get; set; }
    public string? StatusColor { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Revenue by brand
/// </summary>
public class RevenuByBrandDto
{
    public long BrandId { get; set; }
    public string? BrandName { get; set; }
    public string? Logo { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

/// <summary>
/// Request for period-based statistics
/// </summary>
public class PeriodStatsRequest
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public long? BrandId { get; set; }
}
