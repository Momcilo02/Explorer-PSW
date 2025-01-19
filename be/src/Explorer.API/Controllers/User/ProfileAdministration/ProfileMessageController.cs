using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User.ProfileAdministration
{
    [Authorize(Policy = "userPolicy")]
    [Microsoft.AspNetCore.Mvc.Route("api/profile-message")]
    public class ProfileMessageController : BaseApiController
    {
        private readonly IProfileMessageService _profileMessageService;

        public ProfileMessageController(IProfileMessageService profileMessageService)
        {
            _profileMessageService = profileMessageService;
        }

        [HttpGet("{senderId:long}/{receiverId:long}")]
        public ActionResult<PersonDto> GetMessagesForUsers(long senderId, long receiverId)
        {
            var result = _profileMessageService.GetAllForUsers(senderId, receiverId);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<ProfileMessageDto> Create([FromBody] ProfileMessageDto profileMessageDto)
        {
            var result = _profileMessageService.CreateMessage(profileMessageDto);
            return CreateResponse(result);
        }
    }
}
