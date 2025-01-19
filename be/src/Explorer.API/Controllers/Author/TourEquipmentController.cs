using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "administratorPolicy")]
[Route("api/administration/tourEquipment")]
public class TourEquipmentController : BaseApiController
{
    private readonly ITourEquipmentService _tourEquipmentService;

    public TourEquipmentController(ITourEquipmentService tourEquipmentService)
    {
        _tourEquipmentService = tourEquipmentService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourEquipmentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _tourEquipmentService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<TourEquipmentDto> Create([FromBody] TourEquipmentDto tourEquipment)
    {
        var result = _tourEquipmentService.Create(tourEquipment);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<TourEquipmentDto> Update([FromBody] TourEquipmentDto tourEquipment)
    {
        var result = _tourEquipmentService.Update(tourEquipment);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _tourEquipmentService.Delete(id);
        return CreateResponse(result);
    }
}
