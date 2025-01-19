using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class TourProblemReportRepository : CrudDatabaseRepository<TourProblemReport, StakeholdersContext>, ITourProblemReportRepository
    {
        private readonly StakeholdersContext _context;

        public TourProblemReportRepository(StakeholdersContext context) : base(context)
        {
            _context = context;
        }

        public new TourProblemReport? Get(int id)
        {
            return DbContext.TourProblemReports.Where(t => t.Id == id)
                .FirstOrDefault();
        }

        // Get a list of Tour Problems by Tourist Id
        public PagedResult<TourProblemReport> GetByTouristId(int touristId, int page, int pageSize)
        {
            var task = _context.TourProblemReports
                .Where(tpr => tpr.TouristId == touristId)
                .GetPagedById(page, pageSize);

            task.Wait();
            return task.Result;
        }

        public PagedResult<TourProblemReport> GetPaged(int page, int pageSize)
        {
            var totalCount = _context.TourProblemReports.Count();
            var reports = _context.TourProblemReports.GetPagedById(page, pageSize);
            return reports.Result;
        }
    }
}
