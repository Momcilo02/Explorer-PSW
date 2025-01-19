using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.API.Public;

namespace Explorer.Shopping.Infrastructure.Notification
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationService _notificationService;

        public NotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public void Notify(int recipientId, string notificationType)
        {
            var notification = new NotificationDto
            {
                RecipientId = recipientId,
                NotificationType = Enum.Parse<NotificationType>(notificationType),
                ReportId = 0,
                IsRead = false
            };

            _notificationService.Create(notification);
        }
    }
}
