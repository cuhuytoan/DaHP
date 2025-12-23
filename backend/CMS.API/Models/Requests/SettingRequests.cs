using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.Requests;

public class CreateOrUpdateSettingRequest
{
    [Required(ErrorMessage = "Key is required")]
    [StringLength(100, ErrorMessage = "Key cannot exceed 100 characters")]
    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Group cannot exceed 50 characters")]
    public string? Group { get; set; }

    public bool IsPublic { get; set; } = false;
}

public class UpdateWebsiteInfoRequest
{
    [Required(ErrorMessage = "Website name is required")]
    [StringLength(200, ErrorMessage = "Website name cannot exceed 200 characters")]
    public string WebsiteName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public string? Logo { get; set; }

    public string? Favicon { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? ContactPhone { get; set; }

    public string? Address { get; set; }

    [Url(ErrorMessage = "Invalid URL")]
    public string? WebsiteUrl { get; set; }
}

public class UpdateSeoSettingsRequest
{
    [StringLength(200, ErrorMessage = "Default title cannot exceed 200 characters")]
    public string? DefaultTitle { get; set; }

    [StringLength(500, ErrorMessage = "Default description cannot exceed 500 characters")]
    public string? DefaultDescription { get; set; }

    [StringLength(500, ErrorMessage = "Default keywords cannot exceed 500 characters")]
    public string? DefaultKeywords { get; set; }

    public string? DefaultImage { get; set; }

    [StringLength(100, ErrorMessage = "Google Analytics ID cannot exceed 100 characters")]
    public string? GoogleAnalyticsId { get; set; }

    [StringLength(100, ErrorMessage = "Google Tag Manager ID cannot exceed 100 characters")]
    public string? GoogleTagManagerId { get; set; }

    [StringLength(100, ErrorMessage = "Facebook Pixel ID cannot exceed 100 characters")]
    public string? FacebookPixelId { get; set; }

    public string? RobotsText { get; set; }
}

public class UpdateEmailSettingsRequest
{
    [Required(ErrorMessage = "SMTP host is required")]
    [StringLength(200, ErrorMessage = "SMTP host cannot exceed 200 characters")]
    public string SmtpHost { get; set; } = string.Empty;

    [Required(ErrorMessage = "SMTP port is required")]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int SmtpPort { get; set; }

    [Required(ErrorMessage = "From email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string FromEmail { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "From name cannot exceed 200 characters")]
    public string? FromName { get; set; }

    [StringLength(200, ErrorMessage = "Username cannot exceed 200 characters")]
    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool EnableSsl { get; set; } = true;

    public bool UseDefaultCredentials { get; set; } = false;
}

public class TestEmailRequest
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string ToEmail { get; set; } = string.Empty;
}

public class UpdateSocialMediaRequest
{
    [Url(ErrorMessage = "Invalid Facebook URL")]
    public string? FacebookUrl { get; set; }

    [Url(ErrorMessage = "Invalid Twitter URL")]
    public string? TwitterUrl { get; set; }

    [Url(ErrorMessage = "Invalid Instagram URL")]
    public string? InstagramUrl { get; set; }

    [Url(ErrorMessage = "Invalid YouTube URL")]
    public string? YoutubeUrl { get; set; }

    [Url(ErrorMessage = "Invalid LinkedIn URL")]
    public string? LinkedInUrl { get; set; }

    [Url(ErrorMessage = "Invalid TikTok URL")]
    public string? TikTokUrl { get; set; }

    [Url(ErrorMessage = "Invalid Pinterest URL")]
    public string? PinterestUrl { get; set; }
}
