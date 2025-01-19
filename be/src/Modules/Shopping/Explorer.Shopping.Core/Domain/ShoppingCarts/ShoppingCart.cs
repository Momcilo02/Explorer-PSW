using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.ShoppingCarts
{
    public class ShoppingCart : Entity
    {
        public long UserId { get; init; }
        public List<OrderItem> Items { get; private set; } 
        public decimal TotalPrice { get; private set; }
        public List<TourPurchaseToken> TourPurchaseTokens { get; private set; }

        public ShoppingCart(long userId)
        {
            UserId = userId;
            Items = new List<OrderItem>();
            TourPurchaseTokens = new List<TourPurchaseToken>();
        }
        public void AddItem(OrderItem item)
        {
            Items.Add(item);   
            CalculateTotalPrice();
        }
        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        public void RemoveItem(OrderItem item)
        {
            Items.Remove(item);
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Items.Sum(i => i.Price);
        }
        public void UpdateItem(OrderItem orderItem, Item item)
        {
            //item je ovde null ali radi 

            var index = Items.FindIndex(i => i.ItemId == orderItem.ItemId);

            if (index == -1) throw new ArgumentException("Order item not found in cart.");

            var updatedItem = new OrderItem(item.ItemId, item.Name, item.Price);
            Items[index] = updatedItem;
        }
        //public void Checkout()
        //{
        //    foreach (var item in Items)
        //    {
        //        TourPurchaseTokens.Add(new TourPurchaseToken(UserId, item.ItemId));
        //    }
        //}
    }

}
