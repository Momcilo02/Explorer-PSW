using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour-review")]
    public class TourReviewController : BaseApiController
    {
        private readonly ITourReviewService _tourReviewService;
        public TourReviewController(ITourReviewService tourReviewService)
        {
            _tourReviewService = tourReviewService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourReviewDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourReviewService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourReviewDto> Create([FromBody] TourReviewDto review)
        {
            review.TouristId = User.PersonId() + 1;
            var result = _tourReviewService.Create(review);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourReviewDto> Update([FromBody] TourReviewDto review)
        {
            review.TouristId = User.PersonId() + 1;
            var result = _tourReviewService.Update(review);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourReviewService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<PagedResult<TourReviewDto>> GetReviewsByTourId(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourReviewService.GetReviewsByTourId(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("grade/{tourId:int}")]
        public ActionResult<double> GetAverageGrade(int tourId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourReviewService.GetAverageGrade(tourId, page, pageSize);
            return CreateResponse(result);
        }
    }
}
