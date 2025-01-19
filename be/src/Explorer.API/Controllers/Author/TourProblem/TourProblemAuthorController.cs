using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author.TourProblem
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tour-problem")]
    public class TourProblemAuthorController : BaseApiController
    {
        private readonly ITourProblemReportService _tourProblemReportService;

        public TourProblemAuthorController(ITourProblemReportService tourProblemReportService)
        {
            _tourProblemReportService = tourProblemReportService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<PagedResult<TourProblemReportDto>> GetByAuthorId(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourProblemReportService.GetByAuthorId(id, page, pageSize);
            return result;
        }
    }
}
