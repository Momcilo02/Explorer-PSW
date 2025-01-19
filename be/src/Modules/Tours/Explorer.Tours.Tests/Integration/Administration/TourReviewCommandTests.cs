using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourReviewCommandTests : BaseToursIntegrationTest
    {
        public TourReviewCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newReview = new TourReviewDto
            {
                TourId = 1,
                Rating = 5,
                Comment = "Absolutely loved this tour! The guide was amazing and the views were breathtaking.",
                TouristId = 1,
                VisitDate = DateTime.UtcNow.AddDays(-2),
                ReviewDate = DateTime.UtcNow.AddDays(-1),
                CompletedPercentage = 60,
                Images = new List<string> { "image1.jpg", "image2.jpg" }
            };

            // Act
            var result = ((ObjectResult)controller.Create(newReview).Result)?.Value as TourReviewDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TourId.ShouldBe(newReview.TourId);
            result.Rating.ShouldBe(newReview.Rating);
            result.Comment.ShouldBe(newReview.Comment);
            result.TouristId.ShouldBe(newReview.TouristId);
            result.VisitDate.ShouldBe(newReview.VisitDate);
            result.ReviewDate.ShouldBe(newReview.ReviewDate);
            result.CompletedPercentage.ShouldBe(newReview.CompletedPercentage);
            result.Images.ShouldBe(newReview.Images);

            // Assert - Database
            var storedReview = dbContext.TourReview.FirstOrDefault(r => r.Id == result.Id);
            storedReview.ShouldNotBeNull();
            storedReview.TourId.ShouldBe(newReview.TourId);
            storedReview.Rating.ShouldBe(newReview.Rating);
            storedReview.Comment.ShouldBe(newReview.Comment);
            storedReview.TouristId.ShouldBe(newReview.TouristId);
            storedReview.VisitDate.ShouldBe(newReview.VisitDate);
            storedReview.ReviewDate.ShouldBe(newReview.ReviewDate);
            storedReview.CompletedPercentage.ShouldBe(newReview.CompletedPercentage);
            storedReview.Images.ShouldBe(newReview.Images);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidReview = new TourReviewDto
            {
                // Missing TourId and Rating, which should trigger validation error
                Comment = "Incomplete review",
                TouristId = 101,
                VisitDate = DateTime.UtcNow,
                ReviewDate = DateTime.UtcNow
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidReview).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = (OkResult)controller.Delete(-3);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedReview = dbContext.TourReview.FirstOrDefault(r => r.Id == -3);
            storedReview.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReview = new TourReviewDto
            {
                Id = -1,
                TourId = 1,
                Rating = 4,
                Comment = "Updated comment: Great tour, but could be improved.",
                TouristId = 101,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                ReviewDate = DateTime.UtcNow.AddDays(-4),
                CompletedPercentage = 60,
                Images = new List<string> { "updated_image1.jpg" }
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedReview).Result)?.Value as TourReviewDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(updatedReview.Id);
            result.TourId.ShouldBe(updatedReview.TourId);
            result.Rating.ShouldBe(updatedReview.Rating);
            result.Comment.ShouldBe(updatedReview.Comment);
            result.TouristId.ShouldBe(updatedReview.TouristId);
            result.VisitDate.ShouldBe(updatedReview.VisitDate);
            result.ReviewDate.ShouldBe(updatedReview.ReviewDate);
            result.CompletedPercentage.ShouldBe(updatedReview.CompletedPercentage);
            result.Images.ShouldBe(updatedReview.Images);

            // Assert - Database
            var storedReview = dbContext.TourReview.FirstOrDefault(r => r.Id == updatedReview.Id);
            storedReview.ShouldNotBeNull();
            storedReview.Rating.ShouldBe(updatedReview.Rating);
            storedReview.Comment.ShouldBe(updatedReview.Comment);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedReview = new TourReviewDto
            {
                Id = -1000,
                TourId = 1,
                Rating = 3,
                Comment = "Invalid update",
                TouristId = 101,
                VisitDate = DateTime.UtcNow,
                ReviewDate = DateTime.UtcNow
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedReview).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static TourReviewController CreateController(IServiceScope scope)
        {
            return new TourReviewController(scope.ServiceProvider.GetRequiredService<ITourReviewService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
