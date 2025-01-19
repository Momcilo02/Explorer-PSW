using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public enum TransportType { Walk, Bike, Car };
    public enum TimeUnit { Second, Minute, Hour };
    public class TourDurationDto
    {
        public double Duration { get; set; }
        public TransportType TransportType { get;set; }
        public TimeUnit TimeUnit { get; set; }


    }
}
