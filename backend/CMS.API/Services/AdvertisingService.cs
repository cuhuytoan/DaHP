using CMS.API.Data;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

/// <summary>
/// Advertising service implementation
/// </summary>
public class AdvertisingService : IAdvertisingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdvertisingService> _logger;

    public AdvertisingService(ApplicationDbContext context, ILogger<AdvertisingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<AdvertisingDto>> GetByBlockIdAsync(int blockId)
    {
        var now = DateTime.UtcNow;

        var advertisings = await _context.Advertisings
            .AsNoTracking()
            .Where(a => a.AdvertisingBlockId == blockId
                && a.Active == true
                && (a.StartDate == null || a.StartDate <= now)
                && (a.EndDate == null || a.EndDate >= now))
            .OrderBy(a => a.Sort)
            .Select(a => new AdvertisingDto
            {
                Id = a.Id,
                AdvertisingBlockId = a.AdvertisingBlockId,
                AdvertisingTypeId = a.AdvertisingTypeId,
                Name = a.Name,
                Image = a.Image,
                Url = a.Url,
                Description = a.Description,
                Content = a.Content,
                Sort = a.Sort
            })
            .ToListAsync();

        return advertisings;
    }

    /// <inheritdoc />
    public async Task<List<AdvertisingBlockDto>> GetAllBlocksAsync()
    {
        var blocks = await _context.AdvertisingBlocks
            .AsNoTracking()
            .Where(b => b.Active == true)
            .OrderBy(b => b.Sort)
            .Select(b => new AdvertisingBlockDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Width = b.Width,
                Height = b.Height,
                Sort = b.Sort
            })
            .ToListAsync();

        return blocks;
    }

    /// <inheritdoc />
    public async Task<AdvertisingBlockDto?> GetBlockByIdAsync(int blockId)
    {
        var now = DateTime.UtcNow;

        var block = await _context.AdvertisingBlocks
            .AsNoTracking()
            .Where(b => b.Id == blockId && b.Active == true)
            .Select(b => new AdvertisingBlockDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Width = b.Width,
                Height = b.Height,
                Sort = b.Sort,
                Advertisings = b.Advertisings
                    .Where(a => a.Active == true
                        && (a.StartDate == null || a.StartDate <= now)
                        && (a.EndDate == null || a.EndDate >= now))
                    .OrderBy(a => a.Sort)
                    .Select(a => new AdvertisingDto
                    {
                        Id = a.Id,
                        AdvertisingBlockId = a.AdvertisingBlockId,
                        AdvertisingTypeId = a.AdvertisingTypeId,
                        Name = a.Name,
                        Image = a.Image,
                        Url = a.Url,
                        Description = a.Description,
                        Content = a.Content,
                        Sort = a.Sort
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return block;
    }

    /// <inheritdoc />
    public async Task<AdvertisingDto?> GetByIdAsync(long id)
    {
        var advertising = await _context.Advertisings
            .AsNoTracking()
            .Where(a => a.Id == id && a.Active == true)
            .Select(a => new AdvertisingDto
            {
                Id = a.Id,
                AdvertisingBlockId = a.AdvertisingBlockId,
                AdvertisingTypeId = a.AdvertisingTypeId,
                Name = a.Name,
                Image = a.Image,
                Url = a.Url,
                Description = a.Description,
                Content = a.Content,
                Sort = a.Sort
            })
            .FirstOrDefaultAsync();

        return advertising;
    }

    /// <inheritdoc />
    public async Task<bool> TrackClickAsync(long id)
    {
        var advertising = await _context.Advertisings.FindAsync(id);
        if (advertising == null)
        {
            return false;
        }

        advertising.Counter = (advertising.Counter ?? 0) + 1;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Advertising {Id} clicked. Total clicks: {Counter}", id, advertising.Counter);

        return true;
    }
}
