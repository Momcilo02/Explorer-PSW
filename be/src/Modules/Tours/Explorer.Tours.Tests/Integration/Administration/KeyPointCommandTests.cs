using Explorer.API.Controllers.Author.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class KeyPointCommandTests : BaseToursIntegrationTest
{
    public KeyPointCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var keyPointDto = new KeyPointDto
        {
            Name = "test",
            Description = "test description",
            Image = "testImage.jpg",
            Latitude = 10,
            Longitude = 10,
            Status = KeyPointDto.PublicStatus.PRIVATE,
            Comment = "Comment1"
        };

        //Act
        var result = ((ObjectResult)controller.Create(keyPointDto).Result)?.Value as KeyPointDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(keyPointDto.Name);

        //Assert - Database
        var storedKeyPoint = dbContext.KeyPoints.FirstOrDefault(i => i.Id == result.Id);
        storedKeyPoint.ShouldNotBeNull();
        storedKeyPoint.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var keyPoint = new KeyPointDto
        {
            Name = "",
            Description = "this is tescription",
            Image = "image.png",
            Latitude = 123,
            Longitude = 321,
            Status = KeyPointDto.PublicStatus.PRIVATE,
            Comment = "Comment1"
        };

        //Act
        var result = (ObjectResult)controller.Create(keyPoint).Result;

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }
    private static KeyPointController CreateController(IServiceScope scope)
    {
        return new KeyPointController(scope.ServiceProvider.GetRequiredService<IKeyPointService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
