using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ClubMessageService : CrudService<ClubMessageDto, ClubMessage>, IClubMessageService
    {
        private readonly IMapper _mapper;
        private readonly IClubMessageRepository _clubMessageRepository;
        private readonly IPersonEditingService _personEditingService;

        public ClubMessageService(ICrudRepository<ClubMessage> repository, IClubMessageRepository clubMessageRepository, IMapper mapper, IPersonEditingService personEditingService) : base(repository, mapper)
        {
            _mapper = mapper;
            _clubMessageRepository = clubMessageRepository;
            _personEditingService = personEditingService;
            _personEditingService = personEditingService;
        }

        public Result<PagedResult<ClubMessageDto>> GetAllForClub(long touristClubId)
        {
            var messages = _clubMessageRepository.GetAll();
            var messageDtos = new List<ClubMessageDto>();

            foreach (var message in messages)
            {
                if(message.TouristClubId == touristClubId)
                {
                    FluentResults.Result<PersonDto> result = _personEditingService.GetPersonByUserId((int)message.SenderId);
                    PersonDto person = result.Value;

                    var messageDto = new ClubMessageDto
                    {
                        Id = message.Id,
                        SenderId = message.SenderId,
                        SenderName = person.Name,
                        SenderSurname = person.Surname,
                        SentDate = message.SentDate,
                        Content = message.Content,
                        TouristClubId = message.TouristClubId,
                        LikesCount = message.PersonsLiked?.Count() ?? 0,
                        LikedByLoggedUser = false
                    };
                    messageDtos.Add(messageDto);
                }
            }
            return new PagedResult<ClubMessageDto>(messageDtos, messageDtos.Count);
        }

        public Result<PagedResult<ClubMessageDto>> GetAllForLoggedUser(long touristClubId, long loggedUserId)
        {
            var messages = _clubMessageRepository.GetAll();
            var messageDtos = new List<ClubMessageDto>();
            bool liked;

            foreach (var message in messages)
            {
                if (message.TouristClubId == touristClubId)
                {
                    FluentResults.Result<PersonDto> result = _personEditingService.GetPersonByUserId((int)message.SenderId);
                    PersonDto person = result.Value;

                    liked = message.PersonsLiked.Any(id => id == (int)loggedUserId);

                    var messageDto = new ClubMessageDto
                    {
                        Id = message.Id,
                        SenderId = message.SenderId,
                        SenderName = person.Name,
                        SenderSurname = person.Surname,
                        SentDate = message.SentDate,
                        Content = message.Content,
                        TouristClubId = message.TouristClubId,
                        LikesCount = message.PersonsLiked?.Count() ?? 0,
                        LikedByLoggedUser = liked
                    };
                    messageDtos.Add(messageDto);
                }
            }
            return new PagedResult<ClubMessageDto>(messageDtos, messageDtos.Count);
        }


        public Result<ClubMessageDto> CreateMessage(ClubMessageDto message)
        {
            _clubMessageRepository.Create(MapToDomain(message));
            var dto = _mapper.Map<ClubMessageDto>(message);
            return Result.Ok(dto);
        }
        public Result<ClubMessageDto> UpdateMessage(long clubMessageId, ClubMessageDto message)
        {
            var existingMessages = _clubMessageRepository.GetAll();
            var existingMessage = existingMessages.FirstOrDefault(x => x.Id == clubMessageId);


            if (existingMessage == null)
            {
                return Result.Fail<ClubMessageDto>("Poruka nije pronađena.");
            }

            existingMessage.Content = message.Content;
            existingMessage.SentDate = DateTime.UtcNow; 

            _clubMessageRepository.Update(existingMessage);

            var dto = _mapper.Map<ClubMessageDto>(existingMessage);
            return Result.Ok(dto);
        }
        public Result<ClubMessageDto> DeleteMessage(long clubMessageId)
        {
            var message = _clubMessageRepository.Delete(clubMessageId);
            var dto = _mapper.Map<ClubMessageDto>(message);
            return Result.Ok(dto);
        }
        public Result<ClubMessageDto> IncrementLikes(long clubMessageId, long userId)
        {
            var existingMessages = _clubMessageRepository.GetAll();
            var existingMessage = existingMessages.FirstOrDefault(x => x.Id == clubMessageId);


            if (existingMessage == null)
            {
                return Result.Fail<ClubMessageDto>("Poruka nije pronađena.");
            }

            if (!existingMessage.PersonsLiked.Any(id => id == (int)userId))
                existingMessage.PersonsLiked.Add((int)userId);

            _clubMessageRepository.Update(existingMessage);

            var dto = _mapper.Map<ClubMessageDto>(existingMessage);
            return Result.Ok(dto);
        }

        public Result<ClubMessageDto> DecrementLikes(long clubMessageId, long userId)
        {
            var existingMessages = _clubMessageRepository.GetAll();
            var existingMessage = existingMessages.FirstOrDefault(x => x.Id == clubMessageId);


            if (existingMessage == null)
            {
                return Result.Fail<ClubMessageDto>("Poruka nije pronađena.");
            }

            if (existingMessage.PersonsLiked.Any(id => id == (int)userId))
                existingMessage.PersonsLiked.Remove((int)userId);

            _clubMessageRepository.Update(existingMessage);

            var dto = _mapper.Map<ClubMessageDto>(existingMessage);
            return Result.Ok(dto);
        }


    }

}
