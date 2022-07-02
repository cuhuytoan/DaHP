namespace CMS.Services.Repositories
{
    public interface ILogVisitRepository : IRepositoryBase<LogVisit>
    {
        Task LogVisitInsert(LogVisit model);

        Task<VirtualizeResponse<SpLogVisitSearchResult>> lstLogVisitPaging(int page, int pageSize);

        Task<int> LogVisitCountByDateQry(DateTime FromDate, DateTime ToDate);
    }

    public class LogVisitRepository : RepositoryBase<LogVisit>, ILogVisitRepository
    {
        public LogVisitRepository(CmsContext CmsContext) : base(CmsContext)
        {
        }
        public async Task<int> LogVisitCountByDateQry(DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.LogVisit.Where(x => x.CreateDate >= FromDate && x.CreateDate <= ToDate).CountAsync();
        }
        public async Task LogVisitInsert(LogVisit model)
        {
            model.CreateDate = DateTime.Now;
            CmsContext.LogVisit.Add(model);
            await CmsContext.SaveChangesAsync();

        }

        public async Task<VirtualizeResponse<SpLogVisitSearchResult>> lstLogVisitPaging(int page, int pageSize)
        {
            var output = new VirtualizeResponse<SpLogVisitSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();

            var result = await CmsContext.GetProcedures().SpLogVisitSearchAsync(
            pageSize,
            page, itemCounts, returnValues
            );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;
            return output;
        }
    }
}