using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain;

public class PaymentRecord: Entity
{
    public long TouristId { get; init; }
    public long ItemId { get; init; }
    public double Price { get; init; }
    public DateTime ShoppingTime { get; init; }

    public PaymentRecord(long touristId, long itemId, double price, DateTime shoppingTime)
    {
        TouristId = touristId;
        ItemId = itemId;
        Price = price;
        ShoppingTime = shoppingTime;
    }
}
