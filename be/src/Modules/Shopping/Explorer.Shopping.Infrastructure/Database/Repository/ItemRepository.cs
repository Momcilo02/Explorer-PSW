using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain;
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
    public class ItemRepository : CrudDatabaseRepository<Item, ShoppingContext>, IItemRepository
    {
        private ShoppingContext _shoppingContext;
        public ItemRepository(ShoppingContext dbContext) : base(dbContext)
        {
            _shoppingContext = dbContext;
        }
        public List<Item> GetItemsByTourIds(List<long> tourIds)
        {
            return DbContext.Items
                .Where(item => tourIds.Contains(item.ItemId))
                .ToList();
        }
        public Item Update(Item item)
        {
            try
            {

                _shoppingContext.Update(item);
                _shoppingContext.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public Item GetByItemId(long itemId)
        {
            var item = DbContext.Items.FirstOrDefault(i => i.ItemId== itemId);
            if (item == null) return null;
            return item;
        }
        
        public void DeleteByItemId(long itemId)
        {
            var item = DbContext.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null) throw new KeyNotFoundException("Not found: " + itemId);
            DbContext.Items.Remove(item);
            DbContext.SaveChanges();
        }
    }
}
