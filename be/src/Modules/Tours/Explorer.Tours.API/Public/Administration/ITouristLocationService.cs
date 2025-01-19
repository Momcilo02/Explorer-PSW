using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public
{
    public interface ITouristLocationService
    {
        Result<PagedResult<TouristLocationDto>> GetPaged(int page, int pageSize);
        Result<TouristLocationDto> Create(TouristLocationDto tourProblemReport);
        Result<TouristLocationDto> Update(TouristLocationDto tourProblemReport);
        Result Delete(int id);
        Result<TouristLocationDto> GetByTouristId(long id);
    }
}
