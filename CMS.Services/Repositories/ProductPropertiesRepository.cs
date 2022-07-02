namespace CMS.Services.Repositories
{
    public interface IProductPropertiesRepository : IRepositoryBase<ProductProperty>
    {
        Task<List<ProductPropertyValue>> ProductPropertyValuesGetLstByProductId(int productId);

        Task<List<ProductPropertyCategory>> ProductPropertyCategoryGetLstByCategoryId(int productCategoryId);

        Task<List<ProductProperty>> ProductPropertyGetLstByCategoryId(int productCategoryId);

        Task<List<SpGetLstProductPropertiesResult>> ProductPropertiesSpGetLst(int productId, int productPropertyCategory);

        Task<List<SpProductPropertyCategoryTreeResult>> ProductPropertiesSpGetTreeLst(int productCategoryId);

        Task<bool> ProductPropertyValueInsertOrUpdate(ProductPropertiesDTO model, string userId, int productId);

        Task<bool> ProductPropertyCategoryInsertOrUpdate(ProductPropertyCategory model);

        Task<bool> ProductPropertyCategoryDelete(int id);

        Task<bool> ProductPropertyInsertOrUpdate(ProductProperty model);

        Task<bool> ProductPropertyDelete(int id);

        Task<List<ProductPropertyType>> ProductPropertyTypeGetLst();
    }
    public class ProductPropertiesRepository : RepositoryBase<ProductProperty>, IProductPropertiesRepository
    {
        public ProductPropertiesRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }

        public async Task<List<SpGetLstProductPropertiesResult>> ProductPropertiesSpGetLst(int productId, int productPropertyCategory)
        {
            List<SpGetLstProductPropertiesResult> lstOutput = new();
            try
            {
                lstOutput = await CmsContext.Procedures.SpGetLstProductPropertiesAsync(
                    productId,
                    productPropertyCategory
                    );
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<List<SpProductPropertyCategoryTreeResult>> ProductPropertiesSpGetTreeLst(int productCategoryId)
        {
            List<SpProductPropertyCategoryTreeResult> output = new();
            try
            {
                output = await CmsContext.Procedures.SpProductPropertyCategoryTreeAsync(productCategoryId, null);
            }
            catch
            {

            }
            return output;
        }

        public async Task<bool> ProductPropertyCategoryDelete(int id)
        {
            var itemDelete = await CmsContext.ProductPropertyCategory.FindAsync(id);
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

        public async Task<List<ProductPropertyCategory>> ProductPropertyCategoryGetLstByCategoryId(int productCategoryId)
        {
            List<ProductPropertyCategory> lstOutput = new();
            try
            {
                List<int> lstParentChildCategory = new();
                var currentCate = await CmsContext.ProductCategory.FindAsync(productCategoryId);
                if(currentCate.ParentId !=null)
                {
                    lstParentChildCategory.Add((int)currentCate.ParentId);
                    lstParentChildCategory.Add(productCategoryId);
                }
                foreach (var item in lstParentChildCategory)
                {
                    var lstResult = await CmsContext.ProductPropertyCategory.Where(x => x.ProductCategoryId == item).ToListAsync();
                    if (lstResult != null)
                    {
                        foreach (var p in lstResult)
                        {
                            lstOutput.Add(p);
                        }
                    }
                }
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<bool> ProductPropertyCategoryInsertOrUpdate(ProductPropertyCategory model)
        {
            try
            {
                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ProductPropertyDelete(int id)
        {
            var itemDelete = await CmsContext.ProductProperty.FindAsync(id);
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

        public async Task<List<ProductProperty>> ProductPropertyGetLstByCategoryId(int productCategoryId)
        {
            List<ProductProperty> lstOutput = new();
            try
            {
                lstOutput = await CmsContext.ProductProperty.Where(x => x.ProductCategoryId == productCategoryId).ToListAsync();
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<bool> ProductPropertyValueInsertOrUpdate(ProductPropertiesDTO model, string userId, int productId)
        {
            try
            {
                foreach (var item in model.lstProductPropertyCate)
                {
                    foreach (var p in item.lstProductProperties)
                    {
                        if (p.ProductPropertyValueId != null) // Update
                        {
                            var itemUpdate = await CmsContext.ProductPropertyValue.FindAsync(p.ProductPropertyValueId);
                            if (itemUpdate != null)
                            {
                                itemUpdate.Value = p.Value;
                                itemUpdate.LastEditBy = userId;
                                itemUpdate.LastEditDate = DateTime.Now;
                                CmsContext.Entry(itemUpdate).State = EntityState.Modified;
                                await CmsContext.SaveChangesAsync();
                            }

                        }
                        else //Insert
                        {
                            ProductPropertyValue itemInsert = new();
                            itemInsert.ProductId = productId;
                            itemInsert.ProductPropertyId = p.Id;
                            itemInsert.Value = p.Value;
                            itemInsert.CreateBy = userId;
                            itemInsert.CreateDate = DateTime.Now;
                            itemInsert.LastEditBy = userId;
                            itemInsert.LastEditDate = DateTime.Now;
                            CmsContext.ProductPropertyValue.Add(itemInsert);
                            await CmsContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;

        }

        public async Task<bool> ProductPropertyInsertOrUpdate(ProductProperty model)
        {
            try
            {
                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<ProductPropertyValue>> ProductPropertyValuesGetLstByProductId(int productId)
        {
            List<ProductPropertyValue> lstOutput = new();
            try
            {
                lstOutput = await CmsContext.ProductPropertyValue.Where(x => x.ProductId == productId).ToListAsync();
            }
            catch
            {

            }
            return lstOutput;
        }

        public async Task<bool> ProductPropertyValueUpdate(List<ProductPropertyValue> model, string userId)
        {
            try
            {
                foreach (var p in model)
                {
                    if (p.Id == 0)
                    {
                        p.CreateBy = userId;
                        p.CreateDate = DateTime.Now;
                        p.LastEditBy = userId;
                        p.LastEditDate = DateTime.Now;
                        CmsContext.Entry(p).State = EntityState.Added;
                        await CmsContext.SaveChangesAsync();
                    }
                    else
                    {
                        var item = CmsContext.ProductProperty.Find(p.Id);
                        if (item != null)
                        {
                            p.LastEditBy = userId;
                            p.LastEditDate = DateTime.Now;
                            CmsContext.Entry(item).CurrentValues.SetValues(p);
                            await CmsContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<ProductPropertyType>> ProductPropertyTypeGetLst()
        {
            return await CmsContext.ProductPropertyType.ToListAsync();
        }
    }
}
