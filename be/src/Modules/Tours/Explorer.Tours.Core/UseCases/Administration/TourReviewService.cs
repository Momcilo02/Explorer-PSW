using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Explorer.Tours.Core.Domain.TourExecutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository;
        public TourReviewService(ICrudRepository<TourReview> repository, IMapper mapper, ITourReviewRepository tourReviewRepository) : base(repository, mapper)
        {
            _tourReviewRepository = tourReviewRepository;
        }

        public Result<PagedResult<TourReviewDto>> GetReviewsByTourId(int id, int pageIndex, int pageSize)
        {
            var result = _tourReviewRepository.GetReviewsForTour(id, pageIndex, pageSize);
            return MapToDto(result);
        }

        public Result<double> GetAverageGrade(int tourId, int pageIndex, int pageSize)
        {
            var result = _tourReviewRepository.GetAverageGrade(tourId, pageIndex, pageSize);
            return Convert.ToDouble(result);
        }
    }
}
