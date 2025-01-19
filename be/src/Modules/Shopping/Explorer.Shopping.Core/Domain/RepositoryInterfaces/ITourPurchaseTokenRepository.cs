using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces
{
    public interface ITourPurchaseTokenRepository : ICrudRepository<TourPurchaseToken>
    {
        public List<TourPurchaseToken> GetByUser(long userId);
        public bool ExistsByTourAndUser(long tourId, long userId);
        public bool HasPurchasedTour(long tourId, long userId);
        public List<long> GetSoldToursIds();
        public int GetPurchasesNumberForTour(long tourId);

        public List<long> GetItemsByTouristId(long touristId);
    }
}
