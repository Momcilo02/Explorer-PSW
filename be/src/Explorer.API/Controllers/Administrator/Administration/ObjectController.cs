using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{

    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/object")]
    public class ObjectController: BaseApiController
    {
        private readonly ITourObjectService _objectService;

        public ObjectController(ITourObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourObjectDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _objectService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourObjectDto> UpdateObjectStatus([FromBody] TourObjectDto tourObject)
        {
            var result = _objectService.UpdateStatus(tourObject);
            return CreateResponse(result);
        }

    }
}
