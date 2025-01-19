using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository
{
    public class TourPurchaseTokenDatabaseRepository : CrudDatabaseRepository<TourPurchaseToken, ShoppingContext>, ITourPurchaseTokenRepository
    {
        public TourPurchaseTokenDatabaseRepository(ShoppingContext dbContext) : base(dbContext) { }

        public List<TourPurchaseToken> GetByUser(long userId)
        {
            return DbContext.PurchaseTokens.Where(t => t.UserId == userId).ToList();
        }
       
        public bool ExistsByTourAndUser(long tourId, long userId)
        {
            return DbContext.PurchaseTokens.Any(t => t.TourId == tourId && t.UserId == userId);
        }

        public bool HasPurchasedTour(long tourId, long userId)
        {
            return DbContext.PurchaseTokens.Any(t => t.UserId == userId && t.TourId == tourId);
        }

        public List<long> GetSoldToursIds()
        {
            return DbContext.PurchaseTokens.Select(t => t.TourId).ToList();
        }
        public int GetPurchasesNumberForTour(long tourId)
        {
            return DbContext.PurchaseTokens.Count(t => t.TourId == tourId);
        }

        public List<long> GetItemsByTouristId(long touristId)
        {
            var tourIds = DbContext.PurchaseTokens
                             .Where(t => t.UserId == touristId)
                             .Select(t => t.TourId)
                             .ToList();

            return tourIds; ;
        }
    }
}
