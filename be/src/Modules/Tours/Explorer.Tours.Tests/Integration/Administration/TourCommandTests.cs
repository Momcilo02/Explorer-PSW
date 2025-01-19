using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain;


namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class TourCommandTests : BaseToursIntegrationTest
{
    public TourCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Name = "Tura4",
            Difficulty = API.Dtos.TourDifficulty.Easy,
            Description = "Planinski hajk",
            Cost = 200,
            Status = 0,
            Tags = "visina,priroda",
            Length = 0,
            TourDurations = new List<TourDurationDto>(),
            AuthorId = -12,
            Image="testImage1.jpg"
        };

        //Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

        //Assert - Response

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        //Assert - Database

        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }
    //Ukoliko se pokusa kreirati nekompletan entitet
    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var updatedEntity = new TourDto
        {
            Name = "",
            Difficulty = API.Dtos.TourDifficulty.Easy,
            Description = "Planinski hajk",
            Cost = 0,
            Status = 0,
            Tags = "visina,priroda",
            Length = 0,
            TourDurations = new List<TourDurationDto>(),
            AuthorId = -12,
            Image="testImage1.jpg"

        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Tours.FirstOrDefault(i => i.Id == -3);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");

        // Act
        var result = (ObjectResult)controller.Delete(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-12");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new TourDto
        {
            Id = -1,
            Name = "Tura1",
            Difficulty = API.Dtos.TourDifficulty.Hard,
            Description = "Planinski hajk",
            Cost = 200,
            Status = 0,
            Tags = "visina,priroda",
            TourDurations = new List<TourDurationDto>(),
            Length = 0,
            AuthorId = -12,
            Image="testImage1.jpg"
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Difficulty.ShouldBe(updatedEntity.Difficulty);
        result.Description.ShouldBe(updatedEntity.Description);
        result.Cost.ShouldBe(updatedEntity.Cost);
        result.Tags.ShouldBe(updatedEntity.Tags);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Difficulty == Core.Domain.TourDifficulty.Hard);
        storedEntity.ShouldNotBeNull();
        storedEntity.Difficulty.ShouldBe((Core.Domain.TourDifficulty)updatedEntity.Difficulty);
        var oldEntity = dbContext.Tours.FirstOrDefault(i => i.Id==-1 && i.Difficulty == Core.Domain.TourDifficulty.Easy);
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-12");
        var updatedEntity = new TourDto
        {
            Id = -1000,
            Name = "Tura1",
            AuthorId = -12
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    public enum TourStatus
    {
        Draft = 0,
        Published = 1,
        Archived = 2
    }

    [Theory]
    [MemberData(nameof(PublishData))]
    public void Publishes(string authorId, TourDto tour, int expectedResponseCode, TourStatus expectedStatus)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, authorId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (ObjectResult)controller.Publish(tour).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(expectedResponseCode);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tour.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ShouldBe((Core.Domain.TourStatus)expectedStatus);
    }

    [Theory]
    [MemberData(nameof(ArchiveData))]
    public void Archives(string authorId, TourDto tour, int expectedResponseCode, TourStatus expectedStatus)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, authorId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (ObjectResult)controller.Archive(tour).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(expectedResponseCode);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tour.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ShouldBe((Core.Domain.TourStatus)expectedStatus);
    }

    [Theory]
    [MemberData(nameof(ReactivateData))]
    public void Reactive(string authorId, TourDto tour, int expectedResponseCode, TourStatus expectedStatus)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, authorId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (ObjectResult)controller.ReactivateTour(tour).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(expectedResponseCode);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tour.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ShouldBe((Core.Domain.TourStatus)expectedStatus);
    }

    public static IEnumerable<object[]> PublishData()
    {
        return new List<object[]>
        {
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -1,
                    Name = "Tura1",
                    Difficulty = API.Dtos.TourDifficulty.Easy,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = 0,
                    Tags = "visina,priroda",
                    Image="testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>(),
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime = null
                },
                400,
                TourStatus.Draft
            },
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -1,
                    Name = "Tura1",
                    Difficulty = API.Dtos.TourDifficulty.Easy,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = 0,
                    Tags = "visina,priroda",
                    Image="testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>
                    {
                         new KeyPointDto
                            {
                            Id = -1,
                            Name = "test1",
                            Description = "testDescription1",
                            Image = "testImage1.jpg",
                            Latitude = 10,
                            Longitude = 101,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                            new KeyPointDto
                            {
                            Id = -2,
                            Name = "test2",
                            Description = "testDescription2",
                            Image = "testImage2.jpg",
                            Latitude = 20,
                            Longitude = 22,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                    },
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime=null
                },
                200,
                TourStatus.Published
            },
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -2,
                    Name = null,
                    Difficulty = 0,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = 0,
                    Tags = "visina,priroda",
                    Image="testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>
                    {
                         new KeyPointDto
                            {
                            Id = -1,
                            Name = "test1",
                            Description = "testDescription1",
                            Image = "testImage1.jpg",
                            Latitude = 10,
                            Longitude = 101,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                            new KeyPointDto
                            {
                            Id = -2,
                            Name = "test2",
                            Description = "testDescription2",
                            Image = "testImage2.jpg",
                            Latitude = 20,
                            Longitude = 22,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                    },
                    TourDurations = new List<TourDurationDto>(),
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime=null
                },
                400,
                TourStatus.Draft
            },
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -4,
                    Name = null,
                    Difficulty = 0,
                    Description = "Plivanje",
                    Cost = 50,
                    Status = 0,
                    Tags = "more,voda",
                    Image = "testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>
                    {
                         new KeyPointDto
                            {
                            Id = -1,
                            Name = "test1",
                            Description = "testDescription1",
                            Image = "testImage1.jpg",
                            Latitude = 10,
                            Longitude = 101,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                            new KeyPointDto
                            {
                            Id = -2,
                            Name = "test2",
                            Description = "testDescription2",
                            Image = "testImage2.jpg",
                            Latitude = 20,
                            Longitude = 22,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                    },
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime= null
                },
                400,
                TourStatus.Draft
            },
        };


    }

    public static IEnumerable<object[]> ArchiveData()
    {
        return new List<object[]>
        {
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -1,
                    Name = "Tura1",
                    Difficulty = API.Dtos.TourDifficulty.Easy,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = (API.Dtos.TourStatus)TourStatus.Draft,
                    Tags = "visina,priroda",
                    Image="testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>(),
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime = null
                },
                400,
                TourStatus.Draft
            },
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -1,
                    Name = "Tura1",
                    Difficulty = API.Dtos.TourDifficulty.Easy,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = (API.Dtos.TourStatus)TourStatus.Published,
                    Tags = "visina,priroda",
                    Image = "testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>
                    {
                         new KeyPointDto
                            {
                            Id = -1,
                            Name = "test1",
                            Description = "testDescription1",
                            Image = "testImage1.jpg",
                            Latitude = 10,
                            Longitude = 101,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                            new KeyPointDto
                            {
                            Id = -2,
                            Name = "test2",
                            Description = "testDescription2",
                            Image = "testImage2.jpg",
                            Latitude = 20,
                            Longitude = 22,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                    },
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime=null
                },
                200,
                TourStatus.Archived
            }  
        };
    }

    public static IEnumerable<object[]> ReactivateData()
    {
        return new List<object[]>
        {
            new object[]
            {
                "-12",
                new TourDto
                {
                    Id = -1,
                    Name = "Tura1",
                    Difficulty = API.Dtos.TourDifficulty.Easy,
                    Description = "Planinski hajk",
                    Cost = 200,
                    Status = (API.Dtos.TourStatus)TourStatus.Archived,
                    Tags = "visina,priroda",
                    Image="testImage1.jpg",
                    KeyPoints = new List<KeyPointDto>
                    {
                         new KeyPointDto
                            {
                            Id = -1,
                            Name = "test1",
                            Description = "testDescription1",
                            Image = "testImage1.jpg",
                            Latitude = 10,
                            Longitude = 101,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                            new KeyPointDto
                            {
                            Id = -2,
                            Name = "test2",
                            Description = "testDescription2",
                            Image = "testImage2.jpg",
                            Latitude = 20,
                            Longitude = 22,
                            Status = KeyPointDto.PublicStatus.PRIVATE,
                            Comment = "Comment1"
                            },
                    },
                    TourDurations = new List<TourDurationDto>
                    {
                        new TourDurationDto { Duration = 10, TimeUnit = 0, TransportType = 0 }
                    },
                    Length = 0,
                    AuthorId = -12,
                    PublishTime = null,
                    ArchiveTime=null
                },
                200,
                TourStatus.Published
            }
        };
    }



    private static TourController CreateController(IServiceScope scope, string authorId)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext(authorId)
        };
    }
}
