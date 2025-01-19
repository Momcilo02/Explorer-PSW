
using System;
﻿using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Explorer.Tours.Tests.Integration.Administration
{

    [Collection("Sequential")]
    public class TourExecutionCommandTests : BaseToursIntegrationTest
    {
        public TourExecutionCommandTests(ToursTestFactory factory) : base(factory) { }



        [Fact]
        public void StartBoughtTour()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();


            var purchasedTour = new TourDto
            {
                Id = 10,
                Name = "Tura10",
                Difficulty = API.Dtos.TourDifficulty.Hard,
                Description = "Planinski hajk",
                Cost = 220,
                Status = 0,
                Tags = "visina,priroda",
                Length = 0,
                TourDurations = new List<TourDurationDto>(),
                AuthorId = -12

            };
            //Act
            var actionResult = controller.StartNewTour(-21,purchasedTour);

            // Assert-response
            actionResult.ShouldNotBeNull();
            actionResult.ShouldBeOfType<StatusCodeResult>();
            var statusCodeResult = actionResult as StatusCodeResult;
            statusCodeResult.StatusCode.ShouldBe(StatusCodes.Status201Created);

            //Assert-database
            var storedTourExecution = dbContext.TourExecutions.FirstOrDefault(t => t.TourId == 10 && t.TouristId == -21);
            storedTourExecution.ShouldNotBeNull();
            storedTourExecution.TouristId.ShouldBe(-21);
            storedTourExecution.TourId.ShouldBe(purchasedTour.Id);
        }
        [Fact]
        public void LeaveStartedTour()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            //Act
            var activeTour = dbContext.TourExecutions.FirstOrDefault(t => t.Id == -2);
            activeTour.ShouldNotBeNull();
            var activeTourDTO = new TourExecutionDto
            {
                Id = -2,
                TourId = activeTour.TourId,
                TouristId = activeTour.TouristId,
                TourStartDate = activeTour.TourStartDate,
                TourEndDate = activeTour.TourEndDate,
                LastActivity = activeTour.LastActivity,
                CompletedPercentage = activeTour.CompletedPercentage,
                CurrentLatitude = activeTour.CurrentLatitude
            };
            var actionResult = controller.LeaveTour(activeTourDTO);


            //Assert
            var LeavedTour = dbContext.TourExecutions.FirstOrDefault(t => t.Id == -2);
            LeavedTour.ShouldNotBeNull();
            LeavedTour.TouristId.ShouldBe(-21);
            LeavedTour.TourId.ShouldBe(-2);
            LeavedTour.TourEndDate.ShouldBeLessThan(new DateTime(3030, 11, 11));
            Assert.True(LeavedTour.Status == Core.Domain.TourExecutions.ExecutionStatus.ABANDONED, "The status of the tour execution should be ABANDONED but it was not.");

        }
        [Fact]
        public void FinishStartedTour()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            //Act
            var actionResult = controller.FinishTour(-4);


            //Assert
            var FinishedTour = dbContext.TourExecutions.FirstOrDefault(t => t.Id == -4);
            FinishedTour.ShouldNotBeNull();
            FinishedTour.TouristId.ShouldBe(-22);
            FinishedTour.TourId.ShouldBe(-4);
            FinishedTour.TourEndDate.ShouldBeLessThan(new DateTime(3030, 11, 11));
            Assert.True(FinishedTour.Status == Core.Domain.TourExecutions.ExecutionStatus.COMPLETED, "The status of the tour execution should be COMPLETED but it was not.");

        }

        [Fact]
        public void FinishStartedTour_Unsuccessfully()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            //Act
            var actionResult = controller.FinishTour(-2);

            //Assert
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            badRequestResult.ShouldNotBeNull();  // Ensure the result is not null and is BadRequest
            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
            badRequestResult.Value.ShouldBe("Tour must be completed (CompletedPercentage = 100) to be finished");

        }

        [Fact]
        public void CheckLocation_ReturnsExpectedResult()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var touristLocation = new TouristLocationDto
            {
                TouristId = -22,
                Latitude = 10.0F,
                Longitude = 10.1F
            };

            // Act
            var actionResult = controller.CheckLocation(touristLocation, -6);
            var result = (actionResult as ObjectResult)?.Value as TourExecutionDto;

            // Assert-response
            result.ShouldNotBeNull();
            result.CurrentLatitude.ShouldBe(touristLocation.Latitude);
            result.CurrentLongitude.ShouldBe(touristLocation.Longitude);
            result.CompletedKeyPoints.Count.ShouldBe(2);

            //Assert-database
            var storedTourExecution = dbContext.TourExecutions.FirstOrDefault(t => t.Id == -6);
            storedTourExecution.ShouldNotBeNull();
            storedTourExecution.CurrentLatitude.ShouldBe(touristLocation.Latitude);
            storedTourExecution.CurrentLongitude.ShouldBe(touristLocation.Longitude);
            storedTourExecution.CompletedKeyPoints.Count.ShouldBe(2);
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
