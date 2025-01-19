using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public enum ResourcesType
    {
        Tour = 0,
        Blog = 1
    }
    public class ProfileMessageDto
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }
        public long ResourcesId { get; set; }
        public ResourcesType Type { get; set; }
        public string ResourcesName { get; set; }
    }
}
