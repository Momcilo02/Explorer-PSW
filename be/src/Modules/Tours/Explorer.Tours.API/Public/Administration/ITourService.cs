using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourService
    {
        Result<PagedResult<TourDto>> GetPaged(int page, int pageSize);
        Result<TourDto> Create(TourDto tour);
        Result<TourDto> Update(TourDto tour);
        Result Delete(long id);
        Result<TourDto> Get(long id);
        public Result<TourDto> SetHasQuiz(int id, bool hasQuiz);
        void DeleteEquipments(long id);
        Result<TourDto> Publish(TourDto tour);
        Result<PagedResult<TourDto>> GetPublishedTour(int page, int pageSize);
        public Result<PagedResult<BasicTourDetailsDto>> GetPublishedTours(int page, int pageSize);
        public Result<TourDto> GetPublishedTourPreview(long id);
        public Result<TourDto> GetPublishedTourById(long id);
        Result<TourDto> Archive(TourDto tour);
        Result<TourDto> UpdateTourLength(TourDto tour);
        Result<TourDto> ReactivateTour(TourDto tour);
        Result<PagedResult<TourDto>> GetByAuthorId(int id, int page, int pageSize);
        Result<List<TourDto>> GetTourByDistance(float latitude,float longitude,float distance);
        Result<List<KeyPointDto>> GetAvailableKeyPoints(int id);
    }
}
