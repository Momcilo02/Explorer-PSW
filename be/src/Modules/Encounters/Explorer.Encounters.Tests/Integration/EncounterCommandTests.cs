using Explorer.API.Controllers.Administrator.TourProblem;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Infrastructure.Database;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration
{
    [Collection("Sequential")]
    public class EncounterCommandTests : BaseEncountersIntegrationTest
    {
        public EncounterCommandTests(EncountersTestFactory factory) : base(factory) { }
        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateAdministratorController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var newEntity = new EncounterDto
            {
                Id = -4,
                Name = "Izazov 4",
                Description = "Opis izazova 4",
                TotalXp = 50,
                CreatorId = -1,
                Longitude = 19.315318,
                Latitude = 20.484510,
                Status = EncounterStatus.ACTIVE,
                EncounterType = EncounterType.SOCIAL,
                TouristRequestStatus = TouristEncounterStatus.NOTTOURISTENCOUNTER,
                isTourRequired = false,
                TourId = null,
                ActivateRange = 90,
                PeopleNumb = 7
            };

            //Act
            var result = ((ObjectResult)controller.CreateEncounter(newEntity).Result)?.Value as EncounterDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);
            result.Description.ShouldBe(newEntity.Description);
            result.TotalXp.ShouldBe(newEntity.TotalXp);
            result.CreatorId.ShouldBe(newEntity.CreatorId);
            result.Longitude.ShouldBe(newEntity.Longitude);
            result.Latitude.ShouldBe(newEntity.Latitude);
            result.Status.ShouldBe(newEntity.Status);
            result.EncounterType.ShouldBe(newEntity.EncounterType);
            result.isTourRequired.ShouldBe(newEntity.isTourRequired);
            result.TourId.ShouldBe(newEntity.TourId);
            result.ActivateRange.ShouldBe(newEntity.ActivateRange);
            result.PeopleNumb.ShouldBe(newEntity.PeopleNumb);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Id == newEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateAdministratorController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            var updatedEntity = new EncounterDto
            {
                Id = -2,
                Name = "Izazov 2",
                Description = "Novi opis izazova 2",
                TotalXp = 60,
                CreatorId = -2,
                Longitude = 20.54545,
                Latitude = 20.54554,
                Status = EncounterStatus.ACTIVE,
                EncounterType = EncounterType.HIDDENLOCATION,
                TouristRequestStatus = TouristEncounterStatus.NOTTOURISTENCOUNTER,
                isTourRequired = false,
                TourId = null,
                ActivateRange = 150,
                Image = "novaSlika.jpg",
                ImageLongitude = 19.5454548,
                ImageLatitude = 20.485742
            };

            // Act
            var result = ((ObjectResult)controller.UpdateEncounter(updatedEntity, (int)updatedEntity.Id).Result)?.Value as EncounterDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Image.ShouldBe(updatedEntity.Image);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Id == updatedEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(updatedEntity.Description);
        }
        private static AdminEncounterController CreateAdministratorController(IServiceScope scope)
        {
            return new AdminEncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
