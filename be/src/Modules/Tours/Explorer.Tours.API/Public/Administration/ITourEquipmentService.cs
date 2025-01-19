using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration;

public interface ITourEquipmentService
{
    Result<PagedResult<TourEquipmentDto>> GetPaged(int page, int pageSize);
    Result<TourEquipmentDto> Create(TourEquipmentDto tourEquipment);
    Result<TourEquipmentDto> Update(TourEquipmentDto tourEquipment);
    Result Delete(int id);
}
