using CMS.API.Services.Interfaces;

namespace CMS.API.Services;

public interface IFileService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder = "images");
    Task<bool> DeleteFileAsync(string filePath);
    string GetFileUrl(string filePath);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;

    public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder = "images")
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", folder);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        _logger.LogInformation("File uploaded: {FilePath}", filePath);

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_environment.WebRootPath ?? "wwwroot", filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("File deleted: {FilePath}", fullPath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            return false;
        }
    }

    public string GetFileUrl(string filePath)
    {
        return filePath;
    }
}
