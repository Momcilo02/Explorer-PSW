using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizController : BaseApiController
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        public ActionResult<QuizDto> Create([FromBody] QuizDto quizDto)
        {
            var result = _quizService.CreateQuiz(quizDto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet]
        public ActionResult<PagedResult<QuizDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = _quizService.GetAllQuizzes(page, pageSize);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public ActionResult<QuizDto> Update(int id, [FromBody] QuizDto quizDto)
        {
            var result = _quizService.UpdateQuiz(id, quizDto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _quizService.DeleteQuiz(id);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet("byTour/{tourId}")]
        public ActionResult<QuizDto> GetByTour(int tourId)
        {
            // Očekujemo da IQuizService ima metodu GetQuizByTourId(tourId)
            var result = _quizService.GetQuizByTourId(tourId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }
            return Ok(result.Value);
        }
    }
}
