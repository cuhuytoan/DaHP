namespace CMS.Services.Repositories
{
    public interface IProductCategoryRepository : IRepositoryBase<ProductCategory>
    {
        Task<List<ProductCategory>> GetProductCategoryById(int? ProductCategoryId);

        Task<List<ProductCategory>> GetProductCategoryByUserId(string UserId);

        Task<ProductCategory> GetProductCategoryByUrl(string Url);

        Task<List<ProductCategoryProduct>> GetLstProductCatebyProductId(int articleId);

        Task<List<ProductCategoryAssign>> ProductCategoryAssignsGetLstByUserId(string userId);

        Task<List<SpProductCategoryTreeResult>> ProductCategoryTreeGetLst();

        Task<bool> ProductCategoryInsertOrUpdate(ProductCategory model);


        Task<bool> ProductCategoryDelete(int id);

        Task<List<ProductCategory>> ProductCategoryGetLevel1();

        Task<List<ProductCategory>> GetProductCategoryChildByParentId(int ProductCategoryId);

        Task<ProductCategory> ProductCategoryGetByProductId(int productId);

        Task<List<ProductCategory>> ProductCategoryGetDistinctByProductBrandId(int productBrandId);

        

    }
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }
        public async Task<List<ProductCategoryAssign>> ProductCategoryAssignsGetLstByUserId(string userId)
        {
            List<ProductCategoryAssign> lstOutput = new();
            try
            {
                lstOutput = await CmsContext.ProductCategoryAssign.Where(x => x.AspNetUsersId == userId).ToListAsync();
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task ProductCategoryAssignsUpdate(string userId, List<int> articleCategoryId)
        {
            var listItem = new List<ProductCategoryAssign>();
            var item = await CmsContext.ProductCategoryAssign.Where(p => p.AspNetUsersId == userId).ToListAsync();
            if (item != null) // Update
            {
                CmsContext.ProductCategoryAssign.RemoveRange(item);
                await CmsContext.SaveChangesAsync();
            }
            //Add
            foreach (var p in articleCategoryId)
            {
                var itemArtCate = new ProductCategoryAssign
                {
                    AspNetUsersId = userId,
                    ProductCategoryId = p
                };
                listItem.Add(itemArtCate);
            }
            await CmsContext.ProductCategoryAssign.AddRangeAsync(listItem);
            await CmsContext.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoryById(int? ProductCategoryId)
        {
            if (ProductCategoryId != null)
            {
                return await CmsContext.ProductCategory.Where(p => p.Id == ProductCategoryId)
                    .ToListAsync();
            }
            else
            {
                return await CmsContext.ProductCategory.ToListAsync();
            }
        }

        public async Task<ProductCategory> GetProductCategoryByUrl(string Url)
        {
            return await CmsContext.ProductCategory.FirstOrDefaultAsync(p => p.Url == Url);
        }

        public async Task<List<ProductCategory>> GetProductCategoryByUserId(string UserId)
        {
            List<ProductCategory> lstOutput = new();
            try
            {
                var lstCate = await CmsContext.ProductCategoryAssign.Where(x => x.AspNetUsersId == UserId).Select(x => x.ProductCategoryId).ToListAsync();
                if (lstCate != null)
                {
                    lstOutput = await CmsContext.ProductCategory.Where(x => lstCate.Contains(x.Id)).ToListAsync();
                }

            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<List<ProductCategoryProduct>> GetLstProductCatebyProductId(int articleId)
        {
            var lstArtCate = new List<ProductCategoryProduct>();
            try
            {
                lstArtCate = await CmsContext.ProductCategoryProduct.Where(p => p.ProductId == articleId).AsNoTracking().ToListAsync();
            }
            catch
            {
            }
            return lstArtCate;
        }

        public async Task<List<SpProductCategoryTreeResult>> ProductCategoryTreeGetLst()
        {
            List<SpProductCategoryTreeResult> output = new();
            try
            {
                output = await CmsContext.Procedures.SpProductCategoryTreeAsync(null, null);
            }
            catch
            {
                return output;
            }
            return output;
        }
        public async Task<bool> ProductCategoryDelete(int id)
        {
            var itemDelete = await CmsContext.ProductCategory.FindAsync(id);
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

        public async Task<bool> ProductCategoryInsertOrUpdate(ProductCategory model)
        {
            try
            {
                if(String.IsNullOrEmpty(model.Url))
                {
                    model.Url = FormatURL(model.Name);
                }

                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<ProductCategory>> ProductCategoryGetLevel1()
        {
            return await CmsContext.ProductCategory.Where(x => x.ParentId == null && x.Id > 1 && x.Active == true).OrderBy(p => p.Sort).ToListAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoryChildByParentId(int ProductCategoryId)
        {

            return await CmsContext.ProductCategory.Where(p => p.ParentId == ProductCategoryId)
                .ToListAsync();

        }

        public async Task<ProductCategory> ProductCategoryGetByProductId(int productId)
        {
            var item = await CmsContext.ProductCategoryProduct.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (item != null)
            {
                return await CmsContext.ProductCategory.FirstOrDefaultAsync(x => x.Id == item.ProductCategoryId);
            }
            else
            {
                return new ProductCategory();
            }

        }

        public async Task<List<ProductCategory>> ProductCategoryGetDistinctByProductBrandId(int productBrandId)
        {
            var lstProductCategory = await CmsContext.Product.Where(x => x.ProductBrandId == productBrandId && x.ProductStatusId == 4)
                            .Select(z => z.ProductCategoryIds).Distinct().ToListAsync();
            if (lstProductCategory != null)
            {
                return await CmsContext.ProductCategory.Where(x => lstProductCategory.Contains(x.Id.ToString())).ToListAsync();
            }
            else
            {
                return new List<ProductCategory>();
            }
        }
    }
}
