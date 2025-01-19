using Explorer.API.Controllers.User;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CommentCommandTest : BaseBlogIntegrationTest
    {
        public CommentCommandTest(BlogTestFactory factory) : base(factory) { }

        [Fact]
        public void CreatesComment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            var newComment = new CommentDto
            {
                UserId = 1,  // Korisnički ID je integer
                Text = "This is a test comment",
                Username = "mika",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(newComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.UserId.ShouldBe(newComment.UserId);
            result.Text.ShouldBe(newComment.Text);

            // Assert - Database
            var storedEntity = dbContext.Comments.FirstOrDefault(i => i.Text == newComment.Text);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
            storedEntity.UserId.ShouldBe(newComment.UserId);  // Proveravamo UserId
        }

        [Fact]
        public void UpdatesComment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var updatedComment = new CommentDto
            {
                Id = -1,
                UserId = 2,  // Ažuriramo UserId na 2
                Username = "mika",
                Text = "Updated comment text",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                LastModified = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.UserId.ShouldBe(updatedComment.UserId);  // Proveravamo ažurirani UserId
            result.Text.ShouldBe(updatedComment.Text);

            // Assert - Database
            var storedEntity = dbContext.Comments.FirstOrDefault(i => i.Id == -1);
            storedEntity.ShouldNotBeNull();
            storedEntity.UserId.ShouldBe(updatedComment.UserId);  // Proveravamo ažurirani UserId
            storedEntity.Text.ShouldBe(updatedComment.Text);
        }

        [Fact]
        public void DeletesComment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var existingComment = new Comment(1, "mika", "Comment to delete");  // Kreiramo komentar sa UserId = 1
            dbContext.Comments.Add(existingComment);
            dbContext.SaveChanges();

            // Act
            var result = (OkResult)controller.Delete((int)existingComment.Id);

            // Assert - Response
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var deletedEntity = dbContext.Comments.FirstOrDefault(i => i.Id == existingComment.Id);
            deletedEntity.ShouldBeNull();
        }

        private static CommentController CreateController(IServiceScope scope)
        {
            return new CommentController(scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
