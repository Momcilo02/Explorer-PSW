using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/tours/object")]
    public class TourObjectController : BaseApiController
    {
        private readonly ITourObjectService _objectService;

        public TourObjectController(ITourObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourObjectDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _objectService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourObjectDto> Create([FromBody] TourObjectDto objectt)
        {
            var result = _objectService.Create(objectt);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourObjectDto> Update([FromBody] TourObjectDto objectt)
        {
            var result = _objectService.Update(objectt);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _objectService.Delete(id);
            return CreateResponse(result);
        }
    }
}
