using Explorer.API.Controllers.Administrator.TourProblem;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class TourProblemReportQueryTests : BaseStakeholdersIntegrationTest
    {
        public TourProblemReportQueryTests(StakeholdersTestFactory factory) : base(factory) { }
        [Fact]

        public void Retrieves_all()
        {

            //Arragne
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as BuildingBlocks.Core.UseCases.PagedResult<TourProblemReportDto>;


            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
        }

        private static TourProblemAdministratorController CreateController(IServiceScope scope)
        {
            return new TourProblemAdministratorController(scope.ServiceProvider.GetRequiredService<ITourProblemReportService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
