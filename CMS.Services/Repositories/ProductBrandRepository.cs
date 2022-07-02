using CMS.Common;

namespace CMS.Services.Repositories
{
    public interface IProductBrandRepository : IRepositoryBase<ProductBrand>
    {
        Task<List<ProductBrand>> ProductBrandGetLst();

        Task<ProductBrand> ProductBrandById(int productBrandId);

        Task<ProductBrand> ProductBrandByUrl(string url);
        Task<int> ProductBrandInsert(ProductBrand model, string userId);

        Task ProductBrandUpdate(ProductBrand model, string userId);

        Task<string> CreateProductBrandURL(int productBrandId);

        Task<bool> ProductBrandGetFollowByUserId(int productBrandId, string userId);

        Task<bool> ProductBrandFollowInsertOrDelete(bool isFollow, ProductBrandFollow model);

        Task<bool> ProductBrandFollowInsert(int productBrandId, string userId);
        Task<bool> ProductBrandFollowDelete(int productBrandId, string userId);
        Task<int> ProductBrandFollowCountByBrandId(int productBrandId, DateTime FromDate, DateTime ToDate);
        Task<List<ProductBrandStatus>> ProductBrandStatusGetLst();

        Task<VirtualizeResponse<SpProductBrandSearchResult>> ProductBrandSearchWithPaging(ProductBrandSearchFilter model);

        Task ProductBrandUpdateStatusType(string userId, int ProductBrandId, int StatusTypeId);

        Task ProductBrandIncreaseViewCount(int productBrandId);
        Task ProductBrandIncreaseViewPageCount(int productBrandId);

        Task<List<ProductBrandAttachFile>> ProductBrandAttachGetLstByProductBrandId(int productBrandId);

        Task<bool> ProductBrandAttachDelete(int productBrandAttachFileId);

        Task<bool> ProductBrandAttachInsert(List<ProductBrandAttachFile> model);

        Task<int> ProductBrandCountByDateQry(DateTime FromDate, DateTime ToDate);
    }
    public class ProductBrandRepository : RepositoryBase<ProductBrand>, IProductBrandRepository
    {
        public ProductBrandRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }
        public async Task<int> ProductBrandCountByDateQry(DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.ProductBrand.Where(x => x.CreateDate >= FromDate && x.CreateDate <= ToDate).CountAsync();
        }
        public async Task<ProductBrand> ProductBrandById(int productBrandId)
        {
            return await CmsContext.ProductBrand.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productBrandId);
        }

        public async Task<List<ProductBrand>> ProductBrandGetLst()
        {
            return await CmsContext.ProductBrand.ToListAsync();
        }



        public async Task<int> ProductBrandInsert(ProductBrand model, string userId)
        {
            model.ProductBrandCategoryId = 1; // Default;
            model.ProductBrandTypeId = 1;
            model.ProductBrandLevelId = 1;
            model.DepartmentManId = 1;
            model.ProductBrandModelManagementId = 1;
            model.ProductBrandStatusId = 1;
            model.CountryId = 1;
            model.Code = String.Empty;
            model.ViewCount = 0;
            model.ViewPageCount = 0;
            model.FollowCount = 0;
            model.SellCount = 0;
            model.QrcodePublic = Utils.GetRandomText(6);
            model.CreateBy = userId;
            model.CreateDate = DateTime.Now;
            model.LastEditDate = DateTime.Now;
            model.LastEditBy = userId;
            CmsContext.ProductBrand.Add(model);
            await CmsContext.SaveChangesAsync();
            return model.Id;
        }


        public async Task ProductBrandUpdate(ProductBrand model, string userId)
        {
            try
            {
                var items = await CmsContext.ProductBrand.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (items != null)
                {
                    model.LastEditBy = userId;
                    model.LastEditDate = DateTime.Now;
                    if (string.IsNullOrEmpty(items.Url))
                    {
                        model.Url = await CreateProductBrandURL(items.Id);
                    }
                    CmsContext.Entry(items).CurrentValues.SetValues(model);
                    CmsContext.Entry(items).State = EntityState.Modified;
                    await CmsContext.SaveChangesAsync();

                }
            }
            catch
            {
            }
        }

        public async Task<string> CreateProductBrandURL(int productBrandId)
        {
            try
            {
                var currentProduct = await CmsContext.ProductBrand.FirstOrDefaultAsync(p => p.Id == productBrandId);
                return FormatURL(currentProduct?.Name) + "-" + productBrandId.ToString();
            }
            catch
            {
            }
            return "nourl";
        }

        public async Task<bool> ProductBrandGetFollowByUserId(int productBrandId, string userId)
        {
            var returnResult = false;
            var item = await CmsContext.ProductBrandFollow.CountAsync(x => x.ProductBrandId == productBrandId && x.CreateBy == userId);
            if (item > 0)
            {
                returnResult = true;
            }
            return returnResult;
        }

        public async Task<bool> ProductBrandFollowInsertOrDelete(bool isFollow, ProductBrandFollow model)
        {
            bool returnResult = false;


            if (isFollow)// Delete
            {
                var item = await CmsContext.ProductBrandFollow.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (item != null)
                {
                    CmsContext.Entry(item).State = EntityState.Deleted;
                    await CmsContext.SaveChangesAsync();

                    var productBrandItem = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == model.ProductBrandId);
                    if (productBrandItem != null)
                    {
                        if (productBrandItem.FollowCount > 0)
                        {
                            productBrandItem.FollowCount -= 1;
                            await CmsContext.SaveChangesAsync();
                        }
                    }
                    returnResult = true;
                }

            }
            else // Insert
            {
                CmsContext.Entry(model).State = EntityState.Added;
                await CmsContext.SaveChangesAsync();
                var productBrandItem = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == model.ProductBrandId);
                if (productBrandItem != null)
                {
                    productBrandItem.FollowCount += 1;
                    await CmsContext.SaveChangesAsync();
                }
                returnResult = true;
            }

            return returnResult;
        }

        public async Task<bool> ProductBrandFollowInsert(int productBrandId, string userId)
        {
            bool returnResult = false;
            try
            {
                ProductBrandFollow model = new();
                model.ProductBrandId = productBrandId;
                model.CreateBy = userId;
                model.CreateDate = DateTime.Now;
                CmsContext.ProductBrandFollow.Add(model);
                await CmsContext.SaveChangesAsync();

                var productBrandItem = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == productBrandId);
                if (productBrandItem != null)
                {
                    productBrandItem.FollowCount += 1;
                    await CmsContext.SaveChangesAsync();
                }
                returnResult = true;
            }
            catch
            {
                return returnResult;
            }
            return returnResult;
        }

        public async Task<bool> ProductBrandFollowDelete(int productBrandId, string userId)
        {
            bool returnResult = false;
            var item = await CmsContext.ProductBrandFollow.FirstOrDefaultAsync(x => x.ProductBrandId == productBrandId && x.CreateBy == userId);
            if (item != null)
            {
                CmsContext.ProductBrandFollow.Remove(item);
                await CmsContext.SaveChangesAsync();

                var productBrandItem = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == productBrandId);
                if (productBrandItem != null)
                {
                    if (productBrandItem.FollowCount > 0)
                    {
                        productBrandItem.FollowCount -= 1;
                        await CmsContext.SaveChangesAsync();
                    }
                }
                returnResult = true;
            }
            return returnResult;
        }

        public async Task<ProductBrand> ProductBrandByUrl(string url)
        {
            return await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Url == url);
        }

        public async Task<List<ProductBrandStatus>> ProductBrandStatusGetLst()
        {
            return await CmsContext.ProductBrandStatus.ToListAsync();
        }

        public async Task<VirtualizeResponse<SpProductBrandSearchResult>> ProductBrandSearchWithPaging(ProductBrandSearchFilter model)
        {
            var output = new VirtualizeResponse<SpProductBrandSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();

            var result = await CmsContext.GetProcedures().SpProductBrandSearchAsync(
            model.Keyword,
            model.ProductBrandCategoryId,
            model.ProductBrandStatusId,
            model.ProductBrandTypeId,
            model.DepartmentManId,
            model.CountryId,
            model.LocationId,
            model.DistrictId,
            model.WardId,
            model.ExceptionId,
            model.FromDate,
            model.ToDate,
            model.Efficiency,
            model.Active,
            model.CreateBy,
            model.PageSize,
            model.CurrentPage, itemCounts, returnValues
            );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;
            return output;
        }

        public async Task ProductBrandUpdateStatusType(string userId, int ProductBrandId, int StatusTypeId)
        {
            var productBrand = await CmsContext.ProductBrand.FirstOrDefaultAsync(p => p.Id == ProductBrandId);
            if (productBrand != null)
            {
                if (StatusTypeId == 2 || StatusTypeId == 3) // Kiểm tra
                {
                    productBrand.Checked = 1;
                    productBrand.CheckBy = userId;
                    productBrand.CheckDate = DateTime.Now;
                }
                else if (StatusTypeId == 4) //Duyệt
                {
                    productBrand.Approved = 1;
                    productBrand.ApproveBy = userId;
                    productBrand.ApproveDate = DateTime.Now;
                }
                else if (StatusTypeId == 0) //Từ chối đăng
                {
                    productBrand.Approved = 0;
                    productBrand.ApproveBy = userId;
                    productBrand.ApproveDate = DateTime.Now;
                }
                productBrand.ProductBrandStatusId = StatusTypeId;
                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task<int> ProductBrandFollowCountByBrandId(int productBrandId, DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.ProductBrandFollow.CountAsync(x => x.ProductBrandId == productBrandId && x.CreateDate >= FromDate && x.CreateDate <= ToDate); 
        }


        public async Task ProductBrandIncreaseViewCount(int productBrandId)
        {
            var prodBrand = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == productBrandId);
            if (prodBrand != null)
            {
                prodBrand.ViewCount += 1;

                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task ProductBrandIncreaseViewPageCount(int productBrandId)
        {
            var prodBrand = await CmsContext.ProductBrand.FirstOrDefaultAsync(x => x.Id == productBrandId);
            if (prodBrand != null)
            {

                prodBrand.ViewPageCount += 1;
                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProductBrandAttachFile>> ProductBrandAttachGetLstByProductBrandId(int productBrandId)
        {
            return await CmsContext.ProductBrandAttachFile.Where(p => p.ProductBrandId == productBrandId).ToListAsync();
        }

        public async Task<bool> ProductBrandAttachDelete(int productBrandAttachFileId)
        {
            var item = await CmsContext.ProductBrandAttachFile.FindAsync(productBrandAttachFileId);
            if (item != null)
            {
                CmsContext.ProductBrandAttachFile.Remove(item);
                await CmsContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ProductBrandAttachInsert(List<ProductBrandAttachFile> model)
        {
            try
            {
                foreach (var p in model)
                {
                    CmsContext.Entry(p).State = EntityState.Added;
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
