using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class SocialEncounter : Encounter
    {
        public int PeopleNumb { get; private set; }

        public SocialEncounter() : base() { }
        public SocialEncounter(string name, string description, int totalXp, int creatorId, double longitude, double latitude, EncounterStatus status, EncounterType type, TouristEncounterStatus touristEncounterStatus, bool isRequired, int tourId, double activateRange, int peopleNumb)
        : base(name, description, totalXp, creatorId, longitude, latitude, status, type, touristEncounterStatus, isRequired, tourId, activateRange)
        {
            PeopleNumb = peopleNumb;
        }

    }
}
