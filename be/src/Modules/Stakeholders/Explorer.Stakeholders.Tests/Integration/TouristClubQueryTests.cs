using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Tests;

namespace Explorer.Stakeholders.Tests.Integration
{
    [Collection("Sequential")]
    public class TouristClubQueryTests : BaseStakeholdersIntegrationTest
    {
        public TouristClubQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as BuildingBlocks.Core.UseCases.PagedResult<TouristClubDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);

        }


        private static TouristClubController CreateController(IServiceScope scope)
        {
            return new TouristClubController(scope.ServiceProvider.GetRequiredService<IToursitClubService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
