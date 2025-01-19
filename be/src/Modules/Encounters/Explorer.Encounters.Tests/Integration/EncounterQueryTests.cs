using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Administrator.TourProblem;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;

namespace Explorer.Encounters.Tests.Integration
{
    public class EncounterQueryTests : BaseEncountersIntegrationTest
    {
        public EncounterQueryTests(EncountersTestFactory factory) : base(factory) { }
        [Fact]
        public void Retrieves_all()
        {
            //Arragne
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAllEncounters().Result)?.Value as List<EncounterDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);
        }

        private static AdminEncounterController CreateController(IServiceScope scope)
        {
            return new AdminEncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
