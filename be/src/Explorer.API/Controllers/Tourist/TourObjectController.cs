using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tours/object")]
    public class TourObjectController : BaseApiController
    {
        private readonly ITourObjectService _objectService;

        public TourObjectController(ITourObjectService objectService)
        {
            _objectService = objectService;
        }
        [HttpGet]
        [Route("public")]
        public ActionResult<PagedResult<TourObjectDto>> GetPublic([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _objectService.GetPublicObjects(page, pageSize);
            return CreateResponse(result);
        }
    }
}
