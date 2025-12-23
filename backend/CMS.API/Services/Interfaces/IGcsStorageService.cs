namespace CMS.API.Services.Interfaces;

/// <summary>
/// Result of a GCS upload operation
/// </summary>
public class GcsUploadResult
{
    public bool Success { get; set; }
    public string? Bucket { get; set; }
    public string? ObjectPath { get; set; }
    public string? ContentType { get; set; }
    public long Size { get; set; }
    public string? Url { get; set; }
    public string? Error { get; set; }
    
    // Additional URLs for image variants
    public string? SmallUrl { get; set; }
    public string? ThumbUrl { get; set; }
}

/// <summary>
/// Interface for Google Cloud Storage operations
/// </summary>
public interface IGcsStorageService
{
    /// <summary>
    /// Upload an image to GCS with optional resizing
    /// </summary>
    Task<GcsUploadResult> UploadImageAsync(Stream fileStream, string fileName, string folder, bool generateVariants = false);
    
    /// <summary>
    /// Upload a file to GCS
    /// </summary>
    Task<GcsUploadResult> UploadFileAsync(Stream fileStream, string fileName, string folder);
    
    /// <summary>
    /// Delete an object from GCS
    /// </summary>
    Task<bool> DeleteAsync(string objectPath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if an object exists in GCS
    /// </summary>
    Task<bool> ExistsAsync(string objectPath);
    
    /// <summary>
    /// Get the public URL for an object
    /// </summary>
    string GetPublicUrl(string objectPath);
}
