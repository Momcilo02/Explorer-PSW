using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Core.Domain
{
    public class TouristClub : Entity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public string? Picture { get; private set; }

        public long OwnerId { get; private set; }

        public List<int> Members { get; private set; } = new List<int>();
        public double AverageRate { get; private set; }
        public List<int> Rates { get; private set; } = new List<int>();
        public List<int> RatedMembers { get; private set; } = new List<int>();

        public TouristClub(string name, string description, string? picture, long ownerId,double averageRate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.");
            Description = description;
            Picture = picture;
            OwnerId = ownerId;
            AverageRate = averageRate;
            Members = new List<int>();
            Rates = new List<int>();
            RatedMembers = new List<int>();
        }
    }
}
