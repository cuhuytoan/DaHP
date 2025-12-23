using CMS.API.Data;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(int? brandId = null)
    {
        var today = DateTime.UtcNow.Date;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

        return new DashboardSummaryDto
        {
            TotalProducts = await _context.Products.CountAsync(),
            ActiveProducts = await _context.Products.CountAsync(p => p.Active == true),
            PendingProducts = await _context.Products.CountAsync(p => p.Approved == 0),
            OutOfStockProducts = await _context.Products.CountAsync(p => p.IsOutStock == true),
            TotalArticles = await _context.Articles.CountAsync(),
            ActiveArticles = await _context.Articles.CountAsync(a => a.Active == true),
            PendingArticles = await _context.Articles.CountAsync(a => a.Approved == 0),
            TotalOrders = await _context.ProductOrders.CountAsync(o => o.Active == true),
            PendingOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.ProductOrderStatusId == 1),
            ProcessingOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.ProductOrderStatusId == 2),
            CompletedOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.ProductOrderStatusId == 6),
            CancelledOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.ProductOrderStatusId == 5),
            TotalRevenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6).SumAsync(o => o.Total ?? 0),
            TodayRevenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.CreateDate >= today).SumAsync(o => o.Total ?? 0),
            ThisMonthRevenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.CreateDate >= firstDayOfMonth).SumAsync(o => o.Total ?? 0),
            TotalUsers = await _context.Users.CountAsync(),
            ActiveUsers = await _context.UserProfiles.CountAsync(p => p.Verified == true),
            NewUsersToday = await _context.UserProfiles.CountAsync(p => p.RegisterDate >= today),
            NewUsersThisMonth = await _context.UserProfiles.CountAsync(p => p.RegisterDate >= firstDayOfMonth),
            TotalBrands = await _context.ProductBrands.CountAsync(),
            ActiveBrands = await _context.ProductBrands.CountAsync(b => b.Active == true),
            PendingBrands = await _context.ProductBrands.CountAsync(b => b.Approved == 0),
            TotalComments = await _context.ProductComments.CountAsync() + await _context.ArticleComments.CountAsync(),
            PendingComments = await _context.ProductComments.CountAsync(c => c.Active != true) + await _context.ArticleComments.CountAsync(c => c.Active != true)
        };
    }

    public async Task<PeriodStatsDto> GetPeriodStatsAsync(DateTime startDate, DateTime endDate, int? brandId = null)
    {
        var dailyStats = new List<DailyStatsDto>();
        var currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            var nextDate = currentDate.AddDays(1);
            dailyStats.Add(new DailyStatsDto
            {
                Date = currentDate,
                OrderCount = await _context.ProductOrders.CountAsync(o => o.Active == true && o.CreateDate >= currentDate && o.CreateDate < nextDate),
                Revenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.CreateDate >= currentDate && o.CreateDate < nextDate).SumAsync(o => o.Total ?? 0),
                NewUsers = await _context.UserProfiles.CountAsync(p => p.RegisterDate >= currentDate && p.RegisterDate < nextDate),
                NewProducts = await _context.Products.CountAsync(p => p.CreateDate >= currentDate && p.CreateDate < nextDate),
                NewArticles = await _context.Articles.CountAsync(a => a.CreateDate >= currentDate && a.CreateDate < nextDate)
            });
            currentDate = nextDate;
        }

        var periodDays = (endDate - startDate).Days;
        var previousStartDate = startDate.AddDays(-periodDays);
        var previousEndDate = startDate;

        var currentOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.CreateDate >= startDate && o.CreateDate <= endDate);
        var previousOrders = await _context.ProductOrders.CountAsync(o => o.Active == true && o.CreateDate >= previousStartDate && o.CreateDate < previousEndDate);

        var currentRevenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.CreateDate >= startDate && o.CreateDate <= endDate).SumAsync(o => o.Total ?? 0);
        var previousRevenue = await _context.ProductOrders.Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.CreateDate >= previousStartDate && o.CreateDate < previousEndDate).SumAsync(o => o.Total ?? 0);

        var currentUsers = await _context.UserProfiles.CountAsync(p => p.RegisterDate >= startDate && p.RegisterDate <= endDate);
        var previousUsers = await _context.UserProfiles.CountAsync(p => p.RegisterDate >= previousStartDate && p.RegisterDate < previousEndDate);

        return new PeriodStatsDto
        {
            StartDate = startDate,
            EndDate = endDate,
            DailyStats = dailyStats,
            Summary = new PeriodSummaryDto
            {
                TotalOrders = dailyStats.Sum(d => d.OrderCount),
                TotalRevenue = dailyStats.Sum(d => d.Revenue),
                TotalNewUsers = dailyStats.Sum(d => d.NewUsers),
                TotalNewProducts = dailyStats.Sum(d => d.NewProducts),
                TotalNewArticles = dailyStats.Sum(d => d.NewArticles),
                OrderGrowthRate = previousOrders > 0 ? ((decimal)(currentOrders - previousOrders) / previousOrders) * 100 : 0,
                RevenueGrowthRate = previousRevenue > 0 ? ((currentRevenue - previousRevenue) / previousRevenue) * 100 : 0,
                UserGrowthRate = previousUsers > 0 ? ((decimal)(currentUsers - previousUsers) / previousUsers) * 100 : 0
            }
        };
    }

    public async Task<List<OrderStatsDto>> GetOrderStatsByStatusAsync(int? brandId = null)
    {
        var statuses = await _context.ProductOrderStatuses.AsNoTracking().ToListAsync();
        var result = new List<OrderStatsDto>();

        foreach (var status in statuses)
        {
            var query = _context.ProductOrders.AsNoTracking().Where(o => o.Active == true && o.ProductOrderStatusId == status.Id);
            if (brandId.HasValue) query = query.Where(o => o.ProductBrandId == brandId);

            result.Add(new OrderStatsDto
            {
                StatusId = status.Id,
                StatusName = status.Name,
                StatusColor = status.Color,
                Count = await query.CountAsync(),
                TotalAmount = await query.SumAsync(o => o.Total ?? 0)
            });
        }

        return result;
    }

    public async Task<List<ProductByCategoryDto>> GetProductsByCategoryAsync(int? brandId = null)
    {
        var categories = await _context.ProductCategories.AsNoTracking().Where(c => c.Active == true).Take(20).ToListAsync();
        var result = new List<ProductByCategoryDto>();

        foreach (var category in categories)
        {
            var categoryIdStr = category.Id.ToString();
            var query = _context.Products.AsNoTracking().Where(p => p.ProductCategoryIds != null && p.ProductCategoryIds.Contains(categoryIdStr));
            if (brandId.HasValue) query = query.Where(p => p.ProductBrandId == brandId);

            result.Add(new ProductByCategoryDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                ProductCount = await query.CountAsync(),
                ActiveCount = await query.CountAsync(p => p.Active == true)
            });
        }

        return result.OrderByDescending(r => r.ProductCount).ToList();
    }

    public async Task<List<TopProductDto>> GetTopProductsAsync(int count = 10, int? brandId = null)
    {
        var query = _context.Products.AsNoTracking().Where(p => p.Active == true);
        if (brandId.HasValue) query = query.Where(p => p.ProductBrandId == brandId);

        return await query.OrderByDescending(p => p.SellCount)
            .Take(count)
            .Select(p => new TopProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                Sku = p.Sku,
                Price = p.Price,
                SellCount = p.SellCount ?? 0,
                TotalRevenue = (p.Price ?? 0) * (p.SellCount ?? 0)
            })
            .ToListAsync();
    }

    public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(int count = 10, int? brandId = null)
    {
        var query = _context.ProductOrders.AsNoTracking().Where(o => o.Active == true);
        if (brandId.HasValue) query = query.Where(o => o.ProductBrandId == brandId);

        var orders = await query.OrderByDescending(o => o.CreateDate).Take(count).ToListAsync();
        var statuses = await _context.ProductOrderStatuses.AsNoTracking().ToListAsync();

        return orders.Select(o =>
        {
            var status = statuses.FirstOrDefault(s => s.Id == o.ProductOrderStatusId);
            return new RecentOrderDto
            {
                Id = o.Id,
                OrderCode = o.OrderCode,
                CustomerName = o.CustomerName,
                CustomerPhone = o.CustomerPhone,
                Total = o.Total,
                StatusId = o.ProductOrderStatusId,
                StatusName = status?.Name,
                StatusColor = status?.Color,
                CreateDate = o.CreateDate
            };
        }).ToList();
    }

    public async Task<List<RevenuByBrandDto>> GetRevenueByBrandAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var brands = await _context.ProductBrands.AsNoTracking().Where(b => b.Active == true).ToListAsync();
        var result = new List<RevenuByBrandDto>();

        foreach (var brand in brands)
        {
            var query = _context.ProductOrders.AsNoTracking().Where(o => o.Active == true && o.ProductOrderStatusId == 6 && o.ProductBrandId == brand.Id);
            if (startDate.HasValue) query = query.Where(o => o.CreateDate >= startDate);
            if (endDate.HasValue) query = query.Where(o => o.CreateDate <= endDate);

            result.Add(new RevenuByBrandDto
            {
                BrandId = (int)brand.Id,
                BrandName = brand.Name,
                Logo = brand.Image,
                OrderCount = await query.CountAsync(),
                TotalRevenue = await query.SumAsync(o => o.Total ?? 0)
            });
        }

        return result.OrderByDescending(r => r.TotalRevenue).ToList();
    }
}
