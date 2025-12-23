using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.API.Data.Entities;

[Table("product")]
public class Product
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("producttypeid")]
    public long? ProductTypeId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("productmanufactureid")]
    public long? ProductManufactureId { get; set; }

    [Column("productstatusid")]
    public long? ProductStatusId { get; set; }

    [Column("countryid")]
    public long? CountryId { get; set; }

    [Column("unitid")]
    public long? UnitId { get; set; }

    [Column("productcategoryids")]
    public string? ProductCategoryIds { get; set; }

    [Column("barcode")]
    public string? Barcode { get; set; }

    [Column("manufacturesku")]
    public string? ManufactureSku { get; set; }

    [Column("sku")]
    public string? Sku { get; set; }

    [Column("qrcodepublic")]
    public string? QrCodePublic { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("subtitle")]
    public string? SubTitle { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("imagedescription")]
    public string? ImageDescription { get; set; }

    [Column("bannerimage")]
    public string? BannerImage { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("specification")]
    public string? Specification { get; set; }

    [Column("productcertificate")]
    public string? ProductCertificate { get; set; }

    [Column("legalinfo")]
    public string? LegalInfo { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("priceold")]
    public decimal? PriceOld { get; set; }

    [Column("pricewholesale")]
    public decimal? PriceWholesale { get; set; }

    [Column("wholesalemin")]
    public int? WholesaleMin { get; set; }

    [Column("discount")]
    public decimal? Discount { get; set; }

    [Column("discountrate")]
    public int? DiscountRate { get; set; }

    [Column("issecondhand")]
    public bool? IsSecondHand { get; set; }

    [Column("isauthor")]
    public bool? IsAuthor { get; set; }

    [Column("isbestsale")]
    public bool? IsBestSale { get; set; }

    [Column("issaleoff")]
    public bool? IsSaleOff { get; set; }

    [Column("isnew")]
    public bool? IsNew { get; set; }

    [Column("iscomming")]
    public bool? IsComming { get; set; }

    [Column("isoutstock")]
    public bool? IsOutStock { get; set; }

    [Column("isdiscontinue")]
    public bool? IsDiscontinue { get; set; }

    [Column("amountdefault")]
    public int? AmountDefault { get; set; }

    [Column("expirydisplay")]
    public string? ExpiryDisplay { get; set; }

    [Column("expirybyday")]
    public int? ExpiryByDay { get; set; }

    [Column("warrantydisplay")]
    public string? WarrantyDisplay { get; set; }

    [Column("warrantybymonth")]
    public int? WarrantyByMonth { get; set; }

    [Column("rate")]
    public int? Rate { get; set; }

    [Column("startdate")]
    public DateTime? StartDate { get; set; }

    [Column("enddate")]
    public DateTime? EndDate { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("likecount")]
    public int? LikeCount { get; set; }

    [Column("sellcount")]
    public int? SellCount { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("checked")]
    public int? Checked { get; set; }

    [Column("checkby")]
    public string? CheckBy { get; set; }

    [Column("checkdate")]
    public DateTime? CheckDate { get; set; }

    [Column("approved")]
    public int? Approved { get; set; }

    [Column("approveby")]
    public string? ApproveBy { get; set; }

    [Column("approvedate")]
    public DateTime? ApproveDate { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("tags")]
    public string? Tags { get; set; }

    [Column("cancopy")]
    public bool? CanCopy { get; set; }

    [Column("cancomment")]
    public bool? CanComment { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    [Column("metatitle")]
    public string? MetaTitle { get; set; }

    [Column("metadescription")]
    public string? MetaDescription { get; set; }

    [Column("metakeywords")]
    public string? MetaKeywords { get; set; }

    [Column("documentrefer")]
    public string? DocumentRefer { get; set; }

    // Navigation properties
    [ForeignKey("ProductTypeId")]
    public virtual ProductType? ProductType { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }

    [ForeignKey("ProductManufactureId")]
    public virtual ProductManufacture? ProductManufacture { get; set; }

    [ForeignKey("ProductStatusId")]
    public virtual ProductStatus? ProductStatus { get; set; }

    [ForeignKey("UnitId")]
    public virtual Unit? Unit { get; set; }

    [ForeignKey("CountryId")]
    public virtual Country? Country { get; set; }

    public virtual ICollection<ProductComment> Comments { get; set; } = new List<ProductComment>();
    public virtual ICollection<ProductCommentStaff> CommentStaffs { get; set; } = new List<ProductCommentStaff>();
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<ProductPicture> Pictures { get; set; } = new List<ProductPicture>();
    public virtual ICollection<ProductAttachFile> AttachFiles { get; set; } = new List<ProductAttachFile>();
    public virtual ICollection<ProductCategoryProduct> CategoryProducts { get; set; } = new List<ProductCategoryProduct>();
    public virtual ICollection<ProductBlockProduct> BlockProducts { get; set; } = new List<ProductBlockProduct>();
    public virtual ICollection<ProductRelationProduct> RelationProducts { get; set; } = new List<ProductRelationProduct>();
    public virtual ICollection<ProductPropertyValue> PropertyValues { get; set; } = new List<ProductPropertyValue>();
    public virtual ICollection<ProductLike> Likes { get; set; } = new List<ProductLike>();
    public virtual ICollection<ProductTop> ProductTops { get; set; } = new List<ProductTop>();
}

[Table("productcategory")]
public class ProductCategory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("parentid")]
    public long? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("displaymenu")]
    public bool? DisplayMenu { get; set; }

    [Column("displaymenuhorizontal")]
    public bool? DisplayMenuHorizontal { get; set; }

    [Column("menucolor")]
    public string? MenuColor { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditedby")]
    public string? LastEditedBy { get; set; }

    [Column("lastediteddate")]
    public DateTime? LastEditedDate { get; set; }

    [Column("metadescription")]
    public string? MetaDescription { get; set; }

    [Column("metakeywords")]
    public string? MetaKeywords { get; set; }

    // Navigation properties
    [ForeignKey("ParentId")]
    public virtual ProductCategory? Parent { get; set; }

    public virtual ICollection<ProductCategory> Children { get; set; } = new List<ProductCategory>();
    public virtual ICollection<ProductCategoryProduct> CategoryProducts { get; set; } = new List<ProductCategoryProduct>();
    public virtual ICollection<ProductCategoryAssign> CategoryAssigns { get; set; } = new List<ProductCategoryAssign>();
}

[Table("productstatus")]
public class ProductStatus
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("producttype")]
public class ProductType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productbrand")]
public class ProductBrand
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productbrandcategoryid")]
    public long? ProductBrandCategoryId { get; set; }

    [Column("productbrandtypeid")]
    public long? ProductBrandTypeId { get; set; }

    [Column("departmentmanid")]
    public long? DepartmentManId { get; set; }

    [Column("productbrandmodelmanagement_id")]
    public long? ProductBrandModelManagementId { get; set; }

    [Column("productbrandlevelid")]
    public long? ProductBrandLevelId { get; set; }

    [Column("productbrandstatusid")]
    public long? ProductBrandStatusId { get; set; }

    [Column("countryid")]
    public long? CountryId { get; set; }

    [Column("locationid")]
    public long? LocationId { get; set; }

    [Column("districtid")]
    public long? DistrictId { get; set; }

    [Column("wardid")]
    public long? WardId { get; set; }

    [Column("bankid")]
    public long? BankId { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("qrcodepublic")]
    public string? QrCodePublic { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("tradingname")]
    public string? TradingName { get; set; }

    [Column("brandname")]
    public string? BrandName { get; set; }

    [Column("taxcode")]
    public string? TaxCode { get; set; }

    [Column("registrationnumber")]
    public string? RegistrationNumber { get; set; }

    [Column("issueddate")]
    public DateTime? IssuedDate { get; set; }

    [Column("businessarea")]
    public string? BusinessArea { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("telephone")]
    public string? Telephone { get; set; }

    [Column("fax")]
    public string? Fax { get; set; }

    [Column("mobile")]
    public string? Mobile { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("website")]
    public string? Website { get; set; }

    [Column("facebook")]
    public string? Facebook { get; set; }

    [Column("zalo")]
    public string? Zalo { get; set; }

    [Column("hotline")]
    public string? Hotline { get; set; }

    [Column("skype")]
    public string? Skype { get; set; }

    [Column("bankacc")]
    public string? BankAcc { get; set; }

    [Column("prinfo")]
    public string? PrInfo { get; set; }

    [Column("agency")]
    public string? Agency { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("legaldocument")]
    public string? LegalDocument { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("personsurname")]
    public string? PersonSurname { get; set; }

    [Column("personname")]
    public string? PersonName { get; set; }

    [Column("personaddress")]
    public string? PersonAddress { get; set; }

    [Column("personmobile")]
    public string? PersonMobile { get; set; }

    [Column("personzalo")]
    public string? PersonZalo { get; set; }

    [Column("personemail")]
    public string? PersonEmail { get; set; }

    [Column("personposition")]
    public string? PersonPosition { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("hasqrcode")]
    public bool? HasQrCode { get; set; }

    [Column("viewcount")]
    public int? ViewCount { get; set; }

    [Column("viewpagecount")]
    public int? ViewPageCount { get; set; }

    [Column("followcount")]
    public int? FollowCount { get; set; }

    [Column("sellcount")]
    public int? SellCount { get; set; }

    [Column("accountusername")]
    public string? AccountUsername { get; set; }

    [Column("accountemail")]
    public string? AccountEmail { get; set; }

    [Column("directorname")]
    public string? DirectorName { get; set; }

    [Column("directorbirthday")]
    public DateTime? DirectorBirthday { get; set; }

    [Column("directoraddress")]
    public string? DirectorAddress { get; set; }

    [Column("directormobile")]
    public string? DirectorMobile { get; set; }

    [Column("directoremail")]
    public string? DirectorEmail { get; set; }

    [Column("directorposition")]
    public string? DirectorPosition { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("checked")]
    public int? Checked { get; set; }

    [Column("checkby")]
    public string? CheckBy { get; set; }

    [Column("checkdate")]
    public DateTime? CheckDate { get; set; }

    [Column("approved")]
    public int? Approved { get; set; }

    [Column("approveby")]
    public string? ApproveBy { get; set; }

    [Column("approvedate")]
    public DateTime? ApproveDate { get; set; }

    [Column("googlemapcode")]
    public string? GoogleMapCode { get; set; }

    // Navigation properties
    [ForeignKey("ProductBrandCategoryId")]
    public virtual ProductBrandCategory? ProductBrandCategory { get; set; }

    [ForeignKey("ProductBrandTypeId")]
    public virtual ProductBrandType? ProductBrandType { get; set; }

    [ForeignKey("ProductBrandStatusId")]
    public virtual ProductBrandStatus? ProductBrandStatus { get; set; }

    [ForeignKey("ProductBrandLevelId")]
    public virtual ProductBrandLevel? ProductBrandLevel { get; set; }

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

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    public virtual ICollection<ProductBrandAttachFile> AttachFiles { get; set; } = new List<ProductBrandAttachFile>();
    public virtual ICollection<ProductBrandFollow> Follows { get; set; } = new List<ProductBrandFollow>();
}

[Table("productbrandtype")]
public class ProductBrandType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productbrandstatus")]
public class ProductBrandStatus
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productbrandlevel")]
public class ProductBrandLevel
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productbrandcategory")]
public class ProductBrandCategory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("departmentmanid")]
    public long? DepartmentManId { get; set; }

    [Column("parentid")]
    public long? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("displaymenu")]
    public bool? DisplayMenu { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditedby")]
    public string? LastEditedBy { get; set; }

    [Column("lastediteddate")]
    public DateTime? LastEditedDate { get; set; }

    [ForeignKey("ParentId")]
    public virtual ProductBrandCategory? Parent { get; set; }

    public virtual ICollection<ProductBrandCategory> Children { get; set; } = new List<ProductBrandCategory>();
}

[Table("productbrandattachfile")]
public class ProductBrandAttachFile
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("attachfilename")]
    public string? AttachFileName { get; set; }

    [Column("filetype")]
    public string? FileType { get; set; }

    [Column("filesize")]
    public double? FileSize { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }
}

[Table("productbrandfollow")]
public class ProductBrandFollow
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("customerid")]
    public long? CustomerId { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }
}

[Table("productbrandmodelmanagement")]
public class ProductBrandModelManagement
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productbrandqrcodecreatetype")]
public class ProductBrandQrCodeCreateType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("productmanufacture")]
public class ProductManufacture
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }
}

[Table("productcomment")]
public class ProductComment
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("parentid")]
    public long? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ParentId")]
    public virtual ProductComment? Parent { get; set; }

    public virtual ICollection<ProductComment> Children { get; set; } = new List<ProductComment>();
}

[Table("productcommentstaff")]
public class ProductCommentStaff
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("parentid")]
    public long? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productreview")]
public class ProductReview
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productreviewtypeid")]
    public long? ProductReviewTypeId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("customerid")]
    public long? CustomerId { get; set; }

    [Column("qrcodepubliccontent")]
    public string? QrCodePublicContent { get; set; }

    [Column("qrcodesecretcontent")]
    public string? QrCodeSecretContent { get; set; }

    [Column("star")]
    public int? Star { get; set; }

    [Column("customername")]
    public string? CustomerName { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }
}

[Table("productpicture")]
public class ProductPicture
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("image")]
    public string? Image { get; set; }

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

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productattachfile")]
public class ProductAttachFile
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("attachfilename")]
    public string? AttachFileName { get; set; }

    [Column("filetype")]
    public string? FileType { get; set; }

    [Column("filesize")]
    public double? FileSize { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productblock")]
public class ProductBlock
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productcategoryid")]
    public long? ProductCategoryId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("style_id")]
    public long? StyleId { get; set; }

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

    public virtual ICollection<ProductBlockProduct> ProductBlockProducts { get; set; } = new List<ProductBlockProduct>();
}

[Table("productblockproduct")]
public class ProductBlockProduct
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productblockid")]
    public long ProductBlockId { get; set; }

    [Column("productid")]
    public long ProductId { get; set; }

    [ForeignKey("ProductBlockId")]
    public virtual ProductBlock? ProductBlock { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productcategoryproduct")]
public class ProductCategoryProduct
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long ProductId { get; set; }

    [Column("productcategoryid")]
    public long ProductCategoryId { get; set; }

    [ForeignKey("ProductCategoryId")]
    public virtual ProductCategory? ProductCategory { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productcategoryassign")]
public class ProductCategoryAssign
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productcategoryid")]
    public long? ProductCategoryId { get; set; }

    [Column("aspnetusersid")]
    public string? AspNetUsersId { get; set; }

    [ForeignKey("ProductCategoryId")]
    public virtual ProductCategory? ProductCategory { get; set; }
}

[Table("productrelationproduct")]
public class ProductRelationProduct
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("productrelationid")]
    public long? ProductRelationId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductRelationId")]
    public virtual Product? ProductRelation { get; set; }
}

[Table("productlike")]
public class ProductLike
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("customerid")]
    public long? CustomerId { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }
}

[Table("producttop")]
public class ProductTop
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productcategoryid")]
    public long? ProductCategoryId { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductCategoryId")]
    public virtual ProductCategory? ProductCategory { get; set; }
}

[Table("productlogedit")]
public class ProductLogEdit
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("producttypeid")]
    public long? ProductTypeId { get; set; }

    [Column("productcategoryids")]
    public string? ProductCategoryIds { get; set; }

    [Column("productmanufactureid")]
    public long? ProductManufactureId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("countryid")]
    public long? CountryId { get; set; }

    [Column("productstatusid")]
    public long? ProductStatusId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("barcode")]
    public string? Barcode { get; set; }

    [Column("manufacturesku")]
    public string? ManufactureSku { get; set; }

    [Column("sku")]
    public string? Sku { get; set; }

    [Column("qrcodepublic")]
    public string? QrCodePublic { get; set; }

    [Column("subtitle")]
    public string? SubTitle { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("imagedescription")]
    public string? ImageDescription { get; set; }

    [Column("bannerimage")]
    public string? BannerImage { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("specification")]
    public string? Specification { get; set; }

    [Column("productcertificate")]
    public string? ProductCertificate { get; set; }

    [Column("legalinfo")]
    public string? LegalInfo { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("priceold")]
    public decimal? PriceOld { get; set; }

    [Column("pricewholesale")]
    public decimal? PriceWholesale { get; set; }

    [Column("wholesalemin")]
    public int? WholesaleMin { get; set; }

    [Column("discount")]
    public decimal? Discount { get; set; }

    [Column("discountrate")]
    public int? DiscountRate { get; set; }

    [Column("issecondhand")]
    public bool? IsSecondHand { get; set; }

    [Column("isauthor")]
    public bool? IsAuthor { get; set; }

    [Column("isbestsale")]
    public bool? IsBestSale { get; set; }

    [Column("issaleoff")]
    public bool? IsSaleOff { get; set; }

    [Column("isnew")]
    public bool? IsNew { get; set; }

    [Column("iscomming")]
    public bool? IsComming { get; set; }

    [Column("isoutstock")]
    public bool? IsOutStock { get; set; }

    [Column("isdiscontinue")]
    public bool? IsDiscontinue { get; set; }

    [Column("amountdefault")]
    public int? AmountDefault { get; set; }

    [Column("unitid")]
    public long? UnitId { get; set; }

    [Column("expirydisplay")]
    public string? ExpiryDisplay { get; set; }

    [Column("expirybyday")]
    public int? ExpiryByDay { get; set; }

    [Column("warrantydisplay")]
    public string? WarrantyDisplay { get; set; }

    [Column("warrantybymonth")]
    public int? WarrantyByMonth { get; set; }

    [Column("rate")]
    public int? Rate { get; set; }

    [Column("startdate")]
    public DateTime? StartDate { get; set; }

    [Column("enddate")]
    public DateTime? EndDate { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("counter")]
    public int? Counter { get; set; }

    [Column("likecount")]
    public int? LikeCount { get; set; }

    [Column("sellcount")]
    public int? SellCount { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    [Column("checked")]
    public int? Checked { get; set; }

    [Column("checkby")]
    public string? CheckBy { get; set; }

    [Column("checkdate")]
    public DateTime? CheckDate { get; set; }

    [Column("approved")]
    public int? Approved { get; set; }

    [Column("approveby")]
    public string? ApproveBy { get; set; }

    [Column("approvedate")]
    public DateTime? ApproveDate { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("tags")]
    public string? Tags { get; set; }

    [Column("cancopy")]
    public bool? CanCopy { get; set; }

    [Column("cancomment")]
    public bool? CanComment { get; set; }

    [Column("candelete")]
    public bool? CanDelete { get; set; }

    [Column("metatitle")]
    public string? MetaTitle { get; set; }

    [Column("metadescription")]
    public string? MetaDescription { get; set; }

    [Column("metakeywords")]
    public string? MetaKeywords { get; set; }

    [Column("documentrefer")]
    public string? DocumentRefer { get; set; }
}

[Table("unit")]
public class Unit
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}