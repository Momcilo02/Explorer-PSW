using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/touristLocation")]
    public class TouristLocationController : BaseApiController
    {
        private readonly ITouristLocationService _touristLocationService;

        public TouristLocationController(ITouristLocationService touristLocationService)
        {
            _touristLocationService = touristLocationService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TouristLocationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _touristLocationService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TouristLocationDto> Create([FromBody] TouristLocationDto touristLocation)
        {
            var result = _touristLocationService.Create(touristLocation);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TouristLocationDto> Update([FromBody] TouristLocationDto touristLocation)
        {
            var result = _touristLocationService.Update(touristLocation);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _touristLocationService.Delete(id);
            return CreateResponse(result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<TouristLocationDto> GetByTouristId(long id)
        {
            var result = _touristLocationService.GetByTouristId(id);
            return CreateResponse(result);
        }
    }
}
