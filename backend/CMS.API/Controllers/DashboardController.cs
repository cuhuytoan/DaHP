using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Dashboard statistics API for admin panel
/// </summary>
[Authorize(Policy = "EditorOrAdmin")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get dashboard summary statistics
    /// </summary>
    /// <param name="brandId">Optional brand filter</param>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] int? brandId = null)
    {
        try
        {
            var result = await _dashboardService.GetSummaryAsync(brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard summary");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get statistics for a specific time period
    /// </summary>
    [HttpGet("period-stats")]
    [ProducesResponseType(typeof(PeriodStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPeriodStats(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int? brandId = null)
    {
        try
        {
            if (startDate > endDate)
                return BadRequest(new { message = "Start date must be before end date" });

            if ((endDate - startDate).Days > 365)
                return BadRequest(new { message = "Date range cannot exceed 365 days" });

            var result = await _dashboardService.GetPeriodStatsAsync(startDate, endDate, brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting period stats");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get order statistics grouped by status
    /// </summary>
    [HttpGet("order-stats")]
    [ProducesResponseType(typeof(List<OrderStatsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderStats([FromQuery] int? brandId = null)
    {
        try
        {
            var result = await _dashboardService.GetOrderStatsByStatusAsync(brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order stats");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get product count by category
    /// </summary>
    [HttpGet("products-by-category")]
    [ProducesResponseType(typeof(List<ProductByCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsByCategory([FromQuery] int? brandId = null)
    {
        try
        {
            var result = await _dashboardService.GetProductsByCategoryAsync(brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by category");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get top selling products
    /// </summary>
    [HttpGet("top-products")]
    [ProducesResponseType(typeof(List<TopProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopProducts(
        [FromQuery] int count = 10,
        [FromQuery] int? brandId = null)
    {
        try
        {
            if (count < 1 || count > 100)
                count = 10;

            var result = await _dashboardService.GetTopProductsAsync(count, brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top products");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get recent orders
    /// </summary>
    [HttpGet("recent-orders")]
    [ProducesResponseType(typeof(List<RecentOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentOrders(
        [FromQuery] int count = 10,
        [FromQuery] int? brandId = null)
    {
        try
        {
            if (count < 1 || count > 100)
                count = 10;

            var result = await _dashboardService.GetRecentOrdersAsync(count, brandId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent orders");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get revenue by brand
    /// </summary>
    [HttpGet("revenue-by-brand")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(List<RevenuByBrandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueByBrand(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _dashboardService.GetRevenueByBrandAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue by brand");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get today's summary (quick stats for dashboard header)
    /// </summary>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTodayStats([FromQuery] int? brandId = null)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var summary = await _dashboardService.GetSummaryAsync(brandId);
            var recentOrders = await _dashboardService.GetRecentOrdersAsync(5, brandId);

            return Ok(new
            {
                TodayRevenue = summary.TodayRevenue,
                TotalOrders = summary.TotalOrders,
                PendingOrders = summary.PendingOrders,
                NewUsersToday = summary.NewUsersToday,
                PendingProducts = summary.PendingProducts,
                PendingArticles = summary.PendingArticles,
                PendingComments = summary.PendingComments,
                RecentOrders = recentOrders
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting today stats");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
