using Explorer.API.Controllers.Tourist.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection(name: "Sequential")]
    public class TouristEquipmentCommandTests : BaseToursIntegrationTest
    {
        public TouristEquipmentCommandTests(ToursTestFactory factory) : base(factory) { }
        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TouristEquipmentDto
            {
                Id = -4,
                TouristId = -21,
                EquipmentId = -1
            };

            //Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TouristId.ShouldBe(newEntity.TouristId);

            //Assert - Database
            var storedEntity = dbContext.TouristEquipments.FirstOrDefault(i => i.Id == newEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);

        }
        [Fact]
        public void Create_fails_invalid_data()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TouristEquipmentDto
            {
                Id = -5,
                TouristId = -1,
                EquipmentId = 0
            };

            //Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }
        [Fact]
        public void Updates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updatedEntity = new TouristEquipmentDto
            {
                Id = -2,
                TouristId = -22,
                EquipmentId = -2
            };

            //Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TouristEquipmentDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
            result.TouristId.ShouldBe(updatedEntity.TouristId);
            result.EquipmentId.ShouldBe(updatedEntity.EquipmentId);

            //Assert - Database
            var storedEntity = dbContext.TouristEquipments.FirstOrDefault(i => i.EquipmentId == -2);
            storedEntity.ShouldNotBeNull();
            storedEntity.TouristId.ShouldBe(updatedEntity.TouristId);
            var oldEntity = dbContext.TouristEquipments.FirstOrDefault(i => i.Id == -2);
            oldEntity.ShouldNotBeNull();

        }
        [Fact]
        public void Update_fails_invalid_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TouristEquipmentDto
            {
                Id = -1000,
                EquipmentId = -21,
                TouristId=4
            };

            //Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }
        [Fact]
        public void Deletes()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            //Act
            var result = (OkResult)controller.Delete(-1);

            //Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            //Assert - Database
            var storedCourse = dbContext.TouristEquipments.FirstOrDefault(i => i.Id == -1);
            storedCourse.ShouldBeNull();
        }
        [Fact]
        public void Delete_fails_invalid_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = (ObjectResult)controller.Delete(-1000);

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }
        private static TouristEquipmentController CreateController(IServiceScope scope)
        {
            return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}