using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.TourProblem
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour-problem")]
    public class TourProblemTouristController : BaseApiController
    {
        private readonly ITourProblemReportService _tourProblemReportService;

        public TourProblemTouristController(ITourProblemReportService tourProblemReportService)
        {
            _tourProblemReportService = tourProblemReportService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<PagedResult<TourProblemReportDto>> GetByTouristId(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetByTouristId(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourProblemReportDto> Create([FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.Create(tourProblemReport);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourProblemReportDto> Update([FromBody] TourProblemReportDto tourProblemReport)
        {
            var result = _tourProblemReportService.Update(tourProblemReport);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourProblemReportService.Delete(id);
            return CreateResponse(result);
        }

        [HttpPut("setAsSolvedOrUnsolved/{id:int}")]
        public ActionResult<TourProblemReportDto> SetProblemAsSolvedOrUnsolved(int id, [FromBody] bool isSolved, [FromQuery] string comment)
        {
            var result = _tourProblemReportService.SetProblemAsSolvedOrUnsolved(id, isSolved, comment);
            return CreateResponse(result);
        }
    }
}
