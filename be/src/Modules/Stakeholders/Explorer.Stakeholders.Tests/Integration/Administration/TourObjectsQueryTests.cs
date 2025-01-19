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
    public class TourObjectsQueryTests : BaseStakeholdersIntegrationTest
    {
        public TourObjectsQueryTests(StakeholdersTestFactory factory) : base(factory) { }
        public void Retrives_all_public()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetPublic(0, 0).Result).Value as PagedResult<TourObjectDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(0);
        }
        private static TourObjectController CreateController(IServiceScope scope)
        {
            return new TourObjectController(scope.ServiceProvider.GetRequiredService<ITourObjectService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
