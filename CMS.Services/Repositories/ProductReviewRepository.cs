namespace CMS.Services.Repositories
{
    public interface IProductReviewRepository : IRepositoryBase<ProductReview>
    {
        Task<int?> ProductReviewInsert(ProductReview model, string userId);

        Task ProductReviewUpdate(ProductReview model, string userId);

        Task ProductReviewUpdateStatus(int productReviewId, bool active, string userId);

        Task ProductReviewDelete(int ProductReviewId);

        Task<ProductReview> ProductReviewGetById(int ProductReviewId);

        Task<List<ProductReview>> ProductReviewGetByUserId(int productId, string userId);

        Task<VirtualizeResponse<SpProductReviewSearchResult>> ProductReviewSearchWithPaging(ProductReviewSearchFilter model);

        Task<ProductReviewAverageDTO> ProductReviewGetAverage(int productId);
        Task<int> ProductReviewCountByProductBrandId(int productBrandId, DateTime FromDate, DateTime ToDate);

        Task<int> ProductReviewCountByDateQry(DateTime FromDate, DateTime ToDate);

    }
    public class ProductReviewRepository : RepositoryBase<ProductReview>, IProductReviewRepository
    {
        public ProductReviewRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }
        public async Task<int> ProductReviewCountByDateQry(DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.ProductReview.Where(x => x.CreateDate >= FromDate && x.CreateDate <= ToDate).CountAsync();
        }
        public async Task<int> ProductReviewCountByProductBrandId(int productBrandId, DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.ProductReview.CountAsync(x => x.ProductBrandId == productBrandId && x.Active == true && x.CreateDate >= FromDate && x.CreateDate <= ToDate);
        }

        public async Task ProductReviewDelete(int ProductReviewId)
        {
            try
            {
                var items = await CmsContext.ProductReview.FirstOrDefaultAsync(p => p.Id == ProductReviewId);
                if (items != null)
                {
                    CmsContext.ProductReview.Remove(items);
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        public async Task<ProductReviewAverageDTO> ProductReviewGetAverage(int productId)
        {
            ProductReviewAverageDTO result = new();
            var lstProductReview = await CmsContext.ProductReview.Where(x => x.ProductId == productId && x.Active == true).ToListAsync();
            if (lstProductReview != null && lstProductReview.Count > 0)
            {
                result.TotalProductReview = lstProductReview.Count();
                result.AveragePointReview = (int)lstProductReview.Average(x => x.Star);

            }

            return result;

        }

        public async Task<ProductReview> ProductReviewGetById(int ProductReviewId)
        {
            return await CmsContext.ProductReview.FirstOrDefaultAsync(p => p.Id == ProductReviewId);
        }

        public async Task<List<ProductReview>> ProductReviewGetByUserId(int productId, string userId)
        {
            return await CmsContext.ProductReview.Where(x => x.ProductId == productId && x.CreateBy == userId).ToListAsync();
        }

        public async Task<int?> ProductReviewInsert(ProductReview model, string userId)
        {
            model.ProductReviewTypeId = 1;
            model.CreateBy = userId;
            model.CreateDate = DateTime.Now;
            model.LastEditDate = DateTime.Now;
            model.LastEditBy = userId;
            model.Active = false; // Default
            CmsContext.ProductReview.Add(model);
            await CmsContext.SaveChangesAsync();
            return model.Id;
        }

        public async Task<VirtualizeResponse<SpProductReviewSearchResult>> ProductReviewSearchWithPaging(ProductReviewSearchFilter model)
        {
            var output = new VirtualizeResponse<SpProductReviewSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();
            try
            {
                var result = await CmsContext.GetProcedures().SpProductReviewSearchAsync(
                model.Keyword,
                model.ProductReviewTypeId,
                model.LocationId,
                model.DepartmentManId,
                model.ProductBrandId,
                model.ProductId,
                model.Active,
                model.FromDate,
                model.ToDate,
                model.CreateBy,
                model.PageSize,
                model.CurrentPage, itemCounts, returnValues
                );
                output.Items = result.ToList();
                output.TotalSize = (int)itemCounts.Value;
            }
            catch
            {

            }
            return output;
        }


        public async Task ProductReviewUpdate(ProductReview model, string userId)
        {
            try
            {
                var items = CmsContext.ProductReview.FirstOrDefault(p => p.Id == model.Id);
                if (items != null)
                {
                    model.LastEditBy = userId;
                    model.LastEditDate = DateTime.Now;
                    CmsContext.Entry(items).CurrentValues.SetValues(model);
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        public async Task ProductReviewUpdateStatus(int productReviewId, bool active, string userId)
        {
            var item = await CmsContext.ProductReview.FirstOrDefaultAsync(x => x.Id == productReviewId);
            if (item != null)
            {
                item.Active = active;
                item.LastEditBy = userId;
                item.LastEditDate = DateTime.Now;
                await CmsContext.SaveChangesAsync();
            }
        }
    }
}
