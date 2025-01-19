using Explorer.API.Controllers.User.TourProblem;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration
{
    [Collection("Sequential")]
    public class ClubMessageCommandTests : BaseStakeholdersIntegrationTest
    {
        public ClubMessageCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new ClubMessageDto
            {
                SenderId = -11,
                TouristClubId = -1,
                SentDate = DateTime.UtcNow,
                Content = "sadrzaj test poruke"

            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubMessageDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Content.ShouldBe(newEntity.Content);

            // Assert - Database
            var storedEntity = dbContext.ClubMessages.FirstOrDefault(i => i.Content == newEntity.Content);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new ClubMessageDto
            {
                Id = -2,
                SenderId = -11,
                TouristClubId = -1,
                SentDate = DateTime.UtcNow,
                Content = "sadrzaj izmenjen"
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity, -2).Result)?.Value as ClubMessageDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
            result.Content.ShouldBe(updatedEntity.Content);

            // Assert - Database
            var storedEntity = dbContext.ClubMessages.FirstOrDefault(i => i.Content == "sadrzaj izmenjen");
            storedEntity.ShouldNotBeNull();
            storedEntity.Content.ShouldBe(updatedEntity.Content);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Act
            var result = controller.Delete(-1);

            // Assert - Response
            result.ShouldNotBeNull();

            // Assert - Database
            var storedCourse = dbContext.ClubMessages.FirstOrDefault(i => i.Id == -1);
            storedCourse.ShouldBeNull();
        }


        private static ClubMessageController CreateController(IServiceScope scope)
        {
            return new ClubMessageController(scope.ServiceProvider.GetRequiredService<IClubMessageService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
