using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain.TourExecutions;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        public TourReview Get(int id);
        public PagedResult<TourReview> GetReviewsForTour(int id, int pageIndex, int pageSize);
        public float GetAverageGrade(int tourId, int pageIndex, int pageSize);
    }
}
