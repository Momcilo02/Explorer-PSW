using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum ResourcesType
    {
        Tour = 0,
        Blog = 1
    }
    public class ProfileMessage : Entity
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }
        public long ResourcesId { get; set; }
        public ResourcesType Type { get; set; }
        public string ResourcesName { get; set; }
        
        public ProfileMessage(long senderId, long receiverId, DateTime sentDate, string content, long resourcesId, ResourcesType type, string resourcesName)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            SentDate = sentDate;
            Content = content;
            ResourcesId = resourcesId;
            Type = type;
            ResourcesName = resourcesName;
        }

        private void Validate()
        {
            if (SenderId == 0) throw new ArgumentException("Invalid SenderId");
            if (ReceiverId == 0) throw new ArgumentException("Invalid ReceiverId");
            if (string.IsNullOrWhiteSpace(Content)) throw new ArgumentException("Invalid content");
            if (Content.Length > 280) throw new ArgumentException("Exceeded the expected message content length");
            if (string.IsNullOrWhiteSpace(ResourcesName)) throw new ArgumentException("Invalid ResourcesName");
        }
    }
}
