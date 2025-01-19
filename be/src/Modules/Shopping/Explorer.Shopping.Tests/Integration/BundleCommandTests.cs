using Explorer.API.Controllers.Author.Shopping;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Infrastructure.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Tests.Integration;

[Collection("Sequential")]
public class BundleCommandTests : BaseShoppingIntegrationTest
{
    public BundleCommandTests(ShoppingTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var contoller = CreateController(scope, "-11");
        var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

        var bundle = new BundleDto
        {
            Name = "test",
            Price = 10.00,
            Products = new List<ProductDto>()
        };

        //Act
        var result = ((ObjectResult) contoller.Create(bundle).Result)?.Value as BundleDto;

        //Assert - response
        result.ShouldNotBeNull();
        result.Name.ShouldBe("test");

        //Assert - database
        var entity = dbContext.Bundles.FirstOrDefault(p => p.Name == result.Name);
        entity.ShouldNotBeNull();
        entity.Id.ShouldBe(result.Id);
    }

    [Fact]  
    public void Deletes()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var contoller = CreateController(scope, "-11");
        var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

        //Act
        var result = (OkResult)contoller.Delete(-1);

        //Assert - response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        //Assert - database
        var entity = dbContext.Bundles.FirstOrDefault(b =>b.Id == -1);
        entity.ShouldBeNull();

    }
    [Fact]
    public void Delete_fails_is_not_my_bundle()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var contoller = CreateController(scope, "-12");
        var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

        //Act
        var result = (ObjectResult)contoller.Delete(-2);

        //Assert - response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(401);

        //Assert - database
        var entity = dbContext.Bundles.FirstOrDefault(b => b.Id == -2);
        entity.ShouldNotBeNull();

    }

    [Fact]  
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11");

        // Act
        var result = (ObjectResult)controller.Delete(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Updates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11");
        var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

        var products = new List<ProductDto>();
        products.Add(new ProductDto { TourId = -1, Price = 1 });
        var bundle = new BundleDto
        {
            Id = -1,
            Name = "new name",
            Price = 11.00,
            Products = products,
            CreatorId = -11,
        };

        //Act
        var result = ((ObjectResult)controller.Update(bundle).Result)?.Value as BundleDto;

        //Assert - response
        result.ShouldNotBeNull();
        result.Name.ShouldBe(bundle.Name);

        //Assert - database
        var entity = dbContext.Bundles.FirstOrDefault(p => p.Id == result.Id);
        entity.ShouldNotBeNull();
        entity.Name.ShouldBe(result.Name);
        entity.Price.ShouldBe(result.Price);
        entity.Products.Count.ShouldBe(1);

    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11");

        var bundle = new BundleDto
        {
            Id = -111,
            Name = "new name",
            Price = 11.00,
            Products = new List<ProductDto>(),
            CreatorId = -11,
        };

        //Act
        var result = (ObjectResult)controller.Update(bundle).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    private static BundleController CreateController(IServiceScope scope, string authorId)
    {
        return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
        {
            ControllerContext = BuildContext(authorId)
        };
    }
}
