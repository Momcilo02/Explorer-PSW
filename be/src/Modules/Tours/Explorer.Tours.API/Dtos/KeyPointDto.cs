using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class KeyPointDto
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public PublicStatus Status { get; set; }
    public string Comment { get; set; }

    public enum PublicStatus
    {
        PRIVATE,
        REQUESTED,
        PUBLIC
    }
}
