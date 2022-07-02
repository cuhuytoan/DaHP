namespace CMS.Services.Repositories
{
    public interface IArticleBlockRepository : IRepositoryBase<ArticleBlock>
    {
        Task<List<ArticleBlock>> ArticleBlockGetAll();

        Task<List<SpArticleSearchResult>> ArticleBlockArticleGetLstByArticleBlockId(int articleBlockId);

        Task<bool> ArticleBlockArticleInsert(List<ArticleBlockArticle> model);

        Task<bool> ArticleBlockArticleDeleteById(int articleBlockId, int articleId);

    }

    public class ArticleBlockRepository : RepositoryBase<ArticleBlock>, IArticleBlockRepository
    {
        public ArticleBlockRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }

        public async Task<bool> ArticleBlockArticleDeleteById(int articleBlockId, int articleId)
        {
            try
            {
                var item = await CmsContext.ArticleBlockArticle.FirstOrDefaultAsync(x => x.ArticleId == articleId && x.ArticleBlockId == articleBlockId);
                if (item != null)
                {
                    CmsContext.ArticleBlockArticle.Remove(item);
                    await CmsContext.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<SpArticleSearchResult>> ArticleBlockArticleGetLstByArticleBlockId(int articleBlockId)
        {
            List<SpArticleSearchResult> lstOutput = new List<SpArticleSearchResult>();
            var lstArticle = await CmsContext.ArticleBlockArticle.Where(x => x.ArticleBlockId == articleBlockId).AsNoTracking().ToListAsync();
            if (lstArticle != null)
            {
                foreach (var p in lstArticle)
                {
                    SpArticleSearchResult item = new SpArticleSearchResult();
                    var article = await CmsContext.Article.FirstOrDefaultAsync(x => x.Id == p.ArticleId);
                    if (article != null)
                    {
                        item.Id = article.Id;
                        item.Image = article.Image;
                        item.Name = article.Name;
                        item.CreateDate = article.CreateDate;
                        item.URL = article.Url;
                        item.Description = article.Description;
                        if (Int32.TryParse(item.ArticleCategoryIds, out int _cate))
                        {
                            var cate = await CmsContext.ArticleCategory.FirstOrDefaultAsync(x => x.Id == _cate);
                            item.ArticleCategoryName = cate.Name;
                        }
                    }
                    lstOutput.Add(item);
                }

            }
            return lstOutput;
        }

        public async Task<bool> ArticleBlockArticleInsert(List<ArticleBlockArticle> model)
        {
            try
            {
                foreach (var p in model)
                {
                    var item = await CmsContext.ArticleBlockArticle.FirstOrDefaultAsync(x => x.ArticleId == p.ArticleId && x.ArticleBlockId == p.ArticleBlockId);
                    if (item != null) continue;
                    else
                    {
                        CmsContext.ArticleBlockArticle.Add(p);
                        await CmsContext.SaveChangesAsync();
                    }
                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<ArticleBlock>> ArticleBlockGetAll()
        {
            return await CmsContext.ArticleBlock.OrderBy(x => x.Sort).ToListAsync();
        }
    }
}
