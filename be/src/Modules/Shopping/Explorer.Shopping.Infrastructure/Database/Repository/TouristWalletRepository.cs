using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository
{
    public class TouristWalletRepository : CrudDatabaseRepository<TouristWallet, ShoppingContext>, ITouristWalletRepository
    {
        private readonly ShoppingContext _dbContext;
        public TouristWalletRepository(ShoppingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public TouristWallet GetByUser(long userId)
        {
            var wallet = _dbContext.TouristWallets.FirstOrDefault(w => w.UserId == userId);
            //if (wallet == null) throw new KeyNotFoundException("Not found wallet with user ID: " + userId);
            return wallet;
        }

        public TouristWallet PaymentAdventureCoins(long userId, int coins)
        {
            var wallet = _dbContext.TouristWallets.FirstOrDefault(w => w.UserId == userId);
            if (wallet == null) throw new KeyNotFoundException("Not found wallet with user ID: " + userId);

            try
            {
                wallet.PaymentAdventureCoins(coins);
                _dbContext.TouristWallets.Update(wallet);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new KeyNotFoundException(e.Message);
            }
            return wallet;
        }
    }
}
