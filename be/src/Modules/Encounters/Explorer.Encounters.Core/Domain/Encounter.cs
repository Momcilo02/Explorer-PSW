using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public enum EncounterStatus
    {
        ACTIVE,
        DRAFT,
        ARCHIEVED,
        DELETED
    }

    public enum EncounterType
    {
        SOCIAL,
        HIDDENLOCATION,
        MISC
    }
    public enum TouristEncounterStatus
    {
        ACCEPTED,
        AWAITS,
        CANCELLED,
        NOTTOURISTENCOUNTER
    }
    public class Encounter : Entity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }
        public int TotalXp { get; private set; }

        public int CreatorId { get; private set; }

        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public EncounterStatus Status { get; private set; }

        public EncounterType EncounterType { get; private set; }

        public TouristEncounterStatus? TouristRequestStatus { get; private set; }

        public bool? isTourRequired { get; private set; }

        public int? TourId { get; private set; }

        public double ActivateRange { get; private set; }


        public Encounter() {}

        public Encounter(string name, string description, int totalXp, int creatorId, double longitude, double latitude, EncounterStatus status, EncounterType type, TouristEncounterStatus touristEncounterStatus, bool isRequired, int tourId, double activateRange)
        {
            Name = name;
            Description = description;
            TotalXp = totalXp;
            CreatorId = creatorId;
            Longitude = longitude;
            Latitude = latitude;
            Status = status;
            EncounterType = type;
            TouristRequestStatus = touristEncounterStatus;
            isTourRequired = isRequired;
            TourId = tourId;
            ActivateRange = activateRange;
        }

      


    }
}
