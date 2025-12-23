using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Orders API controller
/// </summary>
[Route("api/[controller]")]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Get all orders with pagination and filtering (Admin only)
    /// </summary>
    /// <param name="request">Search parameters</param>
    /// <returns>Paginated list of orders</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductOrderListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders([FromQuery] OrderSearchRequest request)
    {
        var result = await _orderService.SearchAsync(request);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Order details</returns>
    [HttpGet("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(long id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            return ApiNotFound("Order not found");
        }

        // Check if user has access to this order
        if (!IsInRole("Admin") && order.UserId != CurrentUserId)
        {
            return ApiForbidden("You don't have access to this order");
        }

        return ApiSuccess(order);
    }

    /// <summary>
    /// Get order by order code
    /// </summary>
    /// <param name="orderCode">Order code</param>
    /// <returns>Order details</returns>
    [HttpGet("code/{orderCode}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderByCode(string orderCode)
    {
        var order = await _orderService.GetByOrderCodeAsync(orderCode);
        if (order == null)
        {
            return ApiNotFound("Order not found");
        }

        // Check if user has access to this order (if authenticated)
        if (User.Identity?.IsAuthenticated == true && !IsInRole("Admin") && order.UserId != CurrentUserId)
        {
            return ApiForbidden("You don't have access to this order");
        }

        return ApiSuccess(order);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="request">Order data</param>
    /// <returns>Created order</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductOrderDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var order = await _orderService.CreateAsync(request, CurrentUserId);
        _logger.LogInformation("Order {OrderCode} created", order.OrderCode);
        
        return ApiCreated(order, $"/api/orders/{order.Id}", "Order created successfully");
    }

    /// <summary>
    /// Update order status
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="request">Status update data</param>
    /// <returns>Success status</returns>
    [HttpPut("{id:long}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(long id, [FromBody] UpdateOrderStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.UpdateStatusAsync(id, request, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Order status updated successfully");
    }

    /// <summary>
    /// Update payment status
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="request">Payment status update data</param>
    /// <returns>Success status</returns>
    [HttpPut("{id:long}/payment-status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePaymentStatus(long id, [FromBody] UpdatePaymentStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.UpdatePaymentStatusAsync(id, request, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Payment status updated successfully");
    }

    /// <summary>
    /// Cancel an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="request">Cancel reason</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/cancel")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelOrder(long id, [FromBody] CancelOrderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        // Check if user has access to cancel this order
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            return ApiNotFound("Order not found");
        }

        if (!IsInRole("Admin") && order.UserId != userId)
        {
            return ApiForbidden("You don't have permission to cancel this order");
        }

        var result = await _orderService.CancelAsync(id, request, userId);
        if (!result)
        {
            return ApiError("Failed to cancel order. Order may not be in a cancellable state.");
        }

        return ApiSuccess(true, "Order cancelled successfully");
    }

    /// <summary>
    /// Confirm an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/confirm")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmOrder(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.ConfirmAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Order confirmed successfully");
    }

    /// <summary>
    /// Ship an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/ship")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ShipOrder(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.ShipAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Order shipped successfully");
    }

    /// <summary>
    /// Mark order as delivered
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/deliver")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeliverOrder(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.DeliverAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Order marked as delivered");
    }

    /// <summary>
    /// Complete an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/complete")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteOrder(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.CompleteAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Order not found");
        }

        return ApiSuccess(true, "Order completed successfully");
    }

    /// <summary>
    /// Get current user's orders
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated list of user's orders</returns>
    [HttpGet("my-orders")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductOrderListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _orderService.GetUserOrdersAsync(userId, page, pageSize);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Get order statistics
    /// </summary>
    /// <param name="fromDate">Start date</param>
    /// <param name="toDate">End date</param>
    /// <returns>Order statistics</returns>
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<OrderStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var statistics = await _orderService.GetStatisticsAsync(fromDate, toDate);
        return ApiSuccess(statistics);
    }

    /// <summary>
    /// Get order statuses
    /// </summary>
    /// <returns>List of order statuses</returns>
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductOrderStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderStatuses()
    {
        var statuses = await _orderService.GetOrderStatusesAsync();
        return ApiSuccess(statuses);
    }

    /// <summary>
    /// Get payment methods
    /// </summary>
    /// <returns>List of payment methods</returns>
    [HttpGet("payment-methods")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductOrderPaymentMethodDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods()
    {
        var methods = await _orderService.GetPaymentMethodsAsync();
        return ApiSuccess(methods);
    }

    /// <summary>
    /// Get payment statuses
    /// </summary>
    /// <returns>List of payment statuses</returns>
    [HttpGet("payment-statuses")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductOrderPaymentStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentStatuses()
    {
        var statuses = await _orderService.GetPaymentStatusesAsync();
        return ApiSuccess(statuses);
    }
}