using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User.TourProblem
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/user/tour-problem")]
    public class TourProblemUserController : BaseApiController
    {
        private readonly ITourProblemReportService _tourProblemReportService;

        public TourProblemUserController(ITourProblemReportService tourProblemReportService)
        {
            _tourProblemReportService = tourProblemReportService;
        }

        [HttpPut("addMessage/{userId:int}/{reportId:int}")]
        public ActionResult<TourProblemReportDto> AddMessage([FromBody] MessageDto message, int userId, int reportId)
        {
            var result = _tourProblemReportService.AddMessage(message, userId, reportId);
            return CreateResponse(result);
        }

        [HttpGet("getReportById/{id:int}")]
        public ActionResult<TourProblemReportDto> Get(int id)
        {
            var result = _tourProblemReportService.Get(id);
            return CreateResponse(result);
        }
    }
}
