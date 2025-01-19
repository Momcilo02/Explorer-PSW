using Explorer.API.Controllers.Tourist.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Tests;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection(name: "Sequential")]
    public class TouristEquipmentQueryTests : BaseToursIntegrationTest
    {
        public TouristEquipmentQueryTests(ToursTestFactory factory) : base(factory) { }
        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAll(page: 0, pageSize: 0).Result)?.Value as PagedResult<TouristEquipmentDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
        }
        private static TouristEquipmentController CreateController(IServiceScope scope)
        {
            return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>())
            {
                ControllerContext = BuildContext(id: "-1")
            };
        }
    }
}

