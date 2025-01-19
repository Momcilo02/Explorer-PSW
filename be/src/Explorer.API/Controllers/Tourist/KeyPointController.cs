using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tours/keypoint")]
    public class KeyPointController : BaseApiController
    {
        private readonly IKeyPointService _keyPointService;
        public KeyPointController(IKeyPointService keyPointService)
        {
            _keyPointService = keyPointService;
        }
        [HttpGet]
        [Route("public")]
        public ActionResult<PagedResult<KeyPointDto>> GetPublic([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _keyPointService.GetPublicKeyPoints(page, pageSize);
            return CreateResponse(result);
        }
    }
}
