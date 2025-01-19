using Explorer.API.Controllers.User;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration
{
    [Collection("Sequential")]
    public class NotificationCommandTests : BaseStakeholdersIntegrationTest
    {
        public NotificationCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Updates_notification_to_mark_as_read()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var updateDto = new NotificationDto
            {
                Id = -1,
                IsRead = true,
                RecipientId = -2,
                NotificationType = NotificationType.CHAT,
                ReportId = -2
            };

            // Act
            var result = ((ObjectResult)controller.Update(updateDto).Result)?.Value as NotificationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.IsRead.ShouldBe(true);
            result.RecipientId.ShouldBe(-2);
            result.NotificationType.ShouldBe(NotificationType.CHAT);
            result.ReportId.ShouldBe(-2);

            // Assert - Database
            var updatedNotification = dbContext.Notifications.FirstOrDefault(n => n.RecipientId == -2);
            updatedNotification.ShouldNotBeNull();
            updatedNotification = dbContext.Notifications.FirstOrDefault(n => n.ReportId == -2);
            updatedNotification.ShouldNotBeNull();
            updatedNotification = dbContext.Notifications.FirstOrDefault(n => n.NotificationType == 0);
            updatedNotification.ShouldNotBeNull();
            updatedNotification = dbContext.Notifications.FirstOrDefault(n => n.IsRead == true);
            updatedNotification.ShouldNotBeNull();
        }

        [Fact]
        public void Update_notification_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updateDto = new NotificationDto
            {
                Id = -1000, // Non-existent ID
                IsRead = true
            };

            // Act
            var result = (ObjectResult)controller.Update(updateDto).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static NotificationController CreateController(IServiceScope scope)
        {
            return new NotificationController(scope.ServiceProvider.GetRequiredService<INotificationService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

