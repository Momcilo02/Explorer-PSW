using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;
public enum NotificationType
{
    CHAT,
    DEADLINE,
    PROFILE_CHAT,
    ACCEPT_KP,
    REFUSE_KP,
    ACCEPT_OBJ,
    REFUSE_OBJ,
    PAYMENT
}
public class NotificationDto
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public int RecipientId { get; set; }
    public int SenderId { get; set; }
    public bool IsRead { get; set; }
    public NotificationType NotificationType { get; set; }
}
