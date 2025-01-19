using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Tests.Integration
{
    [Collection("Sequential")]
    public class CouponCommandTests : BaseShoppingIntegrationTest
    {
        public CouponCommandTests(ShoppingTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

            var coupon = new CouponDto()
            {
                Identifier = "a9o2ufso",
                Percentage = 30,
                ExpirationDate = DateTime.UtcNow.AddDays(5),
                AuthorId = 1,
                ToursEligible = new List<int> { 4, 6, 8},
                CouponStatus = 1
            };

            // Act
            var result = ((ObjectResult)controller.Create(coupon).Result)?.Value as CouponDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.CouponStatus.ShouldBe(1);
            result.Identifier.ShouldBe("a9o2ufso");
            result.AuthorId.ShouldBe(1);
            result.ToursEligible.ShouldBe(new List<int> { 4, 6, 8 });

            // Assert - Database
            var storedEntity = dbContext.Coupons.FirstOrDefault(c => c.Identifier == coupon.Identifier);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Creates_invalid_data()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var coupon = new CouponDto
            {
                Identifier = "aos024l1"
            };

            //Act
            var result = ((ObjectResult)controller.Create(coupon).Result);

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();
            var updatedEntity = new CouponDto
            {
                Id = -3,
                Identifier = "osiape02",
                AuthorId = 2,
                Percentage = 10,
                CouponStatus = 1,
                ToursEligible = new List<int> { 1 },
                ExpirationDate = DateTime.UtcNow.AddDays(5)
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as CouponDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-3);
            result.Identifier.ShouldBe(updatedEntity.Identifier);
            result.AuthorId.ShouldBe(updatedEntity.AuthorId);
            result.Percentage.ShouldBe(updatedEntity.Percentage);
            result.CouponStatus.ShouldBe(updatedEntity.CouponStatus);
            result.ExpirationDate.ShouldBe(updatedEntity.ExpirationDate);
            result.ToursEligible.ShouldBe(updatedEntity.ToursEligible);

            // Assert - Database
            var storedEntity = dbContext.Coupons.FirstOrDefault(i => i.Identifier.Equals("osiape02"));
            storedEntity.ShouldNotBeNull();
            storedEntity.Identifier.ShouldBe(updatedEntity.Identifier);
            storedEntity.AuthorId.ShouldBe(updatedEntity.AuthorId);
            storedEntity.Percentage.ShouldBe(updatedEntity.Percentage);
            storedEntity.CouponStatus.ShouldBe(updatedEntity.CouponStatus);
            storedEntity.ExpirationDate.ShouldBe(updatedEntity.ExpirationDate);
            storedEntity.ToursEligible.ShouldBe(updatedEntity.ToursEligible);
            var oldEntity = dbContext.Coupons.FirstOrDefault(i => i.Identifier.Equals("WINTER15"));
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new CouponDto
            {
                Id = -1000,
                Identifier = "osiape02",
                AuthorId = 2,
                Percentage = 10,
                CouponStatus = 1,
                ToursEligible = new List<int> { 1 },
                ExpirationDate = new DateTime(2024, 3, 10)
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingContext>();

            // Act
            var result = (OkResult)controller.Delete(-1);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Coupons.FirstOrDefault(i => i.Id == -1);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void GetByIdentifier()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetByIdentifier("SUMMER50").Result)?.Value as CouponDto;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
        }

        [Fact]
        public void GetByIdentifierFails()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = (ObjectResult)controller.GetByIdentifier("provera1").Result;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }


        private static CouponController CreateController(IServiceScope scope)
        {
            return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>())
            {
                ControllerContext = BuildContext("-21")
            };
        }
    }
}
