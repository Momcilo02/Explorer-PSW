using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TouristLocation : Entity
    {
        public int TouristId { get; private set; }
        public float Longitude { get; private set; }
        public float Latitude { get; private set; }

        public TouristLocation(int touristId, float longitude, float latitude)
        {
            TouristId = touristId;
            Longitude = longitude;
            Latitude = latitude;
            Validate();
        }

        public void Validate()
        {
            if (TouristId == 0) throw new ArgumentException("Invalid TouristId!");
            if (Longitude == 0) throw new ArgumentException("Invalid Longitude!");
            if (Latitude == 0) throw new ArgumentException("Invalid Longitude!");
        }
    }
}
