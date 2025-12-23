using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

/// <summary>
/// Newsletter service implementation
/// </summary>
public class NewsletterService : INewsletterService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NewsletterService> _logger;

    public NewsletterService(ApplicationDbContext context, ILogger<NewsletterService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<NewsletterSubscriptionDto> SubscribeAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        // Check if email already exists
        var existing = await _context.NewsletterSubscriptions
            .FirstOrDefaultAsync(n => n.Email.ToLower() == normalizedEmail);

        if (existing != null)
        {
            // Reactivate if previously unsubscribed
            if (!existing.Active)
            {
                existing.Active = true;
                existing.UnsubscribeDate = null;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Newsletter subscription reactivated for {Email}", normalizedEmail);
            }

            return new NewsletterSubscriptionDto
            {
                Id = existing.Id,
                Email = existing.Email,
                Active = existing.Active,
                CreateDate = existing.CreateDate,
                UnsubscribeDate = existing.UnsubscribeDate
            };
        }

        // Create new subscription
        var subscription = new NewsletterSubscription
        {
            Email = normalizedEmail,
            Active = true,
            CreateDate = DateTime.UtcNow
        };

        _context.NewsletterSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New newsletter subscription from {Email}", normalizedEmail);

        return new NewsletterSubscriptionDto
        {
            Id = subscription.Id,
            Email = subscription.Email,
            Active = subscription.Active,
            CreateDate = subscription.CreateDate
        };
    }

    /// <inheritdoc />
    public async Task<bool> UnsubscribeAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var subscription = await _context.NewsletterSubscriptions
            .FirstOrDefaultAsync(n => n.Email.ToLower() == normalizedEmail && n.Active);

        if (subscription == null)
        {
            return false;
        }

        subscription.Active = false;
        subscription.UnsubscribeDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Newsletter unsubscription for {Email}", normalizedEmail);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> IsSubscribedAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _context.NewsletterSubscriptions
            .AnyAsync(n => n.Email.ToLower() == normalizedEmail && n.Active);
    }

    /// <inheritdoc />
    public async Task<List<NewsletterSubscriptionDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var subscriptions = await _context.NewsletterSubscriptions
            .AsNoTracking()
            .Where(n => n.Active)
            .OrderByDescending(n => n.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NewsletterSubscriptionDto
            {
                Id = n.Id,
                Email = n.Email,
                Active = n.Active,
                CreateDate = n.CreateDate,
                UnsubscribeDate = n.UnsubscribeDate
            })
            .ToListAsync();

        return subscriptions;
    }

    /// <inheritdoc />
    public async Task<int> GetSubscriberCountAsync()
    {
        return await _context.NewsletterSubscriptions
            .CountAsync(n => n.Active);
    }
}
