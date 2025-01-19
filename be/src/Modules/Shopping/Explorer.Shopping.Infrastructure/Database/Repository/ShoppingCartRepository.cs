using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using Explorer.Shopping.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository
{
    public class ShoppingCartRepository : CrudDatabaseRepository<ShoppingCart, ShoppingContext>, IShoppingCartRepository
    {
        private readonly ShoppingContext _context;
        public ShoppingCartRepository(ShoppingContext dbContext) : base(dbContext) {

            _context = dbContext;
        }

        public ShoppingCart GetByUser(long userId)
        {
            var shoppingCart = DbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);
            if (shoppingCart == null) throw new KeyNotFoundException("Not found: " + userId);
            return shoppingCart;
        }
       
    }
}
