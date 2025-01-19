using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Explorer.Shopping.Core.Domain.ShoppingCarts
{
    public class OrderItem : ValueObject<OrderItem>
    {
        public long ItemId { get; init; }
        public string Name { get; init; }
        public int Price { get; init; }

        public OrderItem() { }

        [JsonConstructor]
        public OrderItem(long itemId, string name, int price)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            Validate();
        }
        private void Validate()
        {
            if (ItemId == 0) throw new ArgumentException("Invalid ItemId");
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name.");
            if (Price < 0) throw new ArgumentException("Invalid Price.");
        }

        protected override bool EqualsCore(OrderItem other)
        {
            return ItemId == other.ItemId && Name == other.Name && Price == other.Price;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ItemId.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Price.GetHashCode();
                return hash;
            }
        }
    }
}
