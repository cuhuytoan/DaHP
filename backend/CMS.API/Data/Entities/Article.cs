using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.API.Data.Entities;

[Table("article")]
public class Article
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articletypeid")]
    public long? ArticleTypeId { get; set; }

    [Column("articlestatusid")]
    public long? ArticleStatusId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("articlecategoryids")]
    public string? ArticleCategoryIds { get; set; }

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

    [Column("author")]
    public string? Author { get; set; }

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
    [ForeignKey("ArticleTypeId")]
    public virtual ArticleType? ArticleType { get; set; }

    [ForeignKey("ArticleStatusId")]
    public virtual ArticleStatus? ArticleStatus { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }

    public virtual ICollection<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
    public virtual ICollection<ArticleCommentStaff> CommentStaffs { get; set; } = new List<ArticleCommentStaff>();
    public virtual ICollection<ArticleAttachFile> AttachFiles { get; set; } = new List<ArticleAttachFile>();
    public virtual ICollection<ArticleCategoryArticle> CategoryArticles { get; set; } = new List<ArticleCategoryArticle>();
    public virtual ICollection<ArticleBlockArticle> BlockArticles { get; set; } = new List<ArticleBlockArticle>();
    public virtual ICollection<ArticleRelationArticle> RelationArticles { get; set; } = new List<ArticleRelationArticle>();
    public virtual ICollection<ArticleTop> ArticleTops { get; set; } = new List<ArticleTop>();
}

[Table("articlecategory")]
public class ArticleCategory
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

    // Navigation properties
    [ForeignKey("ParentId")]
    public virtual ArticleCategory? Parent { get; set; }

    public virtual ICollection<ArticleCategory> Children { get; set; } = new List<ArticleCategory>();
    public virtual ICollection<ArticleCategoryArticle> CategoryArticles { get; set; } = new List<ArticleCategoryArticle>();
    public virtual ICollection<ArticleCategoryAssign> CategoryAssigns { get; set; } = new List<ArticleCategoryAssign>();
}

[Table("articlestatus")]
public class ArticleStatus
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("articletype")]
public class ArticleType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

[Table("articleblock")]
public class ArticleBlock
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articlecategoryid")]
    public long? ArticleCategoryId { get; set; }

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

    public virtual ICollection<ArticleBlockArticle> ArticleBlockArticles { get; set; } = new List<ArticleBlockArticle>();
}

[Table("articleblockarticle")]
public class ArticleBlockArticle
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articleblockid")]
    public long? ArticleBlockId { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

    [ForeignKey("ArticleBlockId")]
    public virtual ArticleBlock? ArticleBlock { get; set; }

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }
}

[Table("articlecategoryarticle")]
public class ArticleCategoryArticle
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articlecategoryid")]
    public long? ArticleCategoryId { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

    [ForeignKey("ArticleCategoryId")]
    public virtual ArticleCategory? ArticleCategory { get; set; }

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }
}

[Table("articlecategoryassign")]
public class ArticleCategoryAssign
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articlecategoryid")]
    public long? ArticleCategoryId { get; set; }

    [Column("aspnetusersid")]
    public string? AspNetUsersId { get; set; }

    [ForeignKey("ArticleCategoryId")]
    public virtual ArticleCategory? ArticleCategory { get; set; }
}

[Table("articlerelationarticle")]
public class ArticleRelationArticle
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

    [Column("articlerelationid")]
    public long? ArticleRelationId { get; set; }

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }

    [ForeignKey("ArticleRelationId")]
    public virtual Article? ArticleRelation { get; set; }
}

[Table("articletop")]
public class ArticleTop
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articlecategoryid")]
    public long? ArticleCategoryId { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }

    [ForeignKey("ArticleCategoryId")]
    public virtual ArticleCategory? ArticleCategory { get; set; }
}

[Table("articlecomment")]
public class ArticleComment
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

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

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }

    [ForeignKey("ParentId")]
    public virtual ArticleComment? Parent { get; set; }

    public virtual ICollection<ArticleComment> Children { get; set; } = new List<ArticleComment>();
}

[Table("articlecommentstaff")]
public class ArticleCommentStaff
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

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

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }
}

[Table("articleattachfile")]
public class ArticleAttachFile
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("articleid")]
    public long? ArticleId { get; set; }

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

    [ForeignKey("ArticleId")]
    public virtual Article? Article { get; set; }
}