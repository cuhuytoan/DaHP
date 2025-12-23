using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== Order CRUD =====================

    public async Task<ProductOrderDto?> GetByIdAsync(long id)
    {
        var order = await _context.ProductOrders
            .Include(o => o.ProductBrand)
            .Include(o => o.ProductOrderStatus)
            .Include(o => o.ProductOrderPaymentMethod)
            .Include(o => o.ProductOrderPaymentStatus)
            .Include(o => o.Location)
            .Include(o => o.District)
            .Include(o => o.Ward)
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order == null ? null : MapToDto(order);
    }

    public async Task<ProductOrderDto?> GetByOrderCodeAsync(string orderCode)
    {
        var order = await _context.ProductOrders
            .Include(o => o.ProductBrand)
            .Include(o => o.ProductOrderStatus)
            .Include(o => o.ProductOrderPaymentMethod)
            .Include(o => o.ProductOrderPaymentStatus)
            .Include(o => o.Location)
            .Include(o => o.District)
            .Include(o => o.Ward)
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

        return order == null ? null : MapToDto(order);
    }

    public async Task<PagedResult<ProductOrderListDto>> SearchAsync(OrderSearchRequest request)
    {
        var query = _context.ProductOrders
            .Include(o => o.ProductOrderStatus)
            .Include(o => o.ProductOrderPaymentStatus)
            .Include(o => o.OrderDetails)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(o => 
                (o.OrderCode != null && o.OrderCode.Contains(request.Keyword)) ||
                (o.CustomerName != null && o.CustomerName.Contains(request.Keyword)) ||
                (o.CustomerPhone != null && o.CustomerPhone.Contains(request.Keyword)));
        }

        if (!string.IsNullOrEmpty(request.OrderCode))
        {
            query = query.Where(o => o.OrderCode == request.OrderCode);
        }

        if (request.ProductOrderStatusId.HasValue)
        {
            query = query.Where(o => o.ProductOrderStatusId == request.ProductOrderStatusId);
        }

        if (request.ProductOrderPaymentStatusId.HasValue)
        {
            query = query.Where(o => o.ProductOrderPaymentStatusId == request.ProductOrderPaymentStatusId);
        }

        if (request.ProductOrderPaymentMethodId.HasValue)
        {
            query = query.Where(o => o.ProductOrderPaymentMethodId == request.ProductOrderPaymentMethodId);
        }

        if (request.ProductBrandId.HasValue)
        {
            query = query.Where(o => o.ProductBrandId == request.ProductBrandId);
        }

        if (!string.IsNullOrEmpty(request.UserId))
        {
            query = query.Where(o => o.UserId == request.UserId);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(o => o.CreateDate >= request.FromDate);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(o => o.CreateDate <= request.ToDate);
        }

        if (request.MinTotal.HasValue)
        {
            query = query.Where(o => o.Total >= request.MinTotal);
        }

        if (request.MaxTotal.HasValue)
        {
            query = query.Where(o => o.Total <= request.MaxTotal);
        }

        var totalCount = await query.CountAsync();

        query = request.SortBy?.ToLower() switch
        {
            "ordercode" => request.SortDescending ? query.OrderByDescending(o => o.OrderCode) : query.OrderBy(o => o.OrderCode),
            "total" => request.SortDescending ? query.OrderByDescending(o => o.Total) : query.OrderBy(o => o.Total),
            _ => request.SortDescending ? query.OrderByDescending(o => o.CreateDate) : query.OrderBy(o => o.CreateDate)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => MapToListDto(o))
            .ToListAsync();

        return new PagedResult<ProductOrderListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            }
        };
    }

    public async Task<ProductOrderDto> CreateAsync(CreateOrderRequest request, string? userId = null)
    {
        var orderCode = $"ORD{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";

        var order = new ProductOrder
        {
            OrderCode = orderCode,
            UserId = userId,
            ProductBrandId = request.ProductBrandId,
            ProductOrderStatusId = 1,
            ProductOrderPaymentMethodId = request.ProductOrderPaymentMethodId,
            ProductOrderPaymentStatusId = 1,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            CustomerAddress = request.CustomerAddress,
            LocationId = request.LocationId,
            DistrictId = request.DistrictId,
            WardId = request.WardId,
            Note = request.Note,
            CouponCode = request.CouponCode,
            Active = true,
            CreateBy = userId,
            CreateDate = DateTime.Now
        };

        decimal subTotal = 0;

        foreach (var item in request.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) continue;

            var price = product.Price ?? 0;
            var total = price * item.Quantity;
            subTotal += total;

            order.OrderDetails.Add(new ProductOrderDetail
            {
                ProductId = item.ProductId,
                ProductName = product.Name,
                ProductImage = product.Image,
                ProductCode = product.Sku,
                Price = price,
                Quantity = item.Quantity,
                Total = total,
                Note = item.Note,
                CreateBy = userId,
                CreateDate = DateTime.Now
            });
        }

        order.SubTotal = subTotal;
        order.Total = subTotal + (order.ShippingFee ?? 0) - (order.Discount ?? 0) + (order.Tax ?? 0);

        _context.ProductOrders.Add(order);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync((int)order.Id))!;
    }

    public async Task<bool> DeleteAsync(long id, string userId)
    {
        var order = await _context.ProductOrders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return false;

        _context.ProductOrderDetails.RemoveRange(order.OrderDetails);
        _context.ProductOrders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Order Status Management =====================

    public async Task<bool> UpdateStatusAsync(long id, UpdateOrderStatusRequest request, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = request.ProductOrderStatusId;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePaymentStatusAsync(long id, UpdatePaymentStatusRequest request, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderPaymentStatusId = request.ProductOrderPaymentStatusId;
        order.TransactionId = request.TransactionId;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        if (request.ProductOrderPaymentStatusId == 2)
        {
            order.PaymentDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(long id, CancelOrderRequest request, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = 5;
        order.CancelDate = DateTime.Now;
        order.CancelReason = request.CancelReason;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ConfirmAsync(long id, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = 2;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ShipAsync(long id, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = 3;
        order.ShippingDate = DateTime.Now;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeliverAsync(long id, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = 4;
        order.DeliveryDate = DateTime.Now;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteAsync(long id, string userId)
    {
        var order = await _context.ProductOrders.FindAsync((long)id);
        if (order == null) return false;

        order.ProductOrderStatusId = 6;
        order.LastEditBy = userId;
        order.LastEditDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Order Details =====================

    public async Task<List<ProductOrderDetailDto>> GetOrderDetailsAsync(long orderId)
    {
        return await _context.ProductOrderDetails
            .Where(d => d.ProductOrderId == orderId)
            .Select(d => new ProductOrderDetailDto
            {
                Id = (int)d.Id,
                ProductOrderId = d.ProductOrderId,
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                ProductImage = d.ProductImage,
                ProductCode = d.ProductCode,
                Price = d.Price,
                Quantity = d.Quantity,
                Discount = d.Discount,
                Total = d.Total,
                Note = d.Note
            })
            .ToListAsync();
    }

    // ===================== User Orders =====================

    public async Task<PagedResult<ProductOrderListDto>> GetUserOrdersAsync(string userId, int page = 1, int pageSize = 20)
    {
        var query = _context.ProductOrders
            .Include(o => o.ProductOrderStatus)
            .Include(o => o.ProductOrderPaymentStatus)
            .Include(o => o.OrderDetails)
            .Where(o => o.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(o => o.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => MapToListDto(o))
            .ToListAsync();

        return new PagedResult<ProductOrderListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ProductOrderDto?> GetUserOrderAsync(string userId, long orderId)
    {
        var order = await _context.ProductOrders
            .Include(o => o.ProductBrand)
            .Include(o => o.ProductOrderStatus)
            .Include(o => o.ProductOrderPaymentMethod)
            .Include(o => o.ProductOrderPaymentStatus)
            .Include(o => o.Location)
            .Include(o => o.District)
            .Include(o => o.Ward)
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        return order == null ? null : MapToDto(order);
    }

    // ===================== Statistics =====================

    public async Task<OrderStatisticsDto> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.ProductOrders.AsQueryable();

        if (fromDate.HasValue) query = query.Where(o => o.CreateDate >= fromDate);
        if (toDate.HasValue) query = query.Where(o => o.CreateDate <= toDate);

        var orders = await query.ToListAsync();
        var statuses = await _context.ProductOrderStatuses.ToListAsync();

        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);

        return new OrderStatisticsDto
        {
            TotalOrders = orders.Count,
            PendingOrders = orders.Count(o => o.ProductOrderStatusId == 1),
            ProcessingOrders = orders.Count(o => o.ProductOrderStatusId == 2),
            CompletedOrders = orders.Count(o => o.ProductOrderStatusId == 6),
            CancelledOrders = orders.Count(o => o.ProductOrderStatusId == 5),
            TotalRevenue = orders.Where(o => o.ProductOrderStatusId != 5).Sum(o => o.Total ?? 0),
            TodayRevenue = orders.Where(o => o.CreateDate?.Date == today && o.ProductOrderStatusId != 5).Sum(o => o.Total ?? 0),
            MonthRevenue = orders.Where(o => o.CreateDate >= monthStart && o.ProductOrderStatusId != 5).Sum(o => o.Total ?? 0),
            StatusCounts = statuses.Select(s => new OrderStatusCountDto
            {
                StatusId = s.Id,
                StatusName = s.Name,
                StatusColor = s.Color,
                Count = orders.Count(o => o.ProductOrderStatusId == s.Id)
            }).ToList(),
            DailyRevenue = orders
                .Where(o => o.CreateDate.HasValue && o.ProductOrderStatusId != 5)
                .GroupBy(o => o.CreateDate!.Value.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.Total ?? 0),
                    OrderCount = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList()
        };
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.ProductOrders.CountAsync();
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _context.ProductOrders.CountAsync(o => o.ProductOrderStatusId == 1);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.ProductOrders.Where(o => o.ProductOrderStatusId != 5);

        if (fromDate.HasValue) query = query.Where(o => o.CreateDate >= fromDate);
        if (toDate.HasValue) query = query.Where(o => o.CreateDate <= toDate);

        return await query.SumAsync(o => o.Total ?? 0);
    }

    // ===================== Lookup Data =====================

    public async Task<List<ProductOrderStatusDto>> GetOrderStatusesAsync()
    {
        return await _context.ProductOrderStatuses
            .OrderBy(s => s.Sort)
            .Select(s => new ProductOrderStatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
                Sort = s.Sort
            })
            .ToListAsync();
    }

    public async Task<List<ProductOrderPaymentMethodDto>> GetPaymentMethodsAsync()
    {
        return await _context.ProductOrderPaymentMethods
            .Where(p => p.Active == true)
            .OrderBy(p => p.Sort)
            .Select(p => new ProductOrderPaymentMethodDto
            {
                Id = (int)p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Sort = p.Sort,
                Active = p.Active
            })
            .ToListAsync();
    }

    public async Task<List<ProductOrderPaymentStatusDto>> GetPaymentStatusesAsync()
    {
        return await _context.ProductOrderPaymentStatuses
            .Select(s => new ProductOrderPaymentStatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color
            })
            .ToListAsync();
    }

    // ===================== Private Helpers =====================

    private static ProductOrderListDto MapToListDto(ProductOrder o)
    {
        return new ProductOrderListDto
        {
            Id = (int)o.Id,
            OrderCode = o.OrderCode,
            CustomerName = o.CustomerName,
            CustomerPhone = o.CustomerPhone,
            ProductOrderStatusId = o.ProductOrderStatusId,
            ProductOrderStatusName = o.ProductOrderStatus?.Name,
            ProductOrderStatusColor = o.ProductOrderStatus?.Color,
            ProductOrderPaymentStatusId = o.ProductOrderPaymentStatusId,
            ProductOrderPaymentStatusName = o.ProductOrderPaymentStatus?.Name,
            ProductOrderPaymentStatusColor = o.ProductOrderPaymentStatus?.Color,
            Total = o.Total,
            ItemCount = o.OrderDetails.Count,
            CreateDate = o.CreateDate
        };
    }

    private static ProductOrderDto MapToDto(ProductOrder order)
    {
        return new ProductOrderDto
        {
            Id = (int)order.Id,
            OrderCode = order.OrderCode,
            UserId = order.UserId,
            ProductBrandId = order.ProductBrandId,
            ProductBrandName = order.ProductBrand?.Name,
            ProductOrderStatusId = order.ProductOrderStatusId,
            ProductOrderStatusName = order.ProductOrderStatus?.Name,
            ProductOrderStatusColor = order.ProductOrderStatus?.Color,
            ProductOrderPaymentMethodId = order.ProductOrderPaymentMethodId,
            ProductOrderPaymentMethodName = order.ProductOrderPaymentMethod?.Name,
            ProductOrderPaymentStatusId = order.ProductOrderPaymentStatusId,
            ProductOrderPaymentStatusName = order.ProductOrderPaymentStatus?.Name,
            ProductOrderPaymentStatusColor = order.ProductOrderPaymentStatus?.Color,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            CustomerPhone = order.CustomerPhone,
            CustomerAddress = order.CustomerAddress,
            LocationId = order.LocationId,
            LocationName = order.Location?.Name,
            DistrictId = order.DistrictId,
            DistrictName = order.District?.Name,
            WardId = order.WardId,
            WardName = order.Ward?.Name,
            Note = order.Note,
            SubTotal = order.SubTotal,
            ShippingFee = order.ShippingFee,
            Discount = order.Discount,
            Tax = order.Tax,
            Total = order.Total,
            CouponCode = order.CouponCode,
            TransactionId = order.TransactionId,
            PaymentDate = order.PaymentDate,
            ShippingDate = order.ShippingDate,
            DeliveryDate = order.DeliveryDate,
            CancelDate = order.CancelDate,
            CancelReason = order.CancelReason,
            Active = order.Active,
            CreateBy = order.CreateBy,
            CreateDate = order.CreateDate,
            LastEditBy = order.LastEditBy,
            LastEditDate = order.LastEditDate,
            OrderDetails = order.OrderDetails.Select(d => new ProductOrderDetailDto
            {
                Id = (int)d.Id,
                ProductOrderId = d.ProductOrderId,
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                ProductImage = d.ProductImage,
                ProductCode = d.ProductCode,
                Price = d.Price,
                Quantity = d.Quantity,
                Discount = d.Discount,
                Total = d.Total,
                Note = d.Note
            }).ToList()
        };
    }
}
