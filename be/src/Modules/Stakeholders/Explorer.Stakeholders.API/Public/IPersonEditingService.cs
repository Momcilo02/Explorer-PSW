using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.API.Public
{
    public interface IPersonEditingService
    {
        Result<PersonDto> Update(PersonDto personDto);
        Result<PersonDto> Get(int id);
        Result<PagedResult<PersonDto>> GetPaged(int page, int pageSize);

        Result<PersonDto> GetPersonByUserId(int userId);
    }
}
