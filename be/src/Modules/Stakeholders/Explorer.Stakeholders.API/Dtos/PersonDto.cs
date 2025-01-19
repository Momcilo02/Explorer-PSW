using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public enum TouristStatus
    {
        GOLDEN,
        SILVER,
        BRONZE,
        BASIC
    }
    public enum TouristRank
    {
        EXPLORER,
        SURVIVOR,
        TRAVELLER,
        CAPTAIN,
        ULTIMATE
    }
    public class PersonDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Biography { get; set; }
        public string Motto { get; set; }
        public string Email { get; set; }
        public int? TouristLevel { get; set; }
        public int? TouristXp { get; set; }

        public DateTime? LastWheelSpinTime { get; set; }
        public List<int> Followers { get; set; } = new List<int>();
        public List<int> Following { get; set; } = new List<int>();
        public List<int> ClubMember { get; set; } = new List<int>();
        public TouristStatus TouristStatus { get; set; }
        public TouristRank? TouristRank { get; set; }
    }
}