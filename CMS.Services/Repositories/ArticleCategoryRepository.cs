namespace CMS.Services.Repositories
{
    public interface IArticleCategoryRepository : IRepositoryBase<ArticleCategory>
    {
        Task<List<ArticleCategory>> GetArticleCategoryById(int? ArticleCategoryId);

        Task<ArticleCategory> ArticleCategoryGetById(int articleCategoryId);

        Task<List<ArticleCategory>> GetArticleCategoryByUserId(string UserId);

        Task<ArticleCategory> GetArticleCategoryByUrl(string Url);

        Task<List<ArticleCategoryArticle>> GetLstArticleCatebyArticleId(int articleId);

        Task<List<ArticleCategoryAssign>> ArticleCategoryAssignsGetLstByUserId(string userId);

        Task ArticleCategoryAssignsUpdate(string userId, List<int> articleCategoryId);

        Task<List<SpArticleCategoryTreeResult>> ArticleCategoryTreeGetLst();

        Task<bool> ArticleCategoryInsertOrUpdate(ArticleCategory model);

        Task<bool> ArticleCategoryDelete(int id);

        Task<ArticleCategory> ArticleCategoryGetByArticleId(int articleId);
    }

    public class ArticleCategoryRepository : RepositoryBase<ArticleCategory>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }

        public async Task<List<ArticleCategoryAssign>> ArticleCategoryAssignsGetLstByUserId(string userId)
        {
            List<ArticleCategoryAssign> lstOutput = new();
            try
            {
                lstOutput = await CmsContext.ArticleCategoryAssign.Where(x => x.AspNetUsersId == userId).ToListAsync();
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task ArticleCategoryAssignsUpdate(string userId, List<int> articleCategoryId)
        {
            var listItem = new List<ArticleCategoryAssign>();
            var item = await CmsContext.ArticleCategoryAssign.Where(p => p.AspNetUsersId == userId).ToListAsync();
            if (item != null) // Update
            {
                CmsContext.ArticleCategoryAssign.RemoveRange(item);
                await CmsContext.SaveChangesAsync();
            }
            //Add
            foreach (var p in articleCategoryId)
            {
                var itemArtCate = new ArticleCategoryAssign();
                itemArtCate.AspNetUsersId = userId;
                itemArtCate.ArticleCategoryId = p;
                listItem.Add(itemArtCate);
            }
            await CmsContext.ArticleCategoryAssign.AddRangeAsync(listItem);
            await CmsContext.SaveChangesAsync();
        }

        public async Task<bool> ArticleCategoryDelete(int id)
        {
            var itemDelete = await CmsContext.ArticleCategory.FindAsync(id);
            if (itemDelete != null)
            {
                CmsContext.Entry(itemDelete).State = EntityState.Deleted;
                await CmsContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ArticleCategory> ArticleCategoryGetByArticleId(int articleId)
        {
            var item = await CmsContext.ArticleCategoryArticle.FirstOrDefaultAsync(p => p.ArticleId == articleId);
            if (item != null)
            {
                return await CmsContext.ArticleCategory.FirstOrDefaultAsync(p => p.Id == item.ArticleCategoryId);
            }
            else
            {
                return new ArticleCategory();
            }

        }

        public async Task<ArticleCategory> ArticleCategoryGetById(int articleCategoryId)
        {
            ArticleCategory item = new();
            try
            {
                item = await CmsContext.ArticleCategory.FirstOrDefaultAsync(p => p.Id == articleCategoryId);
            }
            catch (Exception)
            {

            }
            return item;
        }

        public async Task<bool> ArticleCategoryInsertOrUpdate(ArticleCategory model)
        {
            try
            {
                model.Url = FormatURL(model.Name);
                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<SpArticleCategoryTreeResult>> ArticleCategoryTreeGetLst()
        {
            List<SpArticleCategoryTreeResult> output = new();
            try
            {
                output = await CmsContext.Procedures.SpArticleCategoryTreeAsync(null, null);
            }
            catch
            {
                return output;
            }
            return output;
        }

        public async Task<List<ArticleCategory>> GetArticleCategoryById(int? ArticleCategoryId)
        {
            if (ArticleCategoryId != null)
            {
                return await CmsContext.ArticleCategory.Where(p => p.Id == ArticleCategoryId)
                    .ToListAsync();
            }
            else
            {
                return await CmsContext.ArticleCategory.ToListAsync();
            }
        }

        public async Task<ArticleCategory> GetArticleCategoryByUrl(string Url)
        {
            return await CmsContext.ArticleCategory.FirstOrDefaultAsync(p => p.Url == Url);
        }

        public async Task<List<ArticleCategory>> GetArticleCategoryByUserId(string UserId)
        {
            List<ArticleCategory> lstOutput = new();
            try
            {
                var lstCate = await CmsContext.ArticleCategoryAssign.Where(x => x.AspNetUsersId == UserId).Select(x => x.ArticleCategoryId).ToListAsync();
                if (lstCate != null)
                {
                    lstOutput = await CmsContext.ArticleCategory.Where(x => lstCate.Contains(x.Id)).ToListAsync();
                }

            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<List<ArticleCategoryArticle>> GetLstArticleCatebyArticleId(int articleId)
        {
            var lstArtCate = new List<ArticleCategoryArticle>();
            try
            {
                lstArtCate = await CmsContext.ArticleCategoryArticle.Where(p => p.ArticleId == articleId).AsNoTracking().ToListAsync();
            }
            catch
            {
            }
            return lstArtCate;
        }
    }
}