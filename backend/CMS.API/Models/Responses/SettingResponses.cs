namespace CMS.API.Models.Responses;

public class SettingResponse
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Description { get; set; }
    public string? Group { get; set; }
    public bool IsPublic { get; set; }
}

public class WebsiteInfoResponse
{
    public string WebsiteName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? Favicon { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? WebsiteUrl { get; set; }
}

public class SeoSettingsResponse
{
    public string? DefaultTitle { get; set; }
    public string? DefaultDescription { get; set; }
    public string? DefaultKeywords { get; set; }
    public string? DefaultImage { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? GoogleTagManagerId { get; set; }
    public string? FacebookPixelId { get; set; }
    public string? RobotsText { get; set; }
}

public class EmailSettingsResponse
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string? FromName { get; set; }
    public string? Username { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
}

public class SocialMediaResponse
{
    public string? FacebookUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TikTokUrl { get; set; }
    public string? PinterestUrl { get; set; }
}

public class SystemStatsResponse
{
    public int TotalArticles { get; set; }
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalUsers { get; set; }
    public int TotalComments { get; set; }
    public int PendingComments { get; set; }
    public int PendingReviews { get; set; }
    public int PendingOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MonthRevenue { get; set; }
    public int TodayVisits { get; set; }
    public int TotalVisits { get; set; }
    public string ServerTime { get; set; } = string.Empty;
    public string DatabaseSize { get; set; } = string.Empty;
}
