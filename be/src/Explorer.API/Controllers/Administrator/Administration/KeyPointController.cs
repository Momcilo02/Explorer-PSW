using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/keyPoint")]
    public class KeyPointController : BaseApiController
    {
        private readonly IKeyPointService _keyPointService;

        public KeyPointController(IKeyPointService keyPointService)
        {
            _keyPointService = keyPointService;
        }

        [HttpGet]
        public ActionResult<List<KeyPointDto>> GetAll()
        {
            var result = _keyPointService.GetAll();
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<KeyPointDto> UpdateKeyPointStatus([FromBody] KeyPointDto keyPoint)
        {
            var result = _keyPointService.UpdateStatus(keyPoint);
            return CreateResponse(result);
        }
    }
}
