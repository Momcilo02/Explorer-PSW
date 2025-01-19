using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos;

public class ProductDto
{
    public long Id { get; set; }
    public long TourId { get; set; }
    public double Price { get; set; }
}
