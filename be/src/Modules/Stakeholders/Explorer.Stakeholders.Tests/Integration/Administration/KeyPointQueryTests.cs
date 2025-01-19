using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class KeyPointQueryTests : BaseStakeholdersIntegrationTest
    {
        public KeyPointQueryTests(StakeholdersTestFactory factory) : base(factory)
        {
        }
        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetPublic(0, 0).Result).Value  as PagedResult<KeyPointDto>;

            //Assert
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBe(3);

        }
        private static KeyPointController CreateController(IServiceScope scope)
        {
            return new KeyPointController(scope.ServiceProvider.GetRequiredService<IKeyPointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
