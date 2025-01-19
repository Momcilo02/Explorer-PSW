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
    public interface IToursitClubService
    {
        Result<PagedResult<TouristClubDto>> GetPaged(int page, int pageSize);
        Result<TouristClubDto> Create(TouristClubDto club);
        Result<TouristClubDto> Update(TouristClubDto club);
        //Result<PagedResult<TouristClubDto>> GetForLoggedUser(long userId);
        Result<TouristClubDto> Get(int id);
    }
}
