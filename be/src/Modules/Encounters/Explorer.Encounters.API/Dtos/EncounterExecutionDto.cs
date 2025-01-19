using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public enum EncounterExecutionStatus
    {
        ACTIVATED,
        COMPLETED,
        ABANDONED
    }
    public class EncounterExecutionDto
    {
        public int Id { get; set; }
        public int EncounterId { get; set; }
        public int TouristId { get; set; }
        public double TouristLongitude { get; set; }
        public double TouristLatitude { get; set; }
        public EncounterExecutionStatus Status { get; set; }
        public int? NumberOfActiveTourists { get; set; }
    }
}
