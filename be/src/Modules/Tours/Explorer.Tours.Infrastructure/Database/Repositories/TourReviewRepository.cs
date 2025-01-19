using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using FluentResults;
using Explorer.Tours.Core.Domain.TourExecutions;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    internal class TourReviewRepository : ITourReviewRepository
    {
        private readonly ToursContext _context;

        public TourReviewRepository(ToursContext context)
        {
            _context = context;
        }

        public TourReview Get(int reviewId)
        {
            return _context.TourReview.FirstOrDefault(tr => tr.Id == reviewId);
        }

        public PagedResult<TourReview> GetReviewsForTour(int tourId, int pageIndex, int pageSize)
        {
            var task = _context.TourReview
                                 .Where(tr => tr.TourId == tourId).GetPagedById(pageIndex, pageSize);
            task.Wait();
            return task.Result;
        }

        public float GetAverageGrade(int tourId, int pageIndex, int pageSize)
        {
            var task = _context.TourReview.Where(tr => tr.TourId == tourId).GetPagedById(pageIndex, pageSize);
            task.Wait();
            float counter = _context.TourReview.Where(tr => tr.TourId == tourId).Count();
            float sum = 0;
            foreach (TourReview review in task.Result.Results)
            {
                sum += review.Rating;
            }
            float result = sum / counter;
            return result;
        }

        
    }
}
