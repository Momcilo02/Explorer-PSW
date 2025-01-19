using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces
{
    public interface IItemRepository:ICrudRepository<Item>
    {
        List<Item> GetItemsByTourIds(List<long> tourIds);
        public Item GetByItemId(long itemId);
        void DeleteByItemId(long itemId);
        public new Item Update(Item item);
    }
}
