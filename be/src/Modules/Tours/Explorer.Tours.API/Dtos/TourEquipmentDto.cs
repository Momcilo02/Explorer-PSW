using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class TourEquipmentDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public int EquipmentId { get; set; }
}
