namespace CMS.Services.Repositories
{
    public interface IAdvertisingRepository : IRepositoryBase<Advertising>
    {
        Task<List<AdvertisingBlock>> AdvertisingBlockGetAll();

        Task<List<Advertising>> AdvertisingGetLstByBlockId(int adBlockId);

        Task<Advertising> AdvertisingGetById(int id);

        Task<bool> AdvertisingInsertOrUpdate(Advertising model);

        Task<bool> AdvertisingDeleteById(int id);
    }

    public class AdvertisingRepository : RepositoryBase<Advertising>, IAdvertisingRepository
    {
        public AdvertisingRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }

        public async Task<List<AdvertisingBlock>> AdvertisingBlockGetAll()
        {
            List<AdvertisingBlock> output = new();
            try
            {
                output = await CmsContext.AdvertisingBlock.Where(x => x.Active == true).OrderBy(x => x.Sort).ToListAsync();
            }
            catch
            {

            }
            return output;
        }

        public async Task<bool> AdvertisingDeleteById(int id)
        {
            var item = await CmsContext.Advertising.FindAsync(id);
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

        public async Task<Advertising> AdvertisingGetById(int id)
        {
            return await CmsContext.Advertising.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Advertising>> AdvertisingGetLstByBlockId(int adBlockId)
        {
            List<Advertising> output = new();
            try
            {
                output = await CmsContext.Advertising.Where(x => x.AdvertisingBlockId == adBlockId).OrderBy(x => x.Sort).ToListAsync();
            }
            catch
            {

            }
            return output;
        }

        public async Task<bool> AdvertisingInsertOrUpdate(Advertising model)
        {
            try
            {
                CmsContext.Entry(model).State = model.Id > 0 ? EntityState.Modified : EntityState.Added;
                await CmsContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}