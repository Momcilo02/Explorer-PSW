using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Explorer.Blog.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogCommandTest : BaseBlogIntegrationTest
    {
        public BlogCommandTest(BlogTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            var newEntity = new BlogDto
            {
                Title = "test",
                Description = "test",
                Date = DateOnly.FromDateTime(DateTime.Today),
                ImageUrl = new List<string>(),
                ActivityStatus = (API.Dtos.BlogActivityStatus)Blog.Core.Domain.BlogActivityStatus.regular,
                Status = (API.Dtos.BlogStatus)Blog.Core.Domain.BlogStatus.draft
            };

            //Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Title.ShouldBe(newEntity.Title);

            //Assert - Database
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Title == newEntity.Title);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);

        }
        [Fact]
        public void UpdatesRating() {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            int blogId = -1;
            var rating = new RatingDto
            {
                UserId = -1,
                CreationTime = DateTime.UtcNow,
                Grade = true
            };

            //Act
            var result = ((ObjectResult)controller.UpdateRating(blogId, rating).Result)?.Value as BlogDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Id.ShouldBe(-1);

            //Assert - Database
            var stored = dbContext.Blogs.FirstOrDefault(b => b.Id == result.Id);
            stored.ShouldNotBeNull();
        }

        private static BlogController CreateController(IServiceScope scope)
        {
            return new BlogController(scope.ServiceProvider.GetRequiredService<IBlogService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
