using Explorer.API.Controllers.Author.Shopping;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Tests.Integration;

[Collection("Sequential")]
public class BundleQueryTests : BaseShoppingIntegrationTest
{
    public BundleQueryTests(ShoppingTestFactory factory) : base(factory)
    {
    }
    [Fact]
    public void Retrives_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11");

        //Act
        var result = ((ObjectResult)controller.GetMyBundles(0, 0).Result)?.Value as PagedResult<BundleDto>;

        //Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(2);
        result.Results.Count.ShouldBe(2);
    }

    private static BundleController CreateController(IServiceScope scope, string authorId)
    {
        return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
        {
            ControllerContext = BuildContext(authorId)
        };
    }
}
