using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourReviewService
    {
       
        Result<PagedResult<TourReviewDto>> GetPaged(int page, int pageSize);

        Result<TourReviewDto> Create(TourReviewDto tourReview);

        Result<TourReviewDto> Update(TourReviewDto tourReview);

        Result<PagedResult<TourReviewDto>> GetReviewsByTourId(int id, int pageIndex, int pageSize);
        Result<double> GetAverageGrade(int tourId, int pageIndex, int pageSize);
        Result Delete(int id);
    }
}
