using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Database;
using Xunit;
using Explorer.API.Controllers.User.ProfileAdministration;
using Explorer.Stakeholders.API.Public;

namespace Explorer.Stakeholders.Tests.Integration.Profile
{
    [Collection("Sequential")]
    public class EditProfileTests : BaseStakeholdersIntegrationTest
    {
        public EditProfileTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_edits_profile()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var userId = -21;
            var level = 3;
            var xp = 125;
            var lastWheelSpin = DateTime.UtcNow;
            var updateProfileDto = new PersonDto
            {
                Id = userId,
                UserId = userId,
                Name = "UpdatedName",
                Surname = "UpdatedSurname",
                Email = "updated.email@example.com",
                Biography = "Updated biography",
                Motto = "Updated motto",
                TouristLevel = level,
                TouristXp   = xp,
                LastWheelSpinTime = lastWheelSpin,
                ProfilePictureUrl = "https://example.com/newprofile.png",
                Following = new List<int>(),
                Followers = new List<int>(),
                ClubMember = new List<int>(),
                TouristStatus =TouristStatus.GOLDEN
            };

            // Act
            var result = controller.Edit(updateProfileDto);
            var objectResult = result.Result as ObjectResult;

            // Assert - Response
            objectResult.StatusCode.ShouldBe(200);
            var updatedProfile = objectResult.Value as PersonDto;
            updatedProfile.ShouldNotBeNull();
            updatedProfile.Name.ShouldBe(updateProfileDto.Name);
            updatedProfile.Surname.ShouldBe(updateProfileDto.Surname);
            updatedProfile.Email.ShouldBe(updateProfileDto.Email);

            // Assert - Database
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            dbContext.ChangeTracker.Clear();  // Osiguraj ažuriran kontekst baze podataka
            var storedPerson = dbContext.People.FirstOrDefault(p => p.UserId == userId);
            storedPerson.ShouldNotBeNull();
            storedPerson.Name.ShouldBe(updateProfileDto.Name);
            storedPerson.Surname.ShouldBe(updateProfileDto.Surname);
            storedPerson.Email.ShouldBe(updateProfileDto.Email);
            storedPerson.Biography.ShouldBe(updateProfileDto.Biography);
            storedPerson.Motto.ShouldBe(updateProfileDto.Motto);
            storedPerson.ProfilePictureUrl.ShouldBe(updateProfileDto.ProfilePictureUrl);
        }

        [Fact]
        public void Invalid_user_fails_edit_profile()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var userId = -9999; // Korisnik koji ne postoji
            var xp = 0;
            var level = 0;
            var lastWheelSpin = DateTime.UtcNow;
            var updateProfileDto = new PersonDto
            {
                Id = userId, // Dodaj ID korisnika
                UserId = 1, // Osiguraj da postaviš validan UserId
                Name = "UpdatedName",
                Surname = "UpdatedSurname",
                Email = "updated.email@example.com",
                TouristXp = xp,
                TouristLevel= level,
                LastWheelSpinTime= lastWheelSpin,
                Following = new List<int>(),
                Followers = new List<int>(),
                ClubMember = new List<int>(),
                TouristStatus = TouristStatus.GOLDEN
            };

            // Act
            var result = controller.Edit(updateProfileDto);
            var objectResult = result.Result as ObjectResult;

            // Assert
            objectResult.StatusCode.ShouldBe(404);
        }

        private static ProfileEditingController CreateController(IServiceScope scope)
        {
            return new ProfileEditingController(scope.ServiceProvider.GetRequiredService<IPersonEditingService>());
        }
    }
}
