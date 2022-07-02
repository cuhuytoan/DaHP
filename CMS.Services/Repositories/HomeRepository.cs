namespace CMS.Services.Repositories
{
    public interface IHomeRepository : IRepositoryBase<Product>
    {

    }
    public class HomeRepository : RepositoryBase<Product>, IHomeRepository
    {
        public HomeRepository(CmsContext CmsContext) : base(CmsContext)
        {
        }
    }
}
