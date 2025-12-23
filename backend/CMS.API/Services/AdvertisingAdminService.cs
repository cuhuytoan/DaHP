using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Responses;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

public class AdvertisingAdminService : IAdvertisingAdminService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdvertisingAdminService> _logger;

    public AdvertisingAdminService(ApplicationDbContext context, ILogger<AdvertisingAdminService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== Advertising Block Operations =====================

    public async Task<PagedResult<AdvertisingBlockListDto>> GetBlocksAsync(
        int page = 1,
        int pageSize = 20,
        string? keyword = null,
        bool? active = null)
    {
        var query = _context.AdvertisingBlocks
            .Include(b => b.Advertisings)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(b => b.Name != null && b.Name.Contains(keyword));
        }

        if (active.HasValue)
        {
            query = query.Where(b => b.Active == active);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Sort)
            .ThenByDescending(b => b.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new AdvertisingBlockListDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Width = b.Width,
                Height = b.Height,
                Sort = b.Sort,
                Active = b.Active,
                AdvertisingCount = b.Advertisings.Count,
                CreateDate = b.CreateDate
            })
            .ToListAsync();

        return new PagedResult<AdvertisingBlockListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<AdvertisingBlockDetailDto?> GetBlockDetailByIdAsync(long id)
    {
        var block = await _context.AdvertisingBlocks
            .Include(b => b.Advertisings)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return null;

        return new AdvertisingBlockDetailDto
        {
            Id = block.Id,
            Name = block.Name,
            Description = block.Description,
            Width = block.Width,
            Height = block.Height,
            Sort = block.Sort,
            Active = block.Active,
            CanDelete = block.CanDelete,
            CreateBy = block.CreateBy,
            CreateDate = block.CreateDate,
            LastEditedBy = block.LastEditedBy,
            LastEditedDate = block.LastEditedDate,
            Advertisings = block.Advertisings
                .OrderBy(a => a.Sort)
                .Select(a => new AdvertisingListDto
                {
                    Id = a.Id,
                    AdvertisingBlockId = a.AdvertisingBlockId,
                    AdvertisingTypeId = a.AdvertisingTypeId,
                    Name = a.Name,
                    Image = a.Image,
                    Url = a.Url,
                    Sort = a.Sort,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Active = a.Active,
                    Counter = a.Counter,
                    CreateDate = a.CreateDate
                })
                .ToList()
        };
    }

    public async Task<AdvertisingBlockDetailDto> CreateBlockAsync(CreateAdvertisingBlockRequest request, string userId)
    {
        var block = new AdvertisingBlock
        {
            Name = request.Name,
            Description = request.Description,
            Width = request.Width,
            Height = request.Height,
            Sort = request.Sort,
            Active = request.Active,
            CanDelete = true,
            CreateBy = userId,
            CreateDate = DateTime.Now
        };

        _context.AdvertisingBlocks.Add(block);
        await _context.SaveChangesAsync();

        return (await GetBlockDetailByIdAsync(block.Id))!;
    }

    public async Task<AdvertisingBlockDetailDto?> UpdateBlockAsync(long id, UpdateAdvertisingBlockRequest request, string userId)
    {
        var block = await _context.AdvertisingBlocks.FindAsync(id);
        if (block == null) return null;

        block.Name = request.Name;
        block.Description = request.Description;
        block.Width = request.Width;
        block.Height = request.Height;
        block.Sort = request.Sort;
        block.Active = request.Active;
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetBlockDetailByIdAsync(id);
    }

    public async Task<bool> DeleteBlockAsync(long id)
    {
        var block = await _context.AdvertisingBlocks
            .Include(b => b.Advertisings)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (block == null) return false;
        if (block.CanDelete == false) return false;

        _context.Advertisings.RemoveRange(block.Advertisings);
        _context.AdvertisingBlocks.Remove(block);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleBlockStatusAsync(long id, string userId)
    {
        var block = await _context.AdvertisingBlocks.FindAsync(id);
        if (block == null) return false;

        block.Active = !(block.Active ?? false);
        block.LastEditedBy = userId;
        block.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Advertising Operations =====================

    public async Task<PagedResult<AdvertisingListDto>> GetAdvertisingsAsync(
        int page = 1,
        int pageSize = 20,
        int? blockId = null,
        int? typeId = null,
        string? keyword = null,
        bool? active = null)
    {
        var query = _context.Advertisings
            .Include(a => a.AdvertisingBlock)
            .Include(a => a.AdvertisingType)
            .AsQueryable();

        if (blockId.HasValue)
        {
            query = query.Where(a => a.AdvertisingBlockId == blockId);
        }

        if (typeId.HasValue)
        {
            query = query.Where(a => a.AdvertisingTypeId == typeId);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(a => a.Name != null && a.Name.Contains(keyword));
        }

        if (active.HasValue)
        {
            query = query.Where(a => a.Active == active);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Sort)
            .ThenByDescending(a => a.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AdvertisingListDto
            {
                Id = a.Id,
                AdvertisingBlockId = a.AdvertisingBlockId,
                BlockName = a.AdvertisingBlock != null ? a.AdvertisingBlock.Name : null,
                AdvertisingTypeId = a.AdvertisingTypeId,
                TypeName = a.AdvertisingType != null ? a.AdvertisingType.Name : null,
                Name = a.Name,
                Image = a.Image,
                Url = a.Url,
                Sort = a.Sort,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                Active = a.Active,
                Counter = a.Counter,
                CreateDate = a.CreateDate
            })
            .ToListAsync();

        return new PagedResult<AdvertisingListDto>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<AdvertisingDetailDto?> GetAdvertisingByIdAsync(long id)
    {
        var ad = await _context.Advertisings
            .Include(a => a.AdvertisingBlock)
            .Include(a => a.AdvertisingType)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null) return null;

        return new AdvertisingDetailDto
        {
            Id = ad.Id,
            AdvertisingBlockId = ad.AdvertisingBlockId,
            BlockName = ad.AdvertisingBlock?.Name,
            AdvertisingTypeId = ad.AdvertisingTypeId,
            TypeName = ad.AdvertisingType?.Name,
            Name = ad.Name,
            Image = ad.Image,
            Url = ad.Url,
            Description = ad.Description,
            Content = ad.Content,
            Sort = ad.Sort,
            StartDate = ad.StartDate,
            EndDate = ad.EndDate,
            Active = ad.Active,
            Counter = ad.Counter,
            CanDelete = ad.CanDelete,
            CreateBy = ad.CreateBy,
            CreateDate = ad.CreateDate,
            LastEditedBy = ad.LastEditedBy,
            LastEditedDate = ad.LastEditedDate
        };
    }

    public async Task<AdvertisingDetailDto> CreateAdvertisingAsync(CreateAdvertisingRequest request, string userId)
    {
        var ad = new Advertising
        {
            AdvertisingBlockId = request.AdvertisingBlockId,
            AdvertisingTypeId = request.AdvertisingTypeId,
            Name = request.Name,
            Image = request.Image,
            Url = request.Url,
            Description = request.Description,
            Content = request.Content,
            Sort = request.Sort,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Active = request.Active,
            Counter = 0,
            CanDelete = true,
            CreateBy = userId,
            CreateDate = DateTime.Now
        };

        _context.Advertisings.Add(ad);
        await _context.SaveChangesAsync();

        return (await GetAdvertisingByIdAsync(ad.Id))!;
    }

    public async Task<AdvertisingDetailDto?> UpdateAdvertisingAsync(long id, UpdateAdvertisingRequest request, string userId)
    {
        var ad = await _context.Advertisings.FindAsync(id);
        if (ad == null) return null;

        ad.AdvertisingBlockId = request.AdvertisingBlockId;
        ad.AdvertisingTypeId = request.AdvertisingTypeId;
        ad.Name = request.Name;
        ad.Image = request.Image;
        ad.Url = request.Url;
        ad.Description = request.Description;
        ad.Content = request.Content;
        ad.Sort = request.Sort;
        ad.StartDate = request.StartDate;
        ad.EndDate = request.EndDate;
        ad.Active = request.Active;
        ad.LastEditedBy = userId;
        ad.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetAdvertisingByIdAsync(id);
    }

    public async Task<bool> DeleteAdvertisingAsync(long id)
    {
        var ad = await _context.Advertisings.FindAsync(id);
        if (ad == null) return false;
        if (ad.CanDelete == false) return false;

        _context.Advertisings.Remove(ad);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAdvertisingStatusAsync(long id, string userId)
    {
        var ad = await _context.Advertisings.FindAsync(id);
        if (ad == null) return false;

        ad.Active = !(ad.Active ?? false);
        ad.LastEditedBy = userId;
        ad.LastEditedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateSortOrderAsync(long blockId, List<long> advertisingIds, string userId)
    {
        var ads = await _context.Advertisings
            .Where(a => a.AdvertisingBlockId == (int)blockId && advertisingIds.Contains(a.Id))
            .ToListAsync();

        for (int i = 0; i < advertisingIds.Count; i++)
        {
            var ad = ads.FirstOrDefault(a => a.Id == advertisingIds[i]);
            if (ad != null)
            {
                ad.Sort = i + 1;
                ad.LastEditedBy = userId;
                ad.LastEditedDate = DateTime.Now;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== Advertising Type Operations =====================

    public async Task<List<AdvertisingTypeDto>> GetTypesAsync()
    {
        return await _context.AdvertisingTypes
            .Select(t => new AdvertisingTypeDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
    }
}
