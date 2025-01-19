using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Administration
{
    [Route("api/administration/tour-problem-report")]
    public class TourProblemReportController : BaseApiController
    {
        private readonly ITourProblemReportService _tourProblemReportService;

        public TourProblemReportController(ITourProblemReportService tourProblemReportService)
        {
            _tourProblemReportService = tourProblemReportService;
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<BuildingBlocks.Core.UseCases.PagedResult<TourProblemReportDto>> GetByTouristId(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetByTouristId(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet]
        [Route("authorView/{id:int}")]
        [Authorize(Policy = "authorPolicy")]
        public ActionResult<BuildingBlocks.Core.UseCases.PagedResult<TourProblemReportDto>> GetByAuthorId(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetByAuthorId(id, page, pageSize);
            return result;
        }

        [HttpGet]
        [Route("administratorView")]
        [Authorize(Policy = "administratorPolicy")]
        public ActionResult<BuildingBlocks.Core.UseCases.PagedResult<TourProblemReportDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<TourProblemReportDto> Create([FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.Create(tourProblemReport);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<TourProblemReportDto> Update([FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.Update(tourProblemReport);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult Delete(int id)
        {
            var result = _tourProblemReportService.Delete(id);
            return CreateResponse(result);
        }

        /*
        [HttpPut("{id:int}/set-solving-deadline")]
        [Authorize(Policy = "administratorPolicy")]
        public ActionResult<TourProblemReportDto> SetSolvingDeadline(int id, [FromBody] DateTime solvingDeadline)
        {
            var result = _tourProblemReportService.SetSolvingDeadline(id, solvingDeadline);
            return CreateResponse(result);
        }
        */
    }
}
