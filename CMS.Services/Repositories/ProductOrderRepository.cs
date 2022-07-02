namespace CMS.Services.Repositories
{
    public interface IProductOrderRepository : IRepositoryBase<ProductOrder>
    {
        Task<int> ProductOrderInsertOrUpdate(ProductOrder model);

        Task<bool> ProductOrderDeleteById(int id);

        Task<ProductOrder> ProductOrderGetById(int id);
        Task<List<ProductOrder>> ProductOrderGetLstByCode(string productOrderCode);

        Task<bool> ProductOrderUpdatePaymentStatus(string productOrderCode, int statusId);

        Task<bool> ProductOrderDetailInsertOrUpdate(List<ProductOrderDetail> lstModel);

        Task<bool> ProductOrderDetailDeleteById(int id);

        Task<ProductOrderDetail> ProductOrderDetailGetById(int id);
        Task<List<ProductOrderDetail>> ProductOrderDetailGetLstByOrderId(int productOrderId);

        Task<VirtualizeResponse<SpProductOrderSearchResult>> ProductOrderSearch(ProductOrderSearchFilter model);

        Task<int> ProductOrderCountIntTab(int productOrderStatusId, string userId);

    }
    public class ProductOrderRepository : RepositoryBase<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(CmsContext CmsContext) : base(CmsContext)
        {
        }

        public async Task<int> ProductOrderCountIntTab(int productOrderStatusId, string userId)
        {
            if (productOrderStatusId == 0) //Tab all
            {
                return await CmsContext.ProductOrder.CountAsync(x => x.CreateBy == userId);
            }
            return await CmsContext.ProductOrder.CountAsync(x => x.CreateBy == userId && x.ProductOrderStatusId == productOrderStatusId);
        }

        public async Task<bool> ProductOrderDeleteById(int id)
        {
            var item = await CmsContext.ProductOrder.FindAsync(id);
            if (item != null)
            {
                try
                {
                    CmsContext.Entry(item).State = EntityState.Deleted;
                    await CmsContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ProductOrderDetailDeleteById(int id)
        {
            var item = await CmsContext.ProductOrderDetail.FindAsync(id);
            if (item != null)
            {
                try
                {
                    CmsContext.Entry(item).State = EntityState.Deleted;
                    await CmsContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<ProductOrderDetail> ProductOrderDetailGetById(int id)
        {
            return await CmsContext.ProductOrderDetail.FindAsync(id);
        }

        public async Task<List<ProductOrderDetail>> ProductOrderDetailGetLstByOrderId(int productOrderId)
        {
            List<ProductOrderDetail> lstProductOrderDetail = new();
            try
            {
                lstProductOrderDetail = await CmsContext.ProductOrderDetail.Where(x => x.ProductOrderId == productOrderId).ToListAsync();
            }
            catch
            {

            }
            return lstProductOrderDetail;
        }

        public async Task<bool> ProductOrderDetailInsertOrUpdate(List<ProductOrderDetail> lstModel)
        {
            try
            {
                foreach (var model in lstModel)
                {
                    CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                    await CmsContext.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ProductOrder> ProductOrderGetById(int id)
        {
            return await CmsContext.ProductOrder.FindAsync(id);
        }

        public async Task<List<ProductOrder>> ProductOrderGetLstByCode(string productOrderCode)
        {
            return await CmsContext.ProductOrder.Where(x => x.ProductOrderCode == productOrderCode).ToListAsync();
        }

        public async Task<int> ProductOrderInsertOrUpdate(ProductOrder model)
        {
            try
            {
                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
                return model.Id;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<VirtualizeResponse<SpProductOrderSearchResult>> ProductOrderSearch(ProductOrderSearchFilter model)
        {
            var output = new VirtualizeResponse<SpProductOrderSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();
            try
            {
                var result = await CmsContext.GetProcedures().SpProductOrderSearchAsync(
                model.ProductOrderId,
                model.CreateBy,
                model.ProductOrderStatusId,
                model.OrderBy,
                model.PageSize,
                model.CurrentPage, itemCounts, returnValues
                );
                output.Items = result.ToList();
                output.TotalSize = (int)itemCounts.Value;
            }
            catch (Exception)
            {

            }
            return output;
        }

        public async Task<bool> ProductOrderUpdatePaymentStatus(string productOrderCode, int statusId)
        {
            var lstProductOrder = await CmsContext.ProductOrder.Where(x => x.ProductOrderCode == productOrderCode).ToListAsync();
            if(lstProductOrder !=null)
            {
                lstProductOrder.ForEach(x =>
                {
                    x.ProductOrderPaymentStatusId = statusId;
                    x.LastEditDate = DateTime.Now;
                });
                CmsContext.ProductOrder.UpdateRange(lstProductOrder);
                await CmsContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
