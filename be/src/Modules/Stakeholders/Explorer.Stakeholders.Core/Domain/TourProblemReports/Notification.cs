using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.TourProblemReports
{
    //po potrebi za ostale zadatke se menja enum
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
    public class Notification : Entity
    {
        public int ReportId { get; private set; }
        public int RecipientId { get; private set; }
        public int SenderId { get; private set; }
        public bool IsRead { get; private set; }
        public NotificationType NotificationType { get; private set; }

        public Notification() { }

        public Notification(int reportId, int recipientId, int senderId, bool isRead, NotificationType notificationType) 
        { 
            ReportId = reportId;
            RecipientId = recipientId;
            IsRead = isRead;
            NotificationType = notificationType;
            SenderId = senderId;
        }

    }
}
