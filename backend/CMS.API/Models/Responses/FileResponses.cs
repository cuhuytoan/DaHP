namespace CMS.API.Models.Responses;

public class FileUploadResponse
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public DateTime UploadDate { get; set; }
}

public class FileInfoResponse
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool Exists { get; set; }
}

public class UploadConfigResponse
{
    public long MaxImageSize { get; set; }
    public long MaxFileSize { get; set; }
    public long MaxEditorImageSize { get; set; }
    public int MaxImagesPerUpload { get; set; }
    public string[] AllowedImageExtensions { get; set; } = Array.Empty<string>();
    public string[] AllowedFileExtensions { get; set; } = Array.Empty<string>();
}
