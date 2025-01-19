using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class NotificationService : CrudService<NotificationDto, Notification>, INotificationService, INotificationInternalService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(ICrudRepository<Notification> repository, INotificationRepository notificationRepository, IMapper mapper) : base(repository, mapper)
        {
            _notificationRepository = notificationRepository;
        }
        public Result<PagedResult<NotificationDto>> GetByLoggedUser(int id, int page, int pageSize)
        {
            var result = _notificationRepository.GetByLoggedUser(id, page, pageSize);
            return MapToDto(result);
        }

        
    }
}
