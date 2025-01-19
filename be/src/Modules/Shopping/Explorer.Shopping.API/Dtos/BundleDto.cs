using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos;

public enum BundleStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2,
}
public class BundleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public BundleStatus Status { get; set; }
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    public long CreatorId { get; set; }
}
