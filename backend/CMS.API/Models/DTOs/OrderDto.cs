using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.DTOs;

/// <summary>
/// Product order DTO
/// </summary>
public class ProductOrderDto
{
    public long Id { get; set; }
    public string? OrderCode { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public long? ProductBrandId { get; set; }
    public string? ProductBrandName { get; set; }
    public long? ProductOrderStatusId { get; set; }
    public string? ProductOrderStatusName { get; set; }
    public string? ProductOrderStatusColor { get; set; }
    public long? ProductOrderPaymentMethodId { get; set; }
    public string? ProductOrderPaymentMethodName { get; set; }
    public long? ProductOrderPaymentStatusId { get; set; }
    public string? ProductOrderPaymentStatusName { get; set; }
    public string? ProductOrderPaymentStatusColor { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerAddress { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public long? DistrictId { get; set; }
    public string? DistrictName { get; set; }
    public long? WardId { get; set; }
    public string? WardName { get; set; }
    public string? Note { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? ShippingFee { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Total { get; set; }
    public string? CouponCode { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? CancelDate { get; set; }
    public string? CancelReason { get; set; }
    public bool? Active { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditBy { get; set; }
    public DateTime? LastEditDate { get; set; }
    public List<ProductOrderDetailDto> OrderDetails { get; set; } = new();
}

/// <summary>
/// Product order list DTO
/// </summary>
public class ProductOrderListDto
{
    public long Id { get; set; }
    public string? OrderCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public long? ProductOrderStatusId { get; set; }
    public string? ProductOrderStatusName { get; set; }
    public string? ProductOrderStatusColor { get; set; }
    public long? ProductOrderPaymentStatusId { get; set; }
    public string? ProductOrderPaymentStatusName { get; set; }
    public string? ProductOrderPaymentStatusColor { get; set; }
    public decimal? Total { get; set; }
    public int ItemCount { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// Product order detail DTO
/// </summary>
public class ProductOrderDetailDto
{
    public long Id { get; set; }
    public long? ProductOrderId { get; set; }
    public long? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductUrl { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Total { get; set; }
    public string? Note { get; set; }
}

/// <summary>
/// Create order request DTO
/// </summary>
public class CreateOrderRequest
{
    public long? ProductBrandId { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(200)]
    [EmailAddress]
    public string? CustomerEmail { get; set; }

    [Required(ErrorMessage = "Customer phone is required")]
    [StringLength(50)]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer address is required")]
    [StringLength(500)]
    public string CustomerAddress { get; set; } = string.Empty;

    public long? LocationId { get; set; }

    public long? DistrictId { get; set; }

    public long? WardId { get; set; }

    public string? Note { get; set; }

    [Required]
    public long ProductOrderPaymentMethodId { get; set; }

    [StringLength(50)]
    public string? CouponCode { get; set; }

    [Required(ErrorMessage = "Order items are required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<CreateOrderDetailRequest> Items { get; set; } = new();
}

/// <summary>
/// Create order detail request DTO
/// </summary>
public class CreateOrderDetailRequest
{
    [Required]
    public long ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public string? Note { get; set; }
}

/// <summary>
/// Update order status request DTO
/// </summary>
public class UpdateOrderStatusRequest
{
    [Required]
    public long ProductOrderStatusId { get; set; }

    public string? Note { get; set; }
}

/// <summary>
/// Update payment status request DTO
/// </summary>
public class UpdatePaymentStatusRequest
{
    [Required]
    public long ProductOrderPaymentStatusId { get; set; }

    [StringLength(200)]
    public string? TransactionId { get; set; }

    public string? Note { get; set; }
}

/// <summary>
/// Cancel order request DTO
/// </summary>
public class CancelOrderRequest
{
    [Required(ErrorMessage = "Cancel reason is required")]
    [StringLength(500)]
    public string CancelReason { get; set; } = string.Empty;
}

/// <summary>
/// Order search request DTO
/// </summary>
public class OrderSearchRequest
{
    public string? Keyword { get; set; }
    public string? OrderCode { get; set; }
    public long? ProductOrderStatusId { get; set; }
    public long? ProductOrderPaymentStatusId { get; set; }
    public long? ProductOrderPaymentMethodId { get; set; }
    public long? ProductBrandId { get; set; }
    public string? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public decimal? MinTotal { get; set; }
    public decimal? MaxTotal { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Order status DTO
/// </summary>
public class ProductOrderStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public int? Sort { get; set; }
}

/// <summary>
/// Payment method DTO
/// </summary>
public class ProductOrderPaymentMethodDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int? Sort { get; set; }
    public bool? Active { get; set; }
}

/// <summary>
/// Payment status DTO
/// </summary>
public class ProductOrderPaymentStatusDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
}

/// <summary>
/// Order statistics DTO
/// </summary>
public class OrderStatisticsDto
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TodayRevenue { get; set; }
    public decimal MonthRevenue { get; set; }
    public List<OrderStatusCountDto> StatusCounts { get; set; } = new();
    public List<DailyRevenueDto> DailyRevenue { get; set; } = new();
}

/// <summary>
/// Order status count DTO
/// </summary>
public class OrderStatusCountDto
{
    public long StatusId { get; set; }
    public string? StatusName { get; set; }
    public string? StatusColor { get; set; }
    public int Count { get; set; }
}

/// <summary>
/// Daily revenue DTO
/// </summary>
public class DailyRevenueDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}