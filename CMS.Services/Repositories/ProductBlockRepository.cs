namespace CMS.Services.Repositories
{
    public interface IProductBlockRepository : IRepositoryBase<ProductBlock>
    {
        Task<List<ProductBlock>> ProductBlockGetAll();

        Task<List<SpProductSearchResult>> ProductBlockProductGetLstByProductBlockId(int productBlockId);

        Task<bool> ProductBlockProductInsert(List<ProductBlockProduct> model);

        Task<bool> ProductBlockProductDeleteById(int productBlockId, int productId);

    }

    public class ProductBlockRepository : RepositoryBase<ProductBlock>, IProductBlockRepository
    {
        public ProductBlockRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }

        public async Task<bool> ProductBlockProductDeleteById(int productBlockId, int productId)
        {
            try
            {
                var item = await CmsContext.ProductBlockProduct.FirstOrDefaultAsync(x => x.ProductId == productId && x.ProductBlockId == productBlockId);
                if (item != null)
                {
                    CmsContext.ProductBlockProduct.Remove(item);
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

        public async Task<List<SpProductSearchResult>> ProductBlockProductGetLstByProductBlockId(int productBlockId)
        {
            List<SpProductSearchResult> lstOutput = new List<SpProductSearchResult>();
            var lstProduct = await CmsContext.ProductBlockProduct.Where(x => x.ProductBlockId == productBlockId).AsNoTracking().ToListAsync();
            if (lstProduct != null)
            {
                foreach (var p in lstProduct)
                {
                    SpProductSearchResult item = new SpProductSearchResult();
                    var product = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == p.ProductId);
                    if (product != null)
                    {
                        item.Id = product.Id;
                        item.Image = product.Image;
                        item.Name = product.Name;
                        item.CreateDate = product.CreateDate;
                        item.URL = product.Url;
                        item.Price = product.Price;
                        item.PriceOld = product.PriceOld;
                        if (Int32.TryParse(item.ProductCategoryIds, out int _cate))
                        {
                            var cate = await CmsContext.ProductCategory.FirstOrDefaultAsync(x => x.Id == _cate);
                            item.ProductCategoryName = cate.Name;
                        }
                    }
                    lstOutput.Add(item);
                }

            }
            return lstOutput;
        }

        public async Task<bool> ProductBlockProductInsert(List<ProductBlockProduct> model)
        {
            try
            {
                foreach (var p in model)
                {
                    var item = await CmsContext.ProductBlockProduct.FirstOrDefaultAsync(x => x.ProductId == p.ProductId && x.ProductBlockId == p.ProductBlockId);
                    if (item != null) continue;
                    else
                    {
                        CmsContext.ProductBlockProduct.Add(p);
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

        public async Task<List<ProductBlock>> ProductBlockGetAll()
        {
            return await CmsContext.ProductBlock.ToListAsync();
        }
    }
}
