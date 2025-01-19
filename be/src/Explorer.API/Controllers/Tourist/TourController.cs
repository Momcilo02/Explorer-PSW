using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.API.Dtos;

namespace Explorer.API.Controllers.Tourist
{
    [Route("api/tours")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> Get([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
        [HttpPut("{id:int}/hasQuiz")]
        public ActionResult<TourDto> SetHasQuiz(int id, [FromBody] bool hasQuiz)
        {
            var result = _tourService.SetHasQuiz(id, hasQuiz);
            return CreateResponse(result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<TourDto> GetById(int id)
        {
            var result = _tourService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet]
        [Route("published")]
        public ActionResult<PagedResult<TourDto>> GetPublished([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPublishedTour(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet]
        [Route("published/preview/{id:long}")]
        public ActionResult<TourDto> GetPublishedTourPreview(long id)
        {
            var result = _tourService.GetPublishedTourPreview(id);
            return CreateResponse(result);
        }

        [HttpGet]
        [Route("published/{id:int}")]
        public ActionResult<TourDto> GetPublishedTourById(long id)
        {
            var result = _tourService.GetPublishedTourById(id);
            return CreateResponse(result);
        }
        [HttpGet]
        [Route("search")]
        public ActionResult<List<TourDto>> GetTourByDistance([FromQuery] float latitude, [FromQuery] float longitude,[FromQuery] float distance)
        {
            var result = _tourService.GetTourByDistance(latitude, longitude,distance);
            return CreateResponse(result);
        }
    }
}