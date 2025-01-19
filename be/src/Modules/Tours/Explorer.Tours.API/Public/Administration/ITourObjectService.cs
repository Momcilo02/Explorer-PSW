using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourObjectService
    {
        Result<PagedResult<TourObjectDto>> GetPaged(int page, int pageSize);
        Result<TourObjectDto> Create(TourObjectDto objectt);
        Result<TourObjectDto> Update(TourObjectDto objectt);
        Result<TourObjectDto> UpdateStatus(TourObjectDto tourObject);
        Result Delete(int id);
        Result<PagedResult<TourObjectDto>> GetPublicObjects(int page, int pageSize);
    }
}
