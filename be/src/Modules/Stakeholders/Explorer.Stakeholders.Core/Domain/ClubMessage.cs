using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubMessage : Entity
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public long TouristClubId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }
        public List<int> PersonsLiked { get; set; } = new List<int>();

        public ClubMessage(long senderId, long touristClubId, DateTime sentDate, string content)
        {
            SenderId = senderId;
            TouristClubId = touristClubId;
            SentDate = sentDate;
            Content = content;
            PersonsLiked = new List<int>();
        }

        private void Validate()
        {
            if (SenderId == 0) throw new ArgumentException("Invalid SenderId");
            if (string.IsNullOrWhiteSpace(Content)) throw new ArgumentException("Invalid content");
            if (Content.Length > 280) throw new ArgumentException("Exceeded the expected message content length");
        }
    }
}
