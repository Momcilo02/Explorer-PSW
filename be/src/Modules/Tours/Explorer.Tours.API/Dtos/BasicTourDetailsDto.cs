using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class BasicTourDetailsDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Cost { get; set; }
    public double Length { get; set; }
    public KeyPointDto FirstKeyPoint { get; set; } = new KeyPointDto();
    public double AverageRate { get; set; }

}
