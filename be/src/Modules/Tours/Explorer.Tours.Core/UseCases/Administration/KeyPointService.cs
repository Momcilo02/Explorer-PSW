using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Tours.Core.UseCases.Administration;

public class KeyPointService : BaseService<KeyPointDto, KeyPoint>, IKeyPointService
{
    private readonly IKeyPointRepository _keyPointRepository;
    private readonly ITourRepository _tourRepository;
    private readonly INotificationInternalService _notificationInternalService;
    public KeyPointService(IKeyPointRepository keyPointRepository,ITourRepository tourRepository, INotificationInternalService notificationInternalService, IMapper mapper) : base(mapper) 
    {
        _keyPointRepository = keyPointRepository;
        _notificationInternalService = notificationInternalService;
        _tourRepository = tourRepository;
    }

    public Result<KeyPointDto> Create(KeyPointDto keyPoint)
    {
        try
        {
            if (!_keyPointRepository.DoesExistByCoordinates(keyPoint.Longitude, keyPoint.Latitude))
            {
                var result = _keyPointRepository.Create(MapToDomain(keyPoint));
                return MapToDto(result);
            }
            else
            {
               return Result.Fail(FailureCode.InvalidArgument).WithError("Taj keypoint vec postoji!");
            }
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result<List<KeyPointDto>> GetAll()
    {
        try 
        {
            var result = _keyPointRepository.GetAll();
            return MapToDto(result);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result<KeyPointDto> Update(KeyPointDto keyPoint)
    {
        try
        {
            var result = _keyPointRepository.Update(MapToDomain(keyPoint));
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

    public Result<KeyPointDto> UpdateStatus(KeyPointDto keyPoint)
    {
        try
        {
            var tours = _tourRepository.GetAll();
            int authorId=-1;

            foreach(var tour in tours)
            {
                foreach(var kp in tour.KeyPoints)
                {
                    if(kp.Id == keyPoint.Id)
                    {
                        authorId = tour.AuthorId;
                        break;
                    }
                }
            }
            if (authorId != -1)
            {
                NotificationDto notification = new NotificationDto();
                notification.ReportId = 0;
                notification.RecipientId = authorId;
                notification.IsRead = false;

                if (keyPoint.Status == KeyPointDto.PublicStatus.PUBLIC)
                    notification.NotificationType = Stakeholders.API.Dtos.NotificationType.ACCEPT_KP;
                else
                    notification.NotificationType = Stakeholders.API.Dtos.NotificationType.REFUSE_KP;
                _notificationInternalService.Create(notification);
            }
            else return null;

            var result = _keyPointRepository.Update(MapToDomain(keyPoint));
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
    public Result<PagedResult<KeyPointDto>> GetPublicKeyPoints(int page, int pageSize)
    {
        var result = _keyPointRepository.GetPublicKeyPoints(page, pageSize);
        return MapToDto(result);
    }
}
