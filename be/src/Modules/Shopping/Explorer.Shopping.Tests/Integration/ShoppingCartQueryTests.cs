using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class ShoppingCartQueryTests : BaseShoppingIntegrationTest
    {
        public ShoppingCartQueryTests(ShoppingTestFactory factory) : base(factory) { }
        
        [Fact]
        public void Retrieves_by_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetByUser(-21).Result)?.Value as ShoppingCartDto;

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.UserId.ShouldBe(-21);

        }

        [Fact]
        public void Retrieves_by_id_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.GetByUser(-1).Result;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static ShoppingCartController CreateController(IServiceScope scope)
        {
            return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
            {
                ControllerContext = BuildContext("-21")
            };
        }
        
    }
}