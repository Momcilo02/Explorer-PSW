using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogService :BaseService<BlogDto, Core.Domain.Blog>, IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        public BlogService(IMapper mapper, IBlogRepository blogRepository) : base(mapper) {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public Result<BlogDto> Create(BlogDto blog)
        {
            try
            {
                var result = _blogRepository.Create(MapToDomain(blog));
                return MapToDto(result);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<BlogDto> Get(int id)
        {
            try
            {
                var result = _blogRepository.Get(id);
                return MapToDto(result);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<PagedResult<BlogDto>> GetPaged(int page, int pageSize)
        {
            var result = _blogRepository.GetPaged(page, pageSize);
            return MapToDto(result);
        }

        public Result<BlogDto> Update(BlogDto blog)
        {
            throw new NotImplementedException();
        }

        public Result<BlogDto> UpdateRating(int id, RatingDto rationg)
        {
            try
            {
                Domain.Blog b = _blogRepository.Get(id);
                var rating = _mapper.Map<Rating>(rationg);
                b.UpdateRating(rating);
                var result = _blogRepository.Update(b);
                return MapToDto(result);
            }
            catch (ArgumentException e) {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            

        }
        public Result<BlogDto> UpdateBlogStatus(int blogId)
        {
            try
            {
                var blog = _blogRepository.Get(blogId);
                if (blog == null)
                    return Result.Fail(FailureCode.NotFound).WithError("Blog not found");

                blog.UpdateActivityStatus();
                var updatedBlog = _blogRepository.Update(blog);
                return MapToDto(updatedBlog);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result<BlogDto> AddComment(int blogId, CommentDto commentDto)
        {
            var blog = _blogRepository.Get(blogId);
            if (blog == null)
                return Result.Fail(FailureCode.NotFound).WithError("Blog not found");

            var comment = _mapper.Map<Comment>(commentDto);
            blog.AddComment(comment);
            var updatedBlog = _blogRepository.Update(blog);
            return MapToDto(updatedBlog);
        }

        public Result<BlogDto> UpdateComment(int blogId, int commentId, string newText)
        {
            var blog = _blogRepository.Get(blogId);
            if (blog == null)
                return Result.Fail(FailureCode.NotFound).WithError("Blog not found");

            blog.UpdateComment(commentId, newText);
            var updatedBlog = _blogRepository.Update(blog);
            return MapToDto(updatedBlog);
        }

        public Result<BlogDto> DeleteComment(int blogId, int commentId)
        {
            var blog = _blogRepository.Get(blogId);
            if (blog == null)
                return Result.Fail(FailureCode.NotFound).WithError("Blog not found");

            blog.DeleteComment(commentId);
            var updatedBlog = _blogRepository.Update(blog);
            return MapToDto(updatedBlog);
        }
        public Result<List<BlogDto>> GetActiveBlogs()
        {
            var activeBlogs = _blogRepository.GetByActivityStatus(Domain.BlogActivityStatus.active);
            return Result.Ok(activeBlogs.Select(MapToDto).ToList());
        }

        public Result<List<BlogDto>> GetFamousBlogs()
        {
            var famousBlogs = _blogRepository.GetByActivityStatus(Domain.BlogActivityStatus.famous);
            return Result.Ok(famousBlogs.Select(MapToDto).ToList());
        }

        public Result<BlogDto> PublishBlog(int id)
        {
            try
            {
                Domain.Blog b = _blogRepository.Get(id);
                b.PublishBlog();
                var result = _blogRepository.Update(b);
                return MapToDto(result);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }
        public Result<BlogDto> CloseBlog(int id)
        {
            try
            {
                Domain.Blog b = _blogRepository.Get(id);
                b.CloseBlog();
                var result = _blogRepository.Update(b);
                return MapToDto(result);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

    }
}
