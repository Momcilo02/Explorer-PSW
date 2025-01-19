using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User.ProfileAdministration
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/profile-administration/edit")]
    public class ProfileEditingController : BaseApiController
    {
        private readonly IPersonEditingService _personEditingService;

        public ProfileEditingController(IPersonEditingService personEditingService)
        {
            _personEditingService = personEditingService;
        }

        [HttpPut]
        public ActionResult<PersonDto> Edit([FromForm] PersonDto person)
        {
            var result = _personEditingService.Update(person);
            return CreateResponse(result);
        }

        [HttpPut("currentUser")]
        public ActionResult<PersonDto> UpdateCurrentUser([FromBody] PersonDto person)
        {
            var result = _personEditingService.Update(person);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<PersonDto> GetUserInfo(int id)
        {
            var result = _personEditingService.GetPersonByUserId(id);
            return CreateResponse(result);
        }
        [HttpGet]
        public ActionResult<PersonDto> GetAllUsers([FromQuery]int page, [FromQuery]int pageSize)
        {
            var result = _personEditingService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
        [HttpPut]
        [Route("clubMember")]
        public ActionResult<PersonDto> AddClubInMember([FromBody] PersonDto person)
        {
            var result = _personEditingService.Update(person);
            return CreateResponse(result);
        }

        [HttpPut]
        [Route("changeStatus")]
        public ActionResult<PersonDto> ChangeTouristStatus([FromBody] PersonDto person)
        {
            var result = _personEditingService.Update(person);
            return CreateResponse(result);
        }
    }
}
