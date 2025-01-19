using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain;

public class Product: Entity
{
    public long TourId { get; init; }
    public double Price { get; init; }

    public Product(long tourId, double price)
    {
        TourId = tourId;
        Price = price;
    }
}
