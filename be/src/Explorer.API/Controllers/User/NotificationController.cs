using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/user/notification")]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public ActionResult<PagedResult<NotificationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _notificationService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("get/{id:int}")]
        public ActionResult<NotificationDto> Get(int id)
        {
            var result = _notificationService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet("getByLoggedUser/{id:int}")]
        public ActionResult<BuildingBlocks.Core.UseCases.PagedResult<NotificationDto>> GetByLoggedUser(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _notificationService.GetByLoggedUser(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<NotificationDto> Update([FromBody] NotificationDto notification)
        {
            
            notification.IsRead = true;
            var result = _notificationService.Update(notification);
            return CreateResponse(result);
        }
    }
}
