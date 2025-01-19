using Explorer.API.Controllers.Tourist;
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

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourExecutionQueryTests : BaseToursIntegrationTest
    {
        public TourExecutionQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]

        public void Retrives_one()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetById(-1).Result)?.Value as TourExecutionDto;

            //Assert

            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.TourId.ShouldBe(-1);
            result.TouristId.ShouldBe(-21);

        }

        [Fact]
        public void Retrives_all_by_Tourist()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAllTouristTours(-21).Result)?.Value as List<TourExecutionDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);

        }

        [Fact]
        public void Retrive_Tourist_active_tour()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetActiveTour(-21).Result)?.Value as TourExecutionDto;

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
            result.TourId.ShouldBe(-2);

        }

        [Fact]
        public void Retrives_by_tourist_and_tour_ids()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetByUserAndTourIds(-22, -4).Result)?.Value as TourExecutionDto;

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-4);
            result.CompletedPercentage.ShouldBe(100);


        }

        [Fact]
        public void Retrieves_completed_key_points()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetCompletedKeyPoints(-6).Result)?.Value as List<KeyPointDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        private static TourExecutionController CreateController(IServiceScope scope)
        {
            return new TourExecutionController(scope.ServiceProvider.GetRequiredService<ITourExecutionService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}