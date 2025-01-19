using Explorer.API.Controllers.User.ProfileAdministration;
using Explorer.API.Controllers.User.TourProblem;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration
{
    [Collection("Sequential")]
    public class ClubMessageQueryTest : BaseStakeholdersIntegrationTest
    {
        public ClubMessageQueryTest(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retreives_all_for_both_users()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetMessagesForTouristClub(-1).Result)?.Value as BuildingBlocks.Core.UseCases.PagedResult<ClubMessageDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
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
