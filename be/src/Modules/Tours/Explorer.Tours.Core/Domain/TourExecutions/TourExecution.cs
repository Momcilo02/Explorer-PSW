using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.TourExecutions
{
    public enum ExecutionStatus
    {
        COMPLETED,
        ABANDONED,
        ONGOING
    }
    public class TourExecution : Entity
    {
        public int TourId { get; private set; }
        public int TouristId { get; private set; }
        public DateTime TourStartDate { get; private set; }
        public DateTime TourEndDate { get; private set; }
        public DateTime LastActivity { get; private set; }
        public ExecutionStatus Status { get; private set; }
        public float CompletedPercentage { get; private set; }
        public List<CompletedKeyPoints> CompletedKeyPoints { get;  private set; } 
        public float CurrentLongitude { get; private set; }
        public float CurrentLatitude { get; private set; }

        public TourExecution(){  }
        public TourExecution(int tourId, int touristId,DateTime tourStartDate,DateTime tourEndDate, DateTime lastActivity,ExecutionStatus status,float completedPercentage, float currentLongitude, float currentLatitude)
        {
            TourId = tourId;
            TouristId = touristId;
            TourStartDate = tourStartDate;
            TourEndDate = tourEndDate;
            LastActivity = lastActivity;
            Status = status;
            CompletedPercentage = completedPercentage;
            CurrentLongitude = currentLongitude;
            CurrentLatitude = currentLatitude;
            CompletedKeyPoints = new List<CompletedKeyPoints>();
        }

        public void StartNewTour()
        { 
            TourStartDate = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            Status = ExecutionStatus.ONGOING;
            CompletedPercentage = 0;

        }
        public void LeaveTour()
        {
            TourEndDate = DateTime.UtcNow;
            Status = ExecutionStatus.ABANDONED;
        }

        public void AddCompletedKeyPoint(CompletedKeyPoints keyPoint)
        {
            CompletedKeyPoints.Add(keyPoint);
            LastActivity = DateTime.UtcNow;
        }
        public void UpdateCompletedPercentage(int fullCount)
        {
            CompletedPercentage =((float)CompletedKeyPoints.Count/fullCount)*100;
            CompletedPercentage = (float)Math.Round(CompletedPercentage, 2);
        }
         public void UpdateLocation(float latitude,float longitude)
        {
            //CurrentTouristLocation = ...
            CurrentLatitude = latitude;
            CurrentLongitude = longitude;
            LastActivity = DateTime.UtcNow;

        }

        public void FinishTour()
        {
            TourEndDate = DateTime.UtcNow;
            Status = ExecutionStatus.COMPLETED;
        }
    }
}

