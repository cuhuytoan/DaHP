using CMS.API.Data;
using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Controllers;

/// <summary>
/// Settings API controller for managing system settings
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SettingsController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(ApplicationDbContext context, ILogger<SettingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all settings
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<SettingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var settings = await _context.Set<Setting>()
            .Select(s => new SettingDto
            {
                Id = s.Id,
                Domain = s.Domain,
                WebsiteName = s.WebsiteName,
                AdminName = s.AdminName,
                EmailSupport = s.EmailSupport,
                EmailOrder = s.EmailOrder,
                EmailSenderSmtp = s.EmailSenderSmtp,
                EmailSenderPort = s.EmailSenderPort,
                EmailSenderSsl = s.EmailSenderSsl,
                EmailSender = s.EmailSender,
                Telephone = s.Telephone,
                AppStatus = s.AppStatus,
                Counter = s.Counter,
                DefaultLanguageId = s.DefaultLanguageId,
                DefaultSkinId = s.DefaultSkinId,
                MetaDescriptionDefault = s.MetaDescriptionDefault,
                MetaKeywordsDefault = s.MetaKeywordsDefault,
                MetaTitleDefault = s.MetaTitleDefault,
                GoogleAnalyticsCode = s.GoogleAnalyticsCode,
                OtherCode = s.OtherCode,
                FacebookPageId = s.FacebookPageId,
                FacebookAppId = s.FacebookAppId
            })
            .ToListAsync();

        return ApiSuccess(settings);
    }

    /// <summary>
    /// Get setting by ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<SettingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var setting = await _context.Set<Setting>()
            .Where(s => s.Id == id)
            .Select(s => new SettingDto
            {
                Id = s.Id,
                Domain = s.Domain,
                WebsiteName = s.WebsiteName,
                AdminName = s.AdminName,
                EmailSupport = s.EmailSupport,
                EmailOrder = s.EmailOrder,
                EmailSenderSmtp = s.EmailSenderSmtp,
                EmailSenderPort = s.EmailSenderPort,
                EmailSenderSsl = s.EmailSenderSsl,
                EmailSender = s.EmailSender,
                Telephone = s.Telephone,
                AppStatus = s.AppStatus,
                Counter = s.Counter,
                DefaultLanguageId = s.DefaultLanguageId,
                DefaultSkinId = s.DefaultSkinId,
                MetaDescriptionDefault = s.MetaDescriptionDefault,
                MetaKeywordsDefault = s.MetaKeywordsDefault,
                MetaTitleDefault = s.MetaTitleDefault,
                GoogleAnalyticsCode = s.GoogleAnalyticsCode,
                OtherCode = s.OtherCode,
                FacebookPageId = s.FacebookPageId,
                FacebookAppId = s.FacebookAppId
            })
            .FirstOrDefaultAsync();

        if (setting == null)
            return ApiNotFound("Setting not found");

        return ApiSuccess(setting);
    }

    /// <summary>
    /// Update setting
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<SettingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSettingRequest request)
    {
        if (!ModelState.IsValid)
            return ApiError("Invalid request", GetModelErrors());

        var setting = await _context.Set<Setting>().FindAsync(id);
        if (setting == null)
            return ApiNotFound("Setting not found");

        // Update only non-null properties
        if (request.Domain != null) setting.Domain = request.Domain;
        if (request.WebsiteName != null) setting.WebsiteName = request.WebsiteName;
        if (request.AdminName != null) setting.AdminName = request.AdminName;
        if (request.EmailSupport != null) setting.EmailSupport = request.EmailSupport;
        if (request.EmailOrder != null) setting.EmailOrder = request.EmailOrder;
        if (request.EmailSenderSmtp != null) setting.EmailSenderSmtp = request.EmailSenderSmtp;
        if (request.EmailSenderPort != null) setting.EmailSenderPort = request.EmailSenderPort;
        if (request.EmailSenderSsl.HasValue) setting.EmailSenderSsl = request.EmailSenderSsl;
        if (request.EmailSender != null) setting.EmailSender = request.EmailSender;
        if (request.EmailSenderPassword != null) setting.EmailSenderPassword = request.EmailSenderPassword;
        if (request.Telephone != null) setting.Telephone = request.Telephone;
        if (request.AppStatus.HasValue) setting.AppStatus = request.AppStatus.Value;
        if (request.DefaultLanguageId.HasValue) setting.DefaultLanguageId = request.DefaultLanguageId.Value;
        if (request.DefaultSkinId.HasValue) setting.DefaultSkinId = request.DefaultSkinId.Value;
        if (request.MetaDescriptionDefault != null) setting.MetaDescriptionDefault = request.MetaDescriptionDefault;
        if (request.MetaKeywordsDefault != null) setting.MetaKeywordsDefault = request.MetaKeywordsDefault;
        if (request.MetaTitleDefault != null) setting.MetaTitleDefault = request.MetaTitleDefault;
        if (request.GoogleAnalyticsCode != null) setting.GoogleAnalyticsCode = request.GoogleAnalyticsCode;
        if (request.OtherCode != null) setting.OtherCode = request.OtherCode;
        if (request.FacebookPageId != null) setting.FacebookPageId = request.FacebookPageId;
        if (request.FacebookAppId != null) setting.FacebookAppId = request.FacebookAppId;

        await _context.SaveChangesAsync();

        return ApiSuccess(new SettingDto
        {
            Id = setting.Id,
            Domain = setting.Domain,
            WebsiteName = setting.WebsiteName,
            AdminName = setting.AdminName,
            EmailSupport = setting.EmailSupport,
            EmailOrder = setting.EmailOrder,
            EmailSenderSmtp = setting.EmailSenderSmtp,
            EmailSenderPort = setting.EmailSenderPort,
            EmailSenderSsl = setting.EmailSenderSsl,
            EmailSender = setting.EmailSender,
            Telephone = setting.Telephone,
            AppStatus = setting.AppStatus,
            Counter = setting.Counter,
            DefaultLanguageId = setting.DefaultLanguageId,
            DefaultSkinId = setting.DefaultSkinId,
            MetaDescriptionDefault = setting.MetaDescriptionDefault,
            MetaKeywordsDefault = setting.MetaKeywordsDefault,
            MetaTitleDefault = setting.MetaTitleDefault,
            GoogleAnalyticsCode = setting.GoogleAnalyticsCode,
            OtherCode = setting.OtherCode,
            FacebookPageId = setting.FacebookPageId,
            FacebookAppId = setting.FacebookAppId
        }, "Setting updated successfully");
    }
}

public class SettingDto
{
    public long Id { get; set; }
    public string? Domain { get; set; }
    public string? WebsiteName { get; set; }
    public string? AdminName { get; set; }
    public string? EmailSupport { get; set; }
    public string? EmailOrder { get; set; }
    public string? EmailSenderSmtp { get; set; }
    public string? EmailSenderPort { get; set; }
    public bool? EmailSenderSsl { get; set; }
    public string? EmailSender { get; set; }
    public string? Telephone { get; set; }
    public bool AppStatus { get; set; }
    public int? Counter { get; set; }
    public long DefaultLanguageId { get; set; }
    public long DefaultSkinId { get; set; }
    public string? MetaDescriptionDefault { get; set; }
    public string? MetaKeywordsDefault { get; set; }
    public string? MetaTitleDefault { get; set; }
    public string? GoogleAnalyticsCode { get; set; }
    public string? OtherCode { get; set; }
    public string? FacebookPageId { get; set; }
    public string? FacebookAppId { get; set; }
}

public class UpdateSettingRequest
{
    public string? Domain { get; set; }
    public string? WebsiteName { get; set; }
    public string? AdminName { get; set; }
    public string? EmailSupport { get; set; }
    public string? EmailOrder { get; set; }
    public string? EmailSenderSmtp { get; set; }
    public string? EmailSenderPort { get; set; }
    public bool? EmailSenderSsl { get; set; }
    public string? EmailSender { get; set; }
    public string? EmailSenderPassword { get; set; }
    public string? Telephone { get; set; }
    public bool? AppStatus { get; set; }
    public long? DefaultLanguageId { get; set; }
    public long? DefaultSkinId { get; set; }
    public string? MetaDescriptionDefault { get; set; }
    public string? MetaKeywordsDefault { get; set; }
    public string? MetaTitleDefault { get; set; }
    public string? GoogleAnalyticsCode { get; set; }
    public string? OtherCode { get; set; }
    public string? FacebookPageId { get; set; }
    public string? FacebookAppId { get; set; }
}
