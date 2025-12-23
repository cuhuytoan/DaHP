using CMS.API.Models.DTOs;
using CMS.API.Models.Requests;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Newsletter service interface for email subscriptions
/// </summary>
public interface INewsletterService
{
    /// <summary>
    /// Subscribe an email to the newsletter
    /// </summary>
    Task<NewsletterSubscriptionDto> SubscribeAsync(string email);

    /// <summary>
    /// Unsubscribe an email from the newsletter
    /// </summary>
    Task<bool> UnsubscribeAsync(string email);

    /// <summary>
    /// Check if an email is subscribed
    /// </summary>
    Task<bool> IsSubscribedAsync(string email);

    /// <summary>
    /// Get all active subscriptions (for admin)
    /// </summary>
    Task<List<NewsletterSubscriptionDto>> GetAllAsync(int page = 1, int pageSize = 50);

    /// <summary>
    /// Get total subscriber count
    /// </summary>
    Task<int> GetSubscriberCountAsync();
}
