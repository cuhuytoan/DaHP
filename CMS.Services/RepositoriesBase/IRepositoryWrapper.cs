namespace CMS.Services.RepositoriesBase
{
    public interface IRepositoryWrapper
    {
        IAccountRepository AspNetUsers { get; }
        //User and Common
        ISettingRepository Setting { get; }
        IUserNotiRepository UserNoti { get; }
        IMasterDataRepository MasterData { get; }
        IPermissionRepository Permission { get; }
        //Article
        IArticleRepository Article { get; }
        IArticleBlockRepository ArticleBlock { get; }
        IArticleCategoryRepository ArticleCategory { get; }
        IArticleCommentRepository ArticleComment { get; }
        IArticleCommentStaffRepository ArticleCommentStaff { get; }
        //Product
        IProductBrandRepository ProductBrand { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductCommentRepository ProductComment { get; }
        IProductCommentStaffRepository ProductCommentStaff { get; }
        IProductRepository Product { get; }
        IProductPictureRepository ProductPicture { get; }
        IProductPropertiesRepository ProductProperties { get; }
        IProductBlockRepository ProductBlock { get; }

        IProductReviewRepository ProductReview { get; }

        IAdvertisingRepository Advertising { get; }

        IProductOrderRepository ProductOrder { get; }

        ILogVisitRepository LogVisit { get; }

        //Home 
        IHomeRepository Home { get; }

        void Save();

        Task<int> SaveChangesAsync();
    }
}