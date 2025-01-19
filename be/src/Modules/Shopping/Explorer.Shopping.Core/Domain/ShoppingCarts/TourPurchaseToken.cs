using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.ShoppingCarts
{
    public class TourPurchaseToken:Entity
    {
        public long UserId {  get; set; }
        public long TourId { get; set;}
        public TourPurchaseToken() { }
        public TourPurchaseToken(long userId, long tourId)
        {
            UserId = userId;
            TourId = tourId;
        }
    }
}
