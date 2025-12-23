using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.API.Data.Entities;

[Table("advertising")]
public class Advertising
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("advertisingblockid")]
    public long? AdvertisingBlockId { get; set; }

    [Column("advertisingtypeid")]
    public long? AdvertisingTypeId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("startdate")]
    public DateTime? StartDate { get; set; }

    [Column("enddate")]
    public DateTime? EndDate { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditedby")]
    public string? LastEditedBy { get; set; }

    [Column("lastediteddate")]
    public DateTime? LastEditedDate { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    [ForeignKey("AdvertisingBlockId")]
    public virtual AdvertisingBlock? AdvertisingBlock { get; set; }

    [ForeignKey("AdvertisingTypeId")]
    public virtual AdvertisingType? AdvertisingType { get; set; }
}

[Table("advertisingblock")]
public class AdvertisingBlock
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("width")]
    public int? Width { get; set; }

    [Column("height")]
    public int? Height { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditedby")]
    public string? LastEditedBy { get; set; }

    [Column("lastediteddate")]
    public DateTime? LastEditedDate { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    public virtual ICollection<Advertising> Advertisings { get; set; } = new List<Advertising>();
}

[Table("advertisingtype")]
public class AdvertisingType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("setting")]
public class Setting
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("domain")]
    public string? Domain { get; set; }

    [Column("websitename")]
    public string? WebsiteName { get; set; }

    [Column("adminname")]
    public string? AdminName { get; set; }

    [Column("emailsupport")]
    public string? EmailSupport { get; set; }

    [Column("emailorder")]
    public string? EmailOrder { get; set; }

    [Column("emailsendersmtp")]
    public string? EmailSenderSmtp { get; set; }

    [Column("emailsenderport")]
    public string? EmailSenderPort { get; set; }

    [Column("emailsenderssl")]
    public bool? EmailSenderSsl { get; set; }

    [Column("emailsender")]
    public string? EmailSender { get; set; }

    [Column("emailsenderpassword")]
    public string? EmailSenderPassword { get; set; }

    [Column("telephone")]
    public string? Telephone { get; set; }

    [Column("appstatus")]
    public bool AppStatus { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("defaultlanguage_id")]
    public long DefaultLanguageId { get; set; }

    [Column("defaultskin_id")]
    public long DefaultSkinId { get; set; }

    [Column("metadescriptiondefault")]
    public string? MetaDescriptionDefault { get; set; }

    [Column("metakeywordsdefault")]
    public string? MetaKeywordsDefault { get; set; }

    [Column("metatitledefault")]
    public string? MetaTitleDefault { get; set; }

    [Column("googleanalyticscode")]
    public string? GoogleAnalyticsCode { get; set; }

    [Column("othercode")]
    public string? OtherCode { get; set; }

    [Column("facebookpageid")]
    public string? FacebookPageId { get; set; }

    [Column("facebookappid")]
    public string? FacebookAppId { get; set; }
}

[Table("logvisit")]
public class LogVisit
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("objecttype")]
    public string? ObjectType { get; set; }

    [Column("objecttypename")]
    public string? ObjectTypeName { get; set; }

    [Column("objectid")]
    public string? ObjectId { get; set; }

    [Column("objectname")]
    public string? ObjectName { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("productbrandname")]
    public string? ProductBrandName { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("sessionid")]
    public string? SessionId { get; set; }

    [Column("userid")]
    public string? UserId { get; set; }

    [Column("username")]
    public string? UserName { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("agentid")]
    public string? AgentId { get; set; }
}

[Table("replacechar")]
public class ReplaceChar
{
    [Key]
    [Column("replacechar_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("oldchar")]
    public string? OldChar { get; set; }

    [Column("newchar")]
    public string? NewChar { get; set; }
}

[Table("contact")]
public class Contact
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("fullname")]
    public string? FullName { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("phonenumber")]
    public string? PhoneNumber { get; set; }

    [Column("descriptions")]
    public string? Descriptions { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("lasteditedby")]
    public string? LastEditedBy { get; set; }

    [Column("lastediteddate")]
    public DateTime? LastEditedDate { get; set; }
}

[Table("advise")]
public class Advise
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("fullname")]
    public string? FullName { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("phonenumber")]
    public string? PhoneNumber { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }
}

[Table("productproperty")]
public class ProductProperty
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productcategoryid")]
    public long? ProductCategoryId { get; set; }

    [Column("productpropertycategoryid")]
    public long? ProductPropertyCategoryId { get; set; }

    [Column("productpropertytypeid")]
    public long? ProductPropertyTypeId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("unitid")]
    public long? UnitId { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [ForeignKey("ProductPropertyCategoryId")]
    public virtual ProductPropertyCategory? ProductPropertyCategory { get; set; }

    [ForeignKey("ProductPropertyTypeId")]
    public virtual ProductPropertyType? ProductPropertyType { get; set; }

    public virtual ICollection<ProductPropertyValue> PropertyValues { get; set; } = new List<ProductPropertyValue>();
}

[Table("productpropertycategory")]
public class ProductPropertyCategory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productcategoryid")]
    public long? ProductCategoryId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    public virtual ICollection<ProductProperty> Properties { get; set; } = new List<ProductProperty>();
}

[Table("productpropertytype")]
public class ProductPropertyType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("templatedisplay")]
    public string? TemplateDisplay { get; set; }

    [Column("templateedit")]
    public string? TemplateEdit { get; set; }
}

[Table("productpropertyvalue")]
public class ProductPropertyValue
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("productpropertyid")]
    public long? ProductPropertyId { get; set; }

    [Column("value")]
    public string? Value { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductPropertyId")]
    public virtual ProductProperty? ProductProperty { get; set; }
}

[Table("sysdiagrams")]
public class SysDiagrams
{
    [Key]
    [Column("diagram_id")]
    public long DiagramId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("principal_id")]
    public long PrincipalId { get; set; }

    [Column("version")]
    public int? Version { get; set; }

    [Column("definition")]
    public byte[]? Definition { get; set; }
}

[Table("bank")]
public class Bank
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("shortname")]
    public string? ShortName { get; set; }

    [Column("logo")]
    public string? Logo { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }
}

[Table("newsletter_subscription")]
public class NewsletterSubscription
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("createdate")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [Column("unsubscribedate")]
    public DateTime? UnsubscribeDate { get; set; }
}