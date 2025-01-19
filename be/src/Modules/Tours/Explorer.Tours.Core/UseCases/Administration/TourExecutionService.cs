using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.TourExecutions;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourExecutionService : CrudService<TourExecutionDto, TourExecution>, ITourExecutionService
    {
        private readonly IMapper _mapper;
        private readonly ITourExecutionRepository _tourExecutionRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IKeyPointRepository _keyPointRepository;
        public TourExecutionService(IMapper mapper, ITourExecutionRepository tourExecutionRepository,ITourRepository tourRepository,IKeyPointRepository keyPointRepository)  : base(mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _mapper = mapper;
            _tourRepository = tourRepository;
            _keyPointRepository = keyPointRepository;
        }

        public Result<TourExecutionDto> Get(int id)
        {
            var result = _tourExecutionRepository.Get(id);
            return MapToDto(result);
        }

        public Result<TourExecutionDto> GetByUserAndTourIds(int touristId, int tourId)
        {
            try
            {
                var result = _tourExecutionRepository.GetByUserAndTourIds(touristId, tourId);

                return MapToDto(result);
            }
            catch(KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<PagedResult<TourExecutionDto>> GetPaged(int page, int pageSize)
        {
            var result = _tourExecutionRepository.GetPaged(page, pageSize);
            return MapToDto(result);
        }

        public Result<TourExecutionDto> LeaveTour(int touristId,int tourId)
        {
            var tourToLeave = _tourExecutionRepository.GetByUserAndTourIds(touristId, tourId);
            tourToLeave.LeaveTour();

            var result = _tourExecutionRepository.Update(tourToLeave);
            return MapToDto(result);

        }

        public Result StartNewTour(TourExecutionDto tourExecution)
        {
            try
            {
                TourExecution newTour = MapToDomain(tourExecution);
                newTour.StartNewTour();
                var creationResult = _tourExecutionRepository.Create(newTour);

                if (creationResult.IsSuccess)
                {
                    return Result.Ok(); 
                }

                return Result.Fail(creationResult.Errors); 
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourExecutionDto> Update(TourExecutionDto tour)
        {
            try
            {
                var result = _tourExecutionRepository.Update(MapToDomain(tour));
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result<TourExecutionDto> CheckLocation(int id, float latitude, float longitude)
            {
            try
            {
                var tourExecution = _tourExecutionRepository.Get(id);
                var tour = _tourRepository.Get(tourExecution.TourId);
                tourExecution.UpdateLocation(latitude, longitude);
                //var checkpoint = _tourExecutionRepository.FindNearbyCheckpoint(latitude, longitude,tour);
                for (int i = 0; i < tour.KeyPoints.Count; i++)
                {
                    var keyPoint = tour.KeyPoints[i];
                    bool isNearby = FindNearbyCheckpoint(latitude, longitude, keyPoint.Longitude, keyPoint.Latitude);
                    bool isAlreadyCompleted = tourExecution.CompletedKeyPoints.Any(k => k.CompletedKeyPointId == keyPoint.Id);
                    bool isNextInOrder = tourExecution.CompletedKeyPoints.Count == i;

                    if (isNearby && !isAlreadyCompleted && isNextInOrder)
                    {
                        var completedKey = new CompletedKeyPoints(DateTime.UtcNow, Convert.ToInt32(keyPoint.Id));
                        tourExecution.AddCompletedKeyPoint(completedKey);
                        tourExecution.UpdateCompletedPercentage(tour.KeyPoints.Count);
                        var result = _tourExecutionRepository.Update(tourExecution);
                        return MapToDto(result);
                    }
                }

                // Update location if no checkpoint was completed
                return MapToDto(_tourExecutionRepository.Update(tourExecution));
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }

        public Result<List<KeyPointDto>> GetCompletedKeyPoints(int id)
        {
            try
            {
                var tourExecution = _tourExecutionRepository.Get(id);
                var completedKeyPoints = new List<KeyPointDto>();
                //var result = tourExecution.CompletedKeyPoints;
                foreach (var completed in tourExecution.CompletedKeyPoints)
                {
                    var keyPoint = _keyPointRepository.Get(completed.CompletedKeyPointId);
                    if (keyPoint != null)
                    {
                        var keyPointDto = _mapper.Map<KeyPointDto>(keyPoint);
                        completedKeyPoints.Add(keyPointDto);
                    }
                }
                var result = completedKeyPoints;
                return Result.Ok(completedKeyPoints);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        private bool FindNearbyCheckpoint(float latitude, float longitude, float keyLongitude, float keyLatitude)
        {

            float distance = CalculateDistance(latitude, longitude, keyLatitude, keyLongitude);
            if (distance <= 20)
            {
                return true;
            }
            return false;
        }
        private float CalculateDistance(float lat1, float lon1, float lat2, float lon2)
        {
            // Haversine formula for distance calculation
            const float R = 6371000; // Radius of the Earth in meters
            float dLat = ToRadians(lat2 - lat1);
            float dLon = ToRadians(lon2 - lon1);
            float a =
                (float)(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2));
            float c = (float)(2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
            return R * c; // Distance in meters
        }
        private float ToRadians(float angle)
        {
            return (float)(angle * Math.PI / 180);
        }

        public Result<List<TourExecutionDto>> GetAllTouristTours(int touristId)
        {
            try
            {
                var result = _tourExecutionRepository.GetAllByUserId(touristId);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TourExecutionDto> GetActiveTour(int touristId)
        {
            try
            {
                var result = _tourExecutionRepository.GetUserActiveTour(touristId);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TourExecutionDto> FinishTour(int tourExecutionId)
        {
            var tourToFinish = _tourExecutionRepository.Get(tourExecutionId);
            tourToFinish.FinishTour();

            var result = _tourExecutionRepository.Update(tourToFinish);
            return MapToDto(result);
        }

    }
}
