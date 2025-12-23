using CMS.API.Services.Interfaces;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace CMS.API.Services;

/// <summary>
/// Google Cloud Storage service implementation
/// </summary>
public class GcsStorageService : IGcsStorageService
{
    private readonly StorageClient _storageClient;
    private readonly ILogger<GcsStorageService> _logger;
    private readonly string _bucketName;
    private readonly string _baseUrl;

    public GcsStorageService(IConfiguration configuration, ILogger<GcsStorageService> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _bucketName = configuration["GcsStorage:BucketName"] ?? "historicalmemorydev";
        _baseUrl = $"https://storage.googleapis.com/{_bucketName}";

        // Initialize GCS client
        // In production, use GOOGLE_APPLICATION_CREDENTIALS env var
        // In local development, use the uploader-key.json file
        var credentialPath = configuration["GcsStorage:CredentialPath"];
        
        if (string.IsNullOrEmpty(credentialPath))
        {
            // Try to find the key file in the application directory
            var localKeyPath = Path.Combine(environment.ContentRootPath, "uploader-key.json");
            if (File.Exists(localKeyPath))
            {
                credentialPath = localKeyPath;
            }
        }

        if (!string.IsNullOrEmpty(credentialPath) && File.Exists(credentialPath))
        {
            var credential = GoogleCredential.FromFile(credentialPath);
            _storageClient = StorageClient.Create(credential);
            _logger.LogInformation("GCS client initialized with credential file: {Path}", credentialPath);
        }
        else
        {
            // Use default credentials (from environment variable or compute engine)
            _storageClient = StorageClient.Create();
            _logger.LogInformation("GCS client initialized with default credentials");
        }
    }

    public async Task<GcsUploadResult> UploadImageAsync(Stream fileStream, string fileName, string folder, bool generateVariants = false)
    {
        try
        {
            var contentType = GetContentType(fileName);
            if (!contentType.StartsWith("image/"))
            {
                return new GcsUploadResult { Success = false, Error = "File is not an image" };
            }

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{SanitizeFileName(fileName)}";
            var objectPath = $"{folder}/original/{uniqueFileName}";

            // Read the image into memory for processing
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var originalBytes = memoryStream.ToArray();

            // Upload original image
            memoryStream.Position = 0;
            var uploadedObject = await _storageClient.UploadObjectAsync(
                _bucketName,
                objectPath,
                contentType,
                memoryStream
            );

            var result = new GcsUploadResult
            {
                Success = true,
                Bucket = _bucketName,
                Url = GetPublicUrl(objectPath),
                ObjectPath = objectPath,
                ContentType = contentType,
                Size = originalBytes.Length
            };

            // Generate variants if requested
            if (generateVariants)
            {
                // Small variant (500x500 max)
                var smallPath = $"{folder}/small/{uniqueFileName}";
                var smallBytes = await ResizeImageAsync(originalBytes, 500, 500);
                if (smallBytes != null)
                {
                    using var smallStream = new MemoryStream(smallBytes);
                    await _storageClient.UploadObjectAsync(_bucketName, smallPath, contentType, smallStream);
                    result.SmallUrl = GetPublicUrl(smallPath);
                }

                // Thumb variant (120x120 max)
                var thumbPath = $"{folder}/thumb/{uniqueFileName}";
                var thumbBytes = await ResizeImageAsync(originalBytes, 120, 120);
                if (thumbBytes != null)
                {
                    using var thumbStream = new MemoryStream(thumbBytes);
                    await _storageClient.UploadObjectAsync(_bucketName, thumbPath, contentType, thumbStream);
                    result.ThumbUrl = GetPublicUrl(thumbPath);
                }
            }

            _logger.LogInformation("Image uploaded to GCS: {ObjectPath}", objectPath);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload image to GCS");
            return new GcsUploadResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<GcsUploadResult> UploadFileAsync(Stream fileStream, string fileName, string folder)
    {
        try
        {
            var contentType = GetContentType(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{SanitizeFileName(fileName)}";
            var objectPath = $"{folder}/{uniqueFileName}";

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var uploadedObject = await _storageClient.UploadObjectAsync(
                _bucketName,
                objectPath,
                contentType,
                memoryStream
            );

            _logger.LogInformation("File uploaded to GCS: {ObjectPath}", objectPath);

            return new GcsUploadResult
            {
                Success = true,
                Bucket = _bucketName,
                Url = GetPublicUrl(objectPath),
                ObjectPath = objectPath,
                ContentType = contentType,
                Size = memoryStream.Length
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file to GCS");
            return new GcsUploadResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<bool> DeleteAsync(string objectPath, CancellationToken cancellationToken = default)
    {
        try
        {
            await _storageClient.DeleteObjectAsync(_bucketName, objectPath, cancellationToken: cancellationToken);
            _logger.LogInformation("Object deleted from GCS: {ObjectPath}", objectPath);
            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning("Object not found in GCS: {ObjectPath}", objectPath);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete object from GCS: {ObjectPath}", objectPath);
            return false;
        }
    }

    public async Task<bool> ExistsAsync(string objectPath)
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(_bucketName, objectPath);
            return obj != null;
        }
        catch
        {
            return false;
        }
    }

    public string GetPublicUrl(string objectPath)
    {
        return $"{_baseUrl}/{objectPath}";
    }

    private async Task<byte[]?> ResizeImageAsync(byte[] imageBytes, int maxWidth, int maxHeight)
    {
        try
        {
            using var image = Image.Load(imageBytes);
            
            // Calculate new dimensions maintaining aspect ratio
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            if (ratio >= 1)
            {
                // Image is already smaller than max dimensions
                return imageBytes;
            }

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            image.Mutate(x => x.Resize(newWidth, newHeight));

            using var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 85 });
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to resize image");
            return null;
        }
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            _ => "application/octet-stream"
        };
    }

    private static string SanitizeFileName(string fileName)
    {
        // Remove any path components and invalid characters
        var name = Path.GetFileName(fileName);
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
        {
            name = name.Replace(c, '_');
        }
        // Replace spaces with underscores
        name = name.Replace(' ', '_');
        return name;
    }
}
