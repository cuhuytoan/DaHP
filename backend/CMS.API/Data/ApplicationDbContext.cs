using CMS.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Article entities
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
    public DbSet<ArticleStatus> ArticleStatuses { get; set; } = null!;
    public DbSet<ArticleType> ArticleTypes { get; set; } = null!;
    public DbSet<ArticleBlock> ArticleBlocks { get; set; } = null!;
    public DbSet<ArticleBlockArticle> ArticleBlockArticles { get; set; } = null!;
    public DbSet<ArticleCategoryArticle> ArticleCategoryArticles { get; set; } = null!;
    public DbSet<ArticleCategoryAssign> ArticleCategoryAssigns { get; set; } = null!;
    public DbSet<ArticleComment> ArticleComments { get; set; } = null!;
    public DbSet<ArticleCommentStaff> ArticleCommentStaffs { get; set; } = null!;
    public DbSet<ArticleAttachFile> ArticleAttachFiles { get; set; } = null!;
    public DbSet<ArticleTop> ArticleTops { get; set; } = null!;
    public DbSet<ArticleRelationArticle> ArticleRelationArticles { get; set; } = null!;

    // Product entities
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<ProductStatus> ProductStatuses { get; set; } = null!;
    public DbSet<ProductType> ProductTypes { get; set; } = null!;
    public DbSet<ProductBrand> ProductBrands { get; set; } = null!;
    public DbSet<ProductBrandType> ProductBrandTypes { get; set; } = null!;
    public DbSet<ProductBrandStatus> ProductBrandStatuses { get; set; } = null!;
    public DbSet<ProductBrandLevel> ProductBrandLevels { get; set; } = null!;
    public DbSet<ProductBlock> ProductBlocks { get; set; } = null!;
    public DbSet<ProductBlockProduct> ProductBlockProducts { get; set; } = null!;
    public DbSet<ProductCategoryProduct> ProductCategoryProducts { get; set; } = null!;
    public DbSet<ProductComment> ProductComments { get; set; } = null!;
    public DbSet<ProductReview> ProductReviews { get; set; } = null!;
    public DbSet<ProductPicture> ProductPictures { get; set; } = null!;
    public DbSet<ProductProperty> ProductProperties { get; set; } = null!;
    public DbSet<ProductPropertyCategory> ProductPropertyCategories { get; set; } = null!;
    public DbSet<ProductPropertyType> ProductPropertyTypes { get; set; } = null!;
    public DbSet<ProductPropertyValue> ProductPropertyValues { get; set; } = null!;
    public DbSet<ProductManufacture> ProductManufactures { get; set; } = null!;
    public DbSet<ProductAttachFile> ProductAttachFiles { get; set; } = null!;

    // Order entities
    public DbSet<ProductOrder> ProductOrders { get; set; } = null!;
    public DbSet<ProductOrderDetail> ProductOrderDetails { get; set; } = null!;
    public DbSet<ProductOrderStatus> ProductOrderStatuses { get; set; } = null!;
    public DbSet<ProductOrderPaymentStatus> ProductOrderPaymentStatuses { get; set; } = null!;
    public DbSet<ProductOrderPaymentMethod> ProductOrderPaymentMethods { get; set; } = null!;

    // Location entities
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<District> Districts { get; set; } = null!;
    public DbSet<Ward> Wards { get; set; } = null!;

    // User related entities
    public DbSet<AspNetUserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<DepartmentMan> DepartmentMans { get; set; } = null!;
    public DbSet<UserNotify> UserNotifies { get; set; } = null!;
    public DbSet<Bank> Banks { get; set; } = null!;

    // Advertising entities
    public DbSet<Advertising> Advertisings { get; set; } = null!;
    public DbSet<AdvertisingBlock> AdvertisingBlocks { get; set; } = null!;
    public DbSet<AdvertisingType> AdvertisingTypes { get; set; } = null!;

    // Other entities
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<LogVisit> LogVisits { get; set; } = null!;
    public DbSet<ReplaceChar> ReplaceChars { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Advise> Advises { get; set; } = null!;
    public DbSet<SysDiagrams> SysDiagrams { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Identity tables to match PostgreSQL schema (lowercase)
        modelBuilder.Entity<ApplicationUser>().ToTable("aspnetusers");
        modelBuilder.Entity<ApplicationRole>().ToTable("aspnetroles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("aspnetuserroles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("aspnetuserclaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("aspnetuserlogins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("aspnetusertokens");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("aspnetroleclaims");

        // Configure Identity column mappings
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserName).HasColumnName("username");
            entity.Property(e => e.NormalizedUserName).HasColumnName("normalizedusername");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.NormalizedEmail).HasColumnName("normalizedemail");
            entity.Property(e => e.EmailConfirmed).HasColumnName("emailconfirmed");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
            entity.Property(e => e.SecurityStamp).HasColumnName("securitystamp");
            entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrencystamp");
            entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber");
            entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phonenumberconfirmed");
            entity.Property(e => e.TwoFactorEnabled).HasColumnName("twofactorenabled");
            entity.Property(e => e.LockoutEnd).HasColumnName("lockoutend");
            entity.Property(e => e.LockoutEnabled).HasColumnName("lockoutenabled");
            entity.Property(e => e.AccessFailedCount).HasColumnName("accessfailedcount");
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NormalizedName).HasColumnName("normalizedname");
            entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrencystamp");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.RoleId).HasColumnName("roleid");
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.ClaimType).HasColumnName("claimtype");
            entity.Property(e => e.ClaimValue).HasColumnName("claimvalue");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.Property(e => e.LoginProvider).HasColumnName("loginprovider");
            entity.Property(e => e.ProviderKey).HasColumnName("providerkey");
            entity.Property(e => e.ProviderDisplayName).HasColumnName("providerdisplayname");
            entity.Property(e => e.UserId).HasColumnName("userid");
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.LoginProvider).HasColumnName("loginprovider");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("roleid");
            entity.Property(e => e.ClaimType).HasColumnName("claimtype");
            entity.Property(e => e.ClaimValue).HasColumnName("claimvalue");
        });

        // Configure UserProfile relationship
        modelBuilder.Entity<AspNetUserProfile>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<AspNetUserProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Article indexes
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasIndex(e => e.Url);
            entity.HasIndex(e => e.Active);
            entity.HasIndex(e => e.CreateDate);
        });

        // Configure Product indexes
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Url);
            entity.HasIndex(e => e.Sku);
            entity.HasIndex(e => e.Active);
            entity.HasIndex(e => e.CreateDate);
        });

        // Configure ProductOrder
        modelBuilder.Entity<ProductOrder>(entity =>
        {
            entity.HasIndex(e => e.OrderCode);
        });

        // Configure RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ArticleRelationArticle - self-referencing many-to-many for Article
        modelBuilder.Entity<ArticleRelationArticle>(entity =>
        {
            entity.HasOne(e => e.Article)
                .WithMany(a => a.RelationArticles)
                .HasForeignKey(e => e.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ArticleRelation)
                .WithMany()
                .HasForeignKey(e => e.ArticleRelationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProductRelationProduct - self-referencing many-to-many for Product
        modelBuilder.Entity<ProductRelationProduct>(entity =>
        {
            entity.HasOne(e => e.Product)
                .WithMany(p => p.RelationProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ProductRelation)
                .WithMany()
                .HasForeignKey(e => e.ProductRelationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ArticleCategory self-referencing
        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ArticleComment self-referencing
        modelBuilder.Entity<ArticleComment>(entity =>
        {
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}