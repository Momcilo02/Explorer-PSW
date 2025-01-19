using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.TourProblem
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administrator/tour-problem")]
    public class TourProblemAdministratorController : BaseApiController
    {
        private readonly ITourProblemReportService _tourProblemReportService;

        public TourProblemAdministratorController(ITourProblemReportService tourProblemReportService)
        {
            _tourProblemReportService = tourProblemReportService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<PagedResult<TourProblemReportDto>> GetById(int id)
        {
            var result = _tourProblemReportService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<TourProblemReportDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("set-deadline/{id:int}")]
        public ActionResult<TourProblemReportDto> SetSolvingDeadline(int id, [FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.SetSolvingDeadline(id, tourProblemReport);
            return CreateResponse(result);
        }

        [HttpPut("penalize-author-close-problem/{id:int}")]
        public ActionResult<TourProblemReportDto> PenalizeAuthorAndCloseProblem(int id, [FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.PenalizeAuthorAndCloseProblem(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors); 
            }
            return Ok(result.Value);
        }

        [HttpPut("addMessage/{userId:int}/{reportId:int}")]
        public ActionResult<TourProblemReportDto> AddMessage([FromBody] MessageDto message, int userId, int reportId)
        {
            var result = _tourProblemReportService.AddMessage(message, userId, reportId);
            return CreateResponse(result);
        }
    }
}

