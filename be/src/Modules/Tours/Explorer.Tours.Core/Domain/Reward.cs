using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Reward : ValueObject<Reward>
    {
        public RewardType Type { get; private set; }
        public int Amount { get; private set; }

        public Reward(RewardType type, int amount)
        {
            if (amount <= 0) throw new ArgumentException("Reward amount must be greater than zero.");
            Type = type;
            Amount = amount;
        }

        protected override bool EqualsCore(Reward other)
        {
            return Type == other.Type && Amount == other.Amount;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Amount} {Type}";
        }
    }

    public enum RewardType
    {
        XP,
        Coupon,
        AC
    }
}
