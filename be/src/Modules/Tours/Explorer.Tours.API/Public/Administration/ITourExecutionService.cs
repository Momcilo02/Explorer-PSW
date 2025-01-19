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
    public interface ITourExecutionService
    {
        Result<PagedResult<TourExecutionDto>> GetPaged(int page, int pageSize);
        Result<TourExecutionDto> Update(TourExecutionDto tour);

        Result<TourExecutionDto> Get(int id);

        Result<TourExecutionDto> GetByUserAndTourIds(int touristId, int tourId);

        Result StartNewTour(TourExecutionDto tourExecution);

        Result<TourExecutionDto> FinishTour(int tourExecutionid);
        Result<TourExecutionDto> LeaveTour(int touristId, int tourId);
        Result<TourExecutionDto> CheckLocation(int id, float latitude, float longitude);
        Result<List<KeyPointDto>> GetCompletedKeyPoints(int id);
        Result<List<TourExecutionDto>> GetAllTouristTours(int touristId);
        Result<TourExecutionDto> GetActiveTour(int touristId);
    }
}
