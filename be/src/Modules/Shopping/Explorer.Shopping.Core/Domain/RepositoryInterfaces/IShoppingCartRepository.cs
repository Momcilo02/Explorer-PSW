using Explorer.Shopping.Core.Domain.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces
{
    public interface IShoppingCartRepository
    {
        public ShoppingCart GetByUser(long userId);
        ShoppingCart Get(long id);
        ShoppingCart Create(ShoppingCart shoppingCart);
        ShoppingCart Update(ShoppingCart shoppingCart);
        void Delete(long id);
    }
}
