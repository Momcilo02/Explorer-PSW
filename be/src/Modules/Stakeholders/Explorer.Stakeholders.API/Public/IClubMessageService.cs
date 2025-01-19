using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IClubMessageService
    {
        public Result<PagedResult<ClubMessageDto>> GetAllForClub(long touristClubId);
        public Result<PagedResult<ClubMessageDto>> GetAllForLoggedUser(long touristClubId, long loggedUserId);
        public Result<ClubMessageDto> Create(ClubMessageDto message);
        public Result<ClubMessageDto> Update(ClubMessageDto message);
        public Result<ClubMessageDto> UpdateMessage(long touristClubId, ClubMessageDto message);
        public Result<ClubMessageDto> DeleteMessage(long touristClubId);
        public Result<ClubMessageDto> IncrementLikes(long touristClubId, long userId);
        public Result<ClubMessageDto> DecrementLikes(long touristClubId, long userId);

    }
}
