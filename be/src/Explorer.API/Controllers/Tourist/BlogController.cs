using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/blogs")]
    public class BlogController : BaseApiController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public ActionResult<PagedResult<BlogDto>> Get([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _blogService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<BlogDto> Create([FromBody] BlogDto blog)
        {
            var result = _blogService.Create(blog);
            return CreateResponse(result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<BlogDto> GetById(int id)
        {
            var result = _blogService.Get(id);
            return CreateResponse(result);
        }

        [HttpPut("rating/{id:int}")]
        public ActionResult<BlogDto> UpdateRating(int id, [FromBody] RatingDto rating)
        {
            rating.CreationTime = DateTime.Now;
            var result = _blogService.UpdateRating(id, rating);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<BlogDto> Update([FromBody] BlogDto blog)
        {
            var result = _blogService.Update(blog);
            return CreateResponse(result);
        }
        [HttpGet("active")]
        public ActionResult<List<BlogDto>> GetActiveBlogs()
        {
            var result = _blogService.GetActiveBlogs();
            return CreateResponse(result);
        }

        // New endpoint to get famous blogs
        [HttpGet("famous")]
        public ActionResult<List<BlogDto>> GetFamousBlogs()
        {
            var result = _blogService.GetFamousBlogs();
            return CreateResponse(result);
        }
        [HttpPost("{id:int}/comments")]
        public ActionResult<BlogDto> AddComment(int id, [FromBody] CommentDto commentDto)
        {
            var result = _blogService.AddComment(id, commentDto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPut("{id:int}/comments/{commentId:int}")]
        public ActionResult<BlogDto> UpdateComment(int id, int commentId, [FromBody] string newText)
        {
            var result = _blogService.UpdateComment(id, commentId, newText);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete("{id:int}/comments/{commentId:int}")]
        public ActionResult<BlogDto> DeleteComment(int id, int commentId)
        {
            var result = _blogService.DeleteComment(id, commentId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
        [HttpPut("publish/{id:int}")]
        public ActionResult<BlogDto> Publish(int id, [FromBody] BlogDto blog)
        {
            var result = _blogService.PublishBlog(id);
            return CreateResponse(result);
        }

        [HttpPut("close/{id:int}")]
        public ActionResult<BlogDto> Close(int id, [FromBody] BlogDto blog)
        {
            var result = _blogService.CloseBlog(id);
            return CreateResponse(result);
        }
    }
}
