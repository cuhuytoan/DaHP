using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.Requests;

public class CreateCommentRequest
{
    [Required(ErrorMessage = "Entity ID is required (Article ID or Product ID)")]
    public int EntityId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;
}

public class UpdateCommentRequest
{
    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;

    public bool Active { get; set; }
}

public class UpdateCommentStatusRequest
{
    [Required]
    public bool Active { get; set; }
}

public class CreateCommentReplyRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;
}

public class CreateReviewRequest
{
    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;
}

public class UpdateReviewRequest
{
    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;
}
