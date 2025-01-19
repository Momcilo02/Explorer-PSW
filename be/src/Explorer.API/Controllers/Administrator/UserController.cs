using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator
{

    [Authorize(Policy = "administratorPolicy")]
    [Route("api/profile-administrator/")]
    public class UserController : BaseApiController
    {
        private readonly IPersonEditingService _personEditingService;
        public UserController(IPersonEditingService personEditingService)
        {
            _personEditingService = personEditingService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<PersonDto> GetUserInfo(int id)
        {
            var result = _personEditingService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<PersonDto>> GetAllUsers([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _personEditingService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}
