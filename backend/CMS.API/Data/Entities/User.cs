using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CMS.API.Data.Entities;

[Table("aspnetusers")]
public class ApplicationUser : IdentityUser
{
    [Column("emailconfirmed")]
    public override bool EmailConfirmed { get; set; }

    [Column("phonenumberconfirmed")]
    public override bool PhoneNumberConfirmed { get; set; }

    [Column("twofactorenabled")]
    public override bool TwoFactorEnabled { get; set; }

    [Column("lockoutenabled")]
    public override bool LockoutEnabled { get; set; }

    [Column("accessfailedcount")]
    public override int AccessFailedCount { get; set; }

    // Navigation properties
    public virtual AspNetUserProfile? Profile { get; set; }
    public virtual ICollection<ProductOrder> Orders { get; set; } = new List<ProductOrder>();
    public virtual ICollection<UserNotify> Notifies { get; set; } = new List<UserNotify>();
}

[Table("aspnetroles")]
public class ApplicationRole : IdentityRole
{
    [Column("aspnetrolegroup")]
    public string? AspNetRoleGroup { get; set; }
}

[Table("aspnetuserprofiles")]
public class AspNetUserProfile
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("userid")]
    public string? UserId { get; set; }

    [Column("regtype")]
    public string? RegType { get; set; }

    [Column("registerdate")]
    public DateTime? RegisterDate { get; set; }

    [Column("verified")]
    public bool? Verified { get; set; }

    [Column("verifieddate")]
    public DateTime? VerifiedDate { get; set; }

    [Column("lastlogindate")]
    public DateTime? LastLoginDate { get; set; }

    [Column("lastactivitydate")]
    public DateTime? LastActivityDate { get; set; }

    [Column("fullname")]
    public string? FullName { get; set; }

    [Column("gender")]
    public bool? Gender { get; set; }

    [Column("birthdate")]
    public DateTime? BirthDate { get; set; }

    [Column("company")]
    public string? Company { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("departmentid")]
    public long? DepartmentId { get; set; }

    [Column("rank")]
    public int? Rank { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("countryid")]
    public long? CountryId { get; set; }

    [Column("locationid")]
    public long? LocationId { get; set; }

    [Column("districtid")]
    public long? DistrictId { get; set; }

    [Column("wardid")]
    public long? WardId { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("website")]
    public string? Website { get; set; }

    [Column("facebookid")]
    public string? FacebookId { get; set; }

    [Column("skype")]
    public string? Skype { get; set; }

    [Column("zalo")]
    public string? Zalo { get; set; }

    [Column("telegram")]
    public string? Telegram { get; set; }

    [Column("avatarurl")]
    public string? AvatarUrl { get; set; }

    [Column("signature")]
    public string? Signature { get; set; }

    [Column("accounttype")]
    public int? AccountType { get; set; }

    [Column("department")]
    public string? Department { get; set; }

    [Column("allownotifyapp")]
    public bool? AllowNotifyApp { get; set; }

    [Column("allownotifyemail")]
    public bool? AllowNotifyEmail { get; set; }

    [Column("allownotifysms")]
    public bool? AllowNotifySms { get; set; }

    [Column("bankacc")]
    public string? BankAcc { get; set; }

    [Column("bankid")]
    public long? BankId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }

    [ForeignKey("DistrictId")]
    public virtual District? District { get; set; }

    [ForeignKey("WardId")]
    public virtual Ward? Ward { get; set; }

    [ForeignKey("CountryId")]
    public virtual Country? Country { get; set; }

    [ForeignKey("BankId")]
    public virtual Bank? Bank { get; set; }
}

[Table("department")]
public class Department
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }
}

[Table("departmentman")]
public class DepartmentMan
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("parentid")]
    public long? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [ForeignKey("ParentId")]
    public virtual DepartmentMan? Parent { get; set; }

    public virtual ICollection<DepartmentMan> Children { get; set; } = new List<DepartmentMan>();
}

[Table("location")]
public class Location
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("countryid")]
    public long? CountryId { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [ForeignKey("CountryId")]
    public virtual Country? Country { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}

[Table("district")]
public class District
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("locationid")]
    public long? LocationId { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}

[Table("ward")]
public class Ward
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("districtid")]
    public long? DistrictId { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [ForeignKey("DistrictId")]
    public virtual District? District { get; set; }
}

[Table("country")]
public class Country
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("phonecode")]
    public string? PhoneCode { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}

[Table("usernotify")]
public class UserNotify
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("userid")]
    public string? UserId { get; set; }

    [Column("notificationtypeid")]
    public long? NotificationTypeId { get; set; }

    [Column("title")]
    public string? Title { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("isread")]
    public bool? IsRead { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
}