using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain
{
    public enum EncounterExecutionStatus
    {
        ACTIVATED,
        COMPLETED,
        ABANDONED
    }
    public class EncounterExecution : Entity
    {
        public int EncounterId { get; private set; }
        public int TouristId { get; private set; }
        public double TouristLongitude { get; private set; }
        public double TouristLatitude { get; private set; }
        public EncounterExecutionStatus Status { get; private set; }
        public int NumberOfActiveTourists { get; private set; }

        public EncounterExecution()
        {
        }
        public EncounterExecution(int encounterId, int touristId, double touristLatitude, double touristLongitude, EncounterExecutionStatus status, int numberOfActiveTourists)
        {
            EncounterId = encounterId;
            TouristId = touristId;
            TouristLongitude = touristLongitude;
            TouristLatitude = touristLatitude;
            Status = status;
            NumberOfActiveTourists = numberOfActiveTourists;
        }

        public void UpdateLongitudeLatitude(double longitude, double latitude)
        {
            TouristLongitude = longitude;
            TouristLatitude = latitude;
        }

        public void CompleteEncounter(int activeTourists)
        {
            NumberOfActiveTourists = activeTourists;
            Status = EncounterExecutionStatus.COMPLETED;
        }

        public void AbandonEncounter()
        {
            Status = EncounterExecutionStatus.ABANDONED;
        }

        public void SetNumberOfActiveTourists(int activeTourists)
        {
            NumberOfActiveTourists = activeTourists;
        }
    }
}
