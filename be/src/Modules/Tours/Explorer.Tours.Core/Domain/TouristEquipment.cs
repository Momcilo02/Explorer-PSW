using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TouristEquipment : Entity
    {
        public long TouristId { get; init; }
        public long EquipmentId { get; init; }
        public TouristEquipment(long touristId, long equipmentId)
        {
            TouristId = touristId;
            EquipmentId = equipmentId;
            Validate();
        }
        public void Validate()
        {
            if (EquipmentId == 0)
                throw new ArgumentException("EquipmentId must be greater than 0", nameof(EquipmentId));
            if (TouristId == 0)
                throw new ArgumentException("TouristId must be greater than 0", nameof(TouristId));

        }
    }
}