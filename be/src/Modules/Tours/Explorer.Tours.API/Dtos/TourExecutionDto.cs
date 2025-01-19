using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public enum ExecutionStatus
    {
        COMPLETED,
        ABANDONED,
        ONGOING
    }
    public class TourExecutionDto
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int TouristId { get; set; }
        public DateTime TourStartDate { get;  set; }
        public DateTime? TourEndDate { get;  set; }
        public DateTime? LastActivity { get;  set; }
        public ExecutionStatus Status { get;  set; }
        public float CompletedPercentage { get;  set; }
        public List<CompletedKeyPointsDto>? CompletedKeyPoints { get; set; }
        public double CurrentLongitude { get;  set; }
        public double CurrentLatitude { get;  set; }


    }
}
