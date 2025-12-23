using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// File upload API controller - uploads to Google Cloud Storage
/// </summary>
[Route("api/[controller]")]
public class FilesController : BaseController
{
    private readonly IGcsStorageService _storageService;
    private readonly ILogger<FilesController> _logger;
    private readonly IConfiguration _configuration;

    public FilesController(
        IGcsStorageService storageService, 
        ILogger<FilesController> logger,
        IConfiguration configuration)
    {
        _storageService = storageService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Upload an image to GCS
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <param name="folder">Target folder path (e.g., "data/article/mainimages", "data/product/mainimages")</param>
    /// <param name="generateVariants">Whether to generate small and thumb variants</param>
    /// <returns>Upload result with URLs</returns>
    [HttpPost("image")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<FileUploadResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB limit
    public async Task<IActionResult> UploadImage(
        IFormFile file, 
        [FromForm] string folder = "data/images",
        [FromForm] bool generateVariants = false)
    {
        if (file == null || file.Length == 0)
        {
            return ApiError("No file provided");
        }

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
        {
            return ApiError("Invalid file type. Only JPEG, PNG, GIF, and WebP images are allowed.");
        }

        // Validate file size (max 10MB)
        var maxSize = _configuration.GetValue<long>("FileUpload:MaxFileSizeInMB", 10) * 1024 * 1024;
        if (file.Length > maxSize)
        {
            return ApiError($"File size exceeds the maximum allowed size of {maxSize / 1024 / 1024}MB");
        }

        using var stream = file.OpenReadStream();
        var result = await _storageService.UploadImageAsync(stream, file.FileName, folder, generateVariants);

        if (!result.Success)
        {
            _logger.LogError("Image upload failed: {Error}", result.Error);
            return ApiError($"Upload failed: {result.Error}");
        }

        _logger.LogInformation("Image uploaded by user {UserId}: {ObjectPath}", CurrentUserId, result.ObjectPath);

        return ApiSuccess(new FileUploadResponse
        {
            Url = result.Url,
            ObjectPath = result.ObjectPath,
            ContentType = result.ContentType,
            Size = result.Size,
            SmallUrl = result.SmallUrl,
            ThumbUrl = result.ThumbUrl
        }, "Image uploaded successfully");
    }

    /// <summary>
    /// Upload a general file to GCS
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">Target folder path</param>
    /// <returns>Upload result with URL</returns>
    [HttpPost("upload")]
    [Authorize]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<FileUploadResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit for general files
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromForm] string folder = "data/files")
    {
        if (file == null || file.Length == 0)
        {
            return ApiError("No file provided");
        }

        // Validate file extension
        var allowedExtensions = _configuration.GetSection("FileUpload:AllowedExtensions").Get<string[]>() 
            ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return ApiError($"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
        }

        using var stream = file.OpenReadStream();
        var result = await _storageService.UploadFileAsync(stream, file.FileName, folder);

        if (!result.Success)
        {
            _logger.LogError("File upload failed: {Error}", result.Error);
            return ApiError($"Upload failed: {result.Error}");
        }

        _logger.LogInformation("File uploaded by user {UserId}: {ObjectPath}", CurrentUserId, result.ObjectPath);

        return ApiSuccess(new FileUploadResponse
        {
            Url = result.Url,
            ObjectPath = result.ObjectPath,
            ContentType = result.ContentType,
            Size = result.Size
        }, "File uploaded successfully");
    }

    /// <summary>
    /// Delete a file from GCS
    /// </summary>
    /// <param name="request">Delete request containing the object path</param>
    /// <returns>Success status</returns>
    [HttpDelete]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
    {
        if (string.IsNullOrEmpty(request.ObjectPath))
        {
            return ApiError("Object path is required");
        }

        var result = await _storageService.DeleteAsync(request.ObjectPath);
        
        if (!result)
        {
            return ApiNotFound("File not found or could not be deleted");
        }

        _logger.LogInformation("File deleted by user {UserId}: {ObjectPath}", CurrentUserId, request.ObjectPath);
        return ApiSuccess(true, "File deleted successfully");
    }

    /// <summary>
    /// Get upload configuration
    /// </summary>
    /// <returns>Upload configuration including allowed file types and size limits</returns>
    [HttpGet("config")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<UploadConfigResponse>), StatusCodes.Status200OK)]
    public IActionResult GetUploadConfig()
    {
        var config = new UploadConfigResponse
        {
            MaxFileSizeInMB = _configuration.GetValue<int>("FileUpload:MaxFileSizeInMB", 10),
            AllowedExtensions = _configuration.GetSection("FileUpload:AllowedExtensions").Get<string[]>() 
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".doc", ".docx", ".xls", ".xlsx" },
            AllowedImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" },
            MaxImagesPerUpload = 10
        };

        return ApiSuccess(config);
    }
}

/// <summary>
/// File upload response DTO
/// </summary>
public class FileUploadResponse
{
    public string? Url { get; set; }
    public string? ObjectPath { get; set; }
    public string? ContentType { get; set; }
    public long Size { get; set; }
    public string? SmallUrl { get; set; }
    public string? ThumbUrl { get; set; }
}

/// <summary>
/// Delete file request DTO
/// </summary>
public class DeleteFileRequest
{
    public string? ObjectPath { get; set; }
}

/// <summary>
/// Upload configuration response DTO
/// </summary>
public class UploadConfigResponse
{
    public int MaxFileSizeInMB { get; set; }
    public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
    public string[] AllowedImageTypes { get; set; } = Array.Empty<string>();
    public int MaxImagesPerUpload { get; set; }
}

