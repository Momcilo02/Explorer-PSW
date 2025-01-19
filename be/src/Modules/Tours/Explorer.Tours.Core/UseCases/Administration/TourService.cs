using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Explorer.Stakeholders.API.Dtos;
using TourStatus = Explorer.Tours.Core.Domain.TourStatus;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto,Tour>, ITourService, IInternalTourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IKeyPointRepository _keyPointRepository;
        private readonly IMapper _mapper;

        public TourService(ICrudRepository<Tour> repository, IMapper mapper, ITourRepository tourRepository, IKeyPointRepository keyPointRepository) : base (repository, mapper)
        {
            _tourRepository = tourRepository;
            _keyPointRepository = keyPointRepository;
            _mapper = mapper;
        }
        public Result<TourDto> Create(TourDto dto)
        {
            try
            {
                dto.PublishTime = null;
                dto.ArchiveTime = null;
                var tour = _tourRepository.Create(MapToDomain(dto));
                return MapToDto(tour);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
           
        }
        public Result<PagedResult<TourDto>> GetPaged(int page, int pageSize)
        {
           var result = _tourRepository.GetPaged(page, pageSize);
           return MapToDto(result);
        }

        public Result<TourDto> Get(long id)
        {
            var result = _tourRepository.Get(id);
            return MapToDto(result);
        }
        public Result Delete(long id)
        {
            try
            {
                _tourRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public void DeleteEquipments(long id)
        {
            _tourRepository.DeleteEquipmenmts(id);
        }

        public Result<TourDto> Publish(TourDto tourDto)
        {
            try
            {
                Tour tour = MapToDomain(tourDto);
                tour = tour.Publish();
                return MapToDto(_tourRepository.Update(tour));
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourDto> Archive(TourDto tourDto)
        {
            try
            {
                Tour tour = MapToDomain(tourDto);
                tour = tour.Archive();
                return MapToDto(_tourRepository.Update(tour));
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourDto> UpdateTourLength(TourDto tourDto)
        {
            try
            {
                Tour tour = MapToDomain(tourDto);
                tour = tour.UpdateTourLength(tour.Length);
                foreach (var kp in tour.KeyPoints)
                {
                    _keyPointRepository.Update(kp);
                }
                return MapToDto(_tourRepository.Update(tour));
            }
            catch(ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourDto> ReactivateTour(TourDto tourDto)
        {
            try
            {
                Tour tour = MapToDomain(tourDto);
                tour = tour.ReactivateTour();
                return MapToDto(_tourRepository.Update(tour));
            } catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result<List<TourDto>> GetMany(List<long> tourIds)
        {
            var tours = CrudRepository.GetMany(tourIds);
            return MapToDto(tours);
        }

        public Result<PagedResult<TourDto>> GetPublishedTour(int page, int pageSize)
        {
            var result = _tourRepository.GetPublishedTours(page, pageSize);
            return MapToDto(result);
        }
        public Result<TourDto> CloseTour(int id)
        {
            var tour = CrudRepository.Get(id);
            if (tour == null)
            {
                throw new Exception("Tour not found");
            }
            tour.CloseTour();
            CrudRepository.Update(tour);

            return Result.Ok(MapToDto(tour));
        }
        public Result<PagedResult<BasicTourDetailsDto>> GetPublishedTours(int page, int pageSize)
        {
            var result = _tourRepository.GetPublishedTours(page, pageSize);
            var items = result.Results.Select(t => new BasicTourDetailsDto
            {
                Id = t.Id,
                Cost = t.Cost,
                Description = t.Description,
                Name = t.Name,
                Length = t.Length,
                FirstKeyPoint = t.KeyPoints.Any() ? new KeyPointDto
                {
                    Id = (int)t.KeyPoints.First().Id,
                    Name = t.KeyPoints.First().Name,
                    Description = t.KeyPoints.First().Description,
                    Image = t.KeyPoints.First().Image,
                    Latitude = t.KeyPoints.First().Latitude,
                    Longitude = t.KeyPoints.First().Longitude,
                    Status = (KeyPointDto.PublicStatus)t.KeyPoints.First().Status
                } : null 
            }).ToList();

            return new PagedResult<BasicTourDetailsDto>(items, result.TotalCount);
            //return MapToDto(result);
        }
        public Result<TourDto> GetPublishedTourPreview(long id)
        {
            var result = _tourRepository.GetPublishedTourByid(id);
            return MapToDto(result);
        }
        public Result<TourDto> GetPublishedTourById(long id)
        {
            var result = _tourRepository.Get(id);

            if (result.Status != TourStatus.Published)
                return Result.Fail("Tour is not Public");

            var tourDto = new TourDto
            {
                Id = (int)result.Id,
                Cost = result.Cost,
                Description = result.Description,
                Name = result.Name,
                Length = result.Length,
                Image = result.Image,
                AverageRate = result.getAverageRate(),
                KeyPoints = result.KeyPoints.Select(kp => new KeyPointDto
                {
                    Id = (int)kp.Id,
                    Name = kp.Name,
                    Description = kp.Description,
                    Image = kp.Image,
                    Latitude = kp.Latitude,
                    Longitude = kp.Longitude
                }).ToList()
            };

            return tourDto;
        }

        public Result<TourDto> Update(TourDto tourDto)
        {
            try
            {
                var tour = MapToDomain(tourDto);
                foreach (var kp in tour.KeyPoints)
                {
                     _keyPointRepository.Update(kp);
                }
                _tourRepository.Update(tour);
                return MapToDto(tour);
            }
            catch (Exception ex) 
            {
                return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
            }
        }
        public Result<PagedResult<TourDto>> GetByAuthorId(int id, int page, int pageSize)
        {
            var result = _tourRepository.GetByAuthorId(id, page, pageSize);
            return MapToDto(result);
        }
        public Result<List<TourDto>> GetTourByDistance(float latitude, float longitude, float distance)
        {
            var tours = _tourRepository.GetPublishedToursList();
            var filteredTours = tours.Where(t => t.KeyPoints.Any(kp => IsWithinDistance(kp, latitude, longitude, distance))).ToList();
            return MapToDto(filteredTours);
        }
        private bool IsWithinDistance(KeyPoint keyPoint, float lat, float lon, float distance)
        {
            var R = 6371;
            var dLat = (keyPoint.Latitude - lat) * (Math.PI / 180);
            var dLon = (keyPoint.Longitude - lon) * (Math.PI / 180);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat * (Math.PI / 180)) * Math.Cos(keyPoint.Latitude * (Math.PI / 180)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distanceInKm = R * c;

            return distanceInKm <= distance; 
        }
        public Result<List<KeyPointDto>> GetAvailableKeyPoints(int id)
        {
            var allPublicKeyPoints = _keyPointRepository.GetPublicKeyPoints(0,0).Results;
            var tour = _tourRepository.Get(id);
            var result = new List<KeyPointDto>();
            if (tour == null)
            {
                return result;
            }
            foreach(var keyPoint in allPublicKeyPoints)
            {
                bool isKeyPointAlreadyInTour = tour.KeyPoints.Any(k => k.Latitude == keyPoint.Latitude && k.Longitude == keyPoint.Longitude);
                if (!isKeyPointAlreadyInTour)
                {
                    var keyPointDto = _mapper.Map<KeyPointDto>(keyPoint);
                    result.Add(keyPointDto);
                }
               
            }
            return result;
        }
        public Result<TourDto> SetHasQuiz(int id, bool hasQuiz)
        {
            // 1. Dohvati turu iz baze
            var tour = _tourRepository.Get(id);
            if (tour == null)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Tour not found.");
            }

            // 2. Postavi polje HasQuiz
            tour.HasQuiz = hasQuiz;

            // 3. Ažuriraj turu u bazi
            var updatedTour = _tourRepository.Update(tour);

            // 4. Vrati ažuriranu turu kao DTO
            return MapToDto(updatedTour);
        }

    }
}
