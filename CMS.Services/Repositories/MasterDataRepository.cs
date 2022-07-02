namespace CMS.Services.Repositories
{
    public interface IMasterDataRepository : IRepositoryBase<Bank>
    {
        Task<List<Department>> DepartmentsGetLst();

        Task<List<Country>> CountriesGetLst();

        Task<List<Location>> LocationGetLstByCountryId(int countryId);

        Task<List<Ward>> WardsGetLstByDistrictId(int districtId);

        Task<List<District>> DistrictsGetLstByLocationId(int locationId);

        Task<List<Bank>> BankGetLst();

        Task<List<Unit>> UnitGetLst();

        Task<Location> LocationGetById(int locationId);

        Task<Unit> UnitGetById(int unitId);

        Task<Country> CountryGetById(int countryId);

        Task<List<ProductOrderPaymentMethod>> PaymentMethodGetLst();

        Task<List<ProductBrandLevel>> ProductBrandLevelsGetlst();

        Task<ProductBrandLevel> ProductBrandLevelsGetById(int productBrandLevelId);
    }

    public class MasterDataRepository : RepositoryBase<Bank>, IMasterDataRepository
    {
        public MasterDataRepository(CmsContext CmsContext) : base(CmsContext)
        {
        }

        public async Task<List<Department>> DepartmentsGetLst()
        {
            return await CmsContext.Department.AsNoTracking().ToListAsync();
        }

        public async Task<List<Country>> CountriesGetLst()
        {
            return await CmsContext.Country.AsNoTracking().ToListAsync();
        }

        public async Task<List<Location>> LocationGetLstByCountryId(int countryId)
        {
            return await CmsContext.Location.Where(p => p.CountryId == countryId).AsNoTracking().ToListAsync();
        }

        public async Task<List<Ward>> WardsGetLstByDistrictId(int districtId)
        {
            return await CmsContext.Ward.Where(p => p.DistrictId == districtId).AsNoTracking().ToListAsync();
        }

        public async Task<List<District>> DistrictsGetLstByLocationId(int locationId)
        {
            return await CmsContext.District.Where(p => p.LocationId == locationId).AsNoTracking().ToListAsync();
        }

        public async Task<List<Bank>> BankGetLst()
        {
            return await CmsContext.Bank.AsNoTracking().ToListAsync();
        }

        public async Task<List<Unit>> UnitGetLst()
        {
            return await CmsContext.Unit.AsNoTracking().ToListAsync();
        }

        public async Task<Location> LocationGetById(int locationId)
        {
            return await CmsContext.Location.AsNoTracking().FirstOrDefaultAsync(p => p.Id == locationId);
        }

        public async Task<Unit> UnitGetById(int unitId)
        {
            return await CmsContext.Unit.AsNoTracking().FirstOrDefaultAsync(p => p.Id == unitId);
        }

        public async Task<Country> CountryGetById(int countryId)
        {
            return await CmsContext.Country.AsNoTracking().FirstOrDefaultAsync(p => p.Id == countryId);
        }

        public async Task<List<ProductOrderPaymentMethod>> PaymentMethodGetLst()
        {
            return await CmsContext.ProductOrderPaymentMethod.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductBrandLevel>> ProductBrandLevelsGetlst()
        {
            return await CmsContext.ProductBrandLevel.AsNoTracking().ToListAsync();
        }

        public async Task<ProductBrandLevel> ProductBrandLevelsGetById(int productBrandLevelId)
        {
            return await CmsContext.ProductBrandLevel.FirstOrDefaultAsync(x => x.Id == productBrandLevelId);
        }
    }
}