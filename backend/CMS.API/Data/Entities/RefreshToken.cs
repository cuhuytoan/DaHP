using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.API.Data.Entities;

[Table("refreshtoken")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("userid")]
    public string UserId { get; set; } = null!;

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("jwtid")]
    public string JwtId { get; set; } = null!;

    [Column("isused")]
    public bool IsUsed { get; set; }

    [Column("isrevoked")]
    public bool IsRevoked { get; set; }

    [Column("createdate")]
    public DateTime CreateDate { get; set; }

    [Column("expirydate")]
    public DateTime ExpiryDate { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
}
