using CMS.API.Models.DTOs;
using CMS.API.Models.Requests;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Newsletter API controller for email subscriptions
/// </summary>
[Route("api/[controller]")]
public class NewsletterController : BaseController
{
    private readonly INewsletterService _newsletterService;
    private readonly ILogger<NewsletterController> _logger;

    public NewsletterController(INewsletterService newsletterService, ILogger<NewsletterController> logger)
    {
        _newsletterService = newsletterService;
        _logger = logger;
    }

    /// <summary>
    /// Subscribe to newsletter (public endpoint)
    /// </summary>
    /// <param name="request">Subscription request with email</param>
    /// <returns>Subscription result</returns>
    [HttpPost("subscribe")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<NewsletterSubscriptionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Subscribe([FromBody] NewsletterSubscribeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Dữ liệu không hợp lệ", GetModelErrors());
        }

        var subscription = await _newsletterService.SubscribeAsync(request.Email);
        return ApiCreated(subscription, message: "Cảm ơn bạn đã đăng ký nhận tin!");
    }

    /// <summary>
    /// Unsubscribe from newsletter (public endpoint)
    /// </summary>
    /// <param name="request">Unsubscription request with email</param>
    /// <returns>Success status</returns>
    [HttpPost("unsubscribe")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unsubscribe([FromBody] NewsletterUnsubscribeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Dữ liệu không hợp lệ", GetModelErrors());
        }

        var result = await _newsletterService.UnsubscribeAsync(request.Email);
        if (!result)
        {
            return ApiNotFound("Email không tồn tại trong danh sách đăng ký");
        }

        return ApiSuccess(true, "Bạn đã hủy đăng ký nhận tin thành công");
    }

    /// <summary>
    /// Check if email is subscribed (public endpoint)
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <returns>Subscription status</returns>
    [HttpGet("check")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckSubscription([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return ApiError("Email là bắt buộc");
        }

        var isSubscribed = await _newsletterService.IsSubscribedAsync(email);
        return ApiSuccess(isSubscribed);
    }

    /// <summary>
    /// Get all subscriptions (admin only)
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of subscriptions</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<NewsletterSubscriptionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var subscriptions = await _newsletterService.GetAllAsync(page, pageSize);
        return ApiSuccess(subscriptions);
    }

    /// <summary>
    /// Get subscriber count (admin only)
    /// </summary>
    /// <returns>Total subscriber count</returns>
    [HttpGet("count")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCount()
    {
        var count = await _newsletterService.GetSubscriberCountAsync();
        return ApiSuccess(count);
    }
}
