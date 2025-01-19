using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubMessageDto
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public long TouristClubId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }
        public long LikesCount { get; set; }
        public bool LikedByLoggedUser { get; set; }
    }
}
