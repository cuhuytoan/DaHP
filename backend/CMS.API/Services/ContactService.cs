using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Models.Requests;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Services;

/// <summary>
/// Contact service implementation
/// </summary>
public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ContactDto> SubmitAsync(ContactRequest request)
    {
        var contact = new Contact
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Descriptions = request.Descriptions,
            CreateDate = DateTime.UtcNow
        };

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New contact submitted from {Email}", request.Email);

        return new ContactDto
        {
            Id = contact.Id,
            FullName = contact.FullName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber,
            Descriptions = contact.Descriptions,
            CreateDate = contact.CreateDate
        };
    }

    /// <inheritdoc />
    public async Task<ContactDto?> GetByIdAsync(long id)
    {
        var contact = await _context.Contacts
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ContactDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Descriptions = c.Descriptions,
                CreateDate = c.CreateDate
            })
            .FirstOrDefaultAsync();

        return contact;
    }

    /// <inheritdoc />
    public async Task<List<ContactDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var contacts = await _context.Contacts
            .AsNoTracking()
            .OrderByDescending(c => c.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ContactDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Descriptions = c.Descriptions,
                CreateDate = c.CreateDate
            })
            .ToListAsync();

        return contacts;
    }
}
