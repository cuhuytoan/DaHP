using System.Reflection;
using System.Text.Json;
using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingService> _logger;
    private static readonly Dictionary<string, PropertyInfo> _settingProperties;

    static SettingService()
    {
        // Cache property info for reflection
        _settingProperties = typeof(Setting)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToDictionary(p => p.Name.ToLowerInvariant(), p => p);
    }

    public SettingService(ApplicationDbContext context, ILogger<SettingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string?> GetValueAsync(string key)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync();
        if (setting == null) return null;

        var normalizedKey = key.ToLowerInvariant();
        if (_settingProperties.TryGetValue(normalizedKey, out var property))
        {
            var value = property.GetValue(setting);
            return value?.ToString();
        }

        return null;
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var stringValue = await GetValueAsync(key);
        if (string.IsNullOrEmpty(stringValue)) return default;

        try
        {
            var type = typeof(T);
            if (type == typeof(string)) return (T)(object)stringValue;
            if (type == typeof(int)) return (T)(object)int.Parse(stringValue);
            if (type == typeof(bool)) return (T)(object)bool.Parse(stringValue);
            if (type == typeof(decimal)) return (T)(object)decimal.Parse(stringValue);
            if (type == typeof(double)) return (T)(object)double.Parse(stringValue);
            if (type == typeof(DateTime)) return (T)(object)DateTime.Parse(stringValue);

            // Try JSON deserialization for complex types
            return JsonSerializer.Deserialize<T>(stringValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse setting value for key {Key}", key);
            return default;
        }
    }

    public async Task<bool> SetValueAsync(string key, string value, string userId)
    {
        try
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            if (setting == null)
            {
                // Create default setting if not exists
                setting = new Setting { Id = 1 };
                await _context.Settings.AddAsync(setting);
            }

            var normalizedKey = key.ToLowerInvariant();
            if (!_settingProperties.TryGetValue(normalizedKey, out var property))
            {
                _logger.LogWarning("Setting property {Key} not found", key);
                return false;
            }

            // Convert value to appropriate type
            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            object? convertedValue = underlyingType switch
            {
                Type t when t == typeof(string) => value,
                Type t when t == typeof(int) => int.TryParse(value, out var i) ? i : null,
                Type t when t == typeof(bool) => bool.TryParse(value, out var b) ? b : null,
                Type t when t == typeof(decimal) => decimal.TryParse(value, out var d) ? d : null,
                Type t when t == typeof(double) => double.TryParse(value, out var db) ? db : null,
                Type t when t == typeof(DateTime) => DateTime.TryParse(value, out var dt) ? dt : null,
                _ => value
            };

            property.SetValue(setting, convertedValue);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Setting {Key} updated by user {UserId}", key, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set setting value for key {Key}", key);
            return false;
        }
    }

    public async Task<Dictionary<string, string>> GetAllAsync()
    {
        var setting = await _context.Settings.FirstOrDefaultAsync();
        if (setting == null) return new Dictionary<string, string>();

        var result = new Dictionary<string, string>();
        foreach (var kvp in _settingProperties)
        {
            var value = kvp.Value.GetValue(setting);
            if (value != null)
            {
                result[kvp.Key] = value.ToString() ?? "";
            }
        }

        return result;
    }

    public async Task<Dictionary<string, string>> GetByGroupAsync(string groupName)
    {
        var allSettings = await GetAllAsync();
        var normalizedGroup = groupName.ToLowerInvariant();

        // Group settings by prefix
        var groupMappings = new Dictionary<string, List<string>>
        {
            ["general"] = new() { "id", "domain", "websitename", "adminname", "telephone", "appstatus", "counter", "defaultlanguage_id", "defaultskin_id" },
            ["email"] = new() { "emailsupport", "emailorder", "emailsendersmtp", "emailsenderport", "emailsenderssl", "emailsender", "emailsenderpassword" },
            ["seo"] = new() { "metadescriptiondefault", "metakeywordsdefault", "metatitledefault" },
            ["analytics"] = new() { "googleanalyticscode", "othercode" },
            ["social"] = new() { "facebookpageid", "facebookappid" }
        };

        if (!groupMappings.TryGetValue(normalizedGroup, out var keys))
        {
            return new Dictionary<string, string>();
        }

        return allSettings
            .Where(kvp => keys.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Get full setting entity
    /// </summary>
    public async Task<Setting?> GetSettingAsync()
    {
        return await _context.Settings.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Update full setting entity
    /// </summary>
    public async Task<bool> UpdateSettingAsync(Setting setting, string userId)
    {
        try
        {
            var existing = await _context.Settings.FirstOrDefaultAsync();
            if (existing == null)
            {
                await _context.Settings.AddAsync(setting);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(setting);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Settings updated by user {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update settings");
            return false;
        }
    }
}
