using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
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
  
    public class EncounterDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalXp { get; set; }
        public int CreatorId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public EncounterStatus Status { get; set; }
        public EncounterType EncounterType { get; set; }
        public TouristEncounterStatus? TouristRequestStatus { get; set; }
        public bool? isTourRequired { get; set; }
        public int? TourId { get; set; }
        public double ActivateRange { get; set; }
        public string? Image { get; set; }
        public double? ImageLongitude { get; set; }
        public double? ImageLatitude { get; set; }
        public string? Instructions { get; set; }
        public int? PeopleNumb { get; set; }

    }
}
