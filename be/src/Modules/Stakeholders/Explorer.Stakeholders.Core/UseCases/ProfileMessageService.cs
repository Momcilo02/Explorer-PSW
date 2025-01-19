using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ProfileMessageService : CrudService<ProfileMessageDto,  ProfileMessage>, IProfileMessageService
    {
        private readonly IMapper _mapper;
        private readonly IProfileMessageRepository _profileMessageRepository;
        private readonly IPersonEditingService _personEditingService;
        private readonly INotificationService _notificationService;
        public ProfileMessageService(ICrudRepository<ProfileMessage> repository, IProfileMessageRepository profileMessageRepository, IMapper mapper, IPersonEditingService personEditingService, INotificationService notificationService) : base(repository, mapper)
        {
            _mapper = mapper;
            _profileMessageRepository = profileMessageRepository;
            _personEditingService = personEditingService;
            _notificationService = notificationService;
        }

        public Result<PagedResult<ProfileMessageDto>> GetAllForUsers(long senderId, long receiverId)
        {
            var messages = _profileMessageRepository.GetAll();
            var messageDtos = new List<ProfileMessageDto>();
            long sender;
            long receiver;

            foreach(var message in messages)
            {
                if (message.SenderId == senderId && message.ReceiverId == receiverId)
                {
                    sender = senderId; 
                    receiver = receiverId;
                }
                else if(message.SenderId == receiverId && message.ReceiverId == senderId)
                {
                    sender = receiverId;
                    receiver = senderId;
                }
                else { continue; }

                FluentResults.Result<PersonDto> result = _personEditingService.GetPersonByUserId((int)sender);
                PersonDto person = result.Value;
                var messageDto = new ProfileMessageDto
                {
                    ReceiverId = receiver,
                    SenderId = sender,
                    SenderName = person.Name,
                    SenderSurname = person.Surname,
                    SentDate = message.SentDate,
                    Content = message.Content,
                    ResourcesId = message.ResourcesId,
                    Type = (Explorer.Stakeholders.API.Dtos.ResourcesType)message.Type,
                    ResourcesName = message.ResourcesName
                };
                messageDtos.Add(messageDto);
          
            }
            return new PagedResult<ProfileMessageDto>(messageDtos, messageDtos.Count);

        }
        public Result<ProfileMessageDto> CreateMessage(ProfileMessageDto message)
        {
            NotificationDto notification = new NotificationDto();
            notification.ReportId = 0;
            notification.RecipientId = (int)message.ReceiverId;
            notification.SenderId = (int)message.SenderId;
            notification.IsRead = false;
            notification.NotificationType = API.Dtos.NotificationType.PROFILE_CHAT;
            _notificationService.Create(notification);

             _profileMessageRepository.Create(MapToDomain(message));
            var dto = _mapper.Map<ProfileMessageDto>(message);
            return Result.Ok(dto);
        }


    }
}
