using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using Explorer.Tours.Core.Domain;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourExecutions")]
    public class TourExecutionController : BaseApiController
    {
        private readonly ITourExecutionService _tourExecutionService;

        public TourExecutionController(ITourExecutionService tourExecutionService)
        {
            _tourExecutionService = tourExecutionService;
        }
        [HttpGet]
        public ActionResult<PagedResult<TourExecutionDto>> GetPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourExecutionService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TourExecutionDto> GetById(int id)
        {
            var result = _tourExecutionService.Get(id);
            return CreateResponse(result);
        }


        [HttpGet("{touristId:int}/{tourId:int}")]
        public ActionResult<TourExecutionDto> GetByUserAndTourIds(int touristId, int tourId)
        {
            var result = _tourExecutionService.GetByUserAndTourIds(touristId, tourId);
            return CreateResponse(result);
        }

        [HttpPost("startNewTour/{touristId:int}")]
        public IActionResult StartNewTour(int touristId,[FromBody] TourDto tour)
        {
            TourExecutionDto tourExecution = new TourExecutionDto();
            tourExecution.TourId = tour.Id;
            tourExecution.TouristId = touristId;

            var result = _tourExecutionService.StartNewTour(tourExecution);
 
            if (result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPut("leaveTour")]
        public ActionResult<TourExecutionDto> LeaveTour([FromBody] TourExecutionDto tourExecution)
        {
            var result = _tourExecutionService.LeaveTour(tourExecution.TouristId, tourExecution.TourId);
            return CreateResponse(result);
        }

        [HttpPut("finishTour/{tourExecutionId:int}")]
        public ActionResult<TourExecutionDto> FinishTour(int tourExecutionId)
        {

            var tourExecution = _tourExecutionService.Get(tourExecutionId).Value;
            if (tourExecution.CompletedPercentage == 100.0)
            {
                var result = _tourExecutionService.FinishTour(tourExecutionId);
                return CreateResponse(result);
            }
            else
            {
                return BadRequest("Tour must be completed (CompletedPercentage = 100) to be finished");
            }
        }


        [HttpPut("checkLocation/{id:int}")]
        public ActionResult CheckLocation([FromBody] TouristLocationDto touristLocation, int id)
        {
            var result = _tourExecutionService.CheckLocation(id, touristLocation.Latitude, touristLocation.Longitude);

            return CreateResponse(result);

        }
        [HttpGet("completed/{id:int}")]
        public ActionResult<TourExecutionDto> GetCompletedKeyPoints(int id)
        {
            var result = _tourExecutionService.GetCompletedKeyPoints(id);
            return CreateResponse(result);
        }

        [HttpGet("allMyTours/{touristId:int}")]
        public ActionResult<List<TourExecutionDto>> GetAllTouristTours(int touristId)
        {   
            var tourExecutions = _tourExecutionService.GetAllTouristTours(touristId);
            return CreateResponse(tourExecutions);

        }

        [HttpGet("activeTour/{touristId:int}")]
        public ActionResult<TourExecutionDto> GetActiveTour(int touristId)
        {
            var activeTour = _tourExecutionService.GetActiveTour(touristId);
            return CreateResponse(activeTour);
        }


    }

}

