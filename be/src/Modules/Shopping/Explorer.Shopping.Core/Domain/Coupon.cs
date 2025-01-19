using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain
{
    /* 
        Status: 0-Active
                1-Used
                2-Expired
     */
    public class Coupon : Entity
    {
        public string Identifier { get; private set; }
        public long Percentage { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public long AuthorId { get; private set; }
        public List<int> ToursEligible { get; private set; }
        public int CouponStatus { get; private set; }


        public Coupon(long id, string identifier, long percentage, DateTime expirationDate, long authorId, List<int> toursEligible, int couponStatus) 
        {
            Id = id;
            Identifier = identifier;
            Percentage = percentage;
            ExpirationDate = expirationDate;
            AuthorId = authorId;
            ToursEligible = toursEligible;
            CouponStatus = couponStatus;
            Validate();
        }

        public void Validate()
        {
            if (Identifier == "") throw new ArgumentException("Identifier invalid!");
            if (Percentage == 0) throw new ArgumentException("Percentage cannot be 0!");
            if (ExpirationDate == DateTime.MinValue) throw new ArgumentException("Invalid date!");
            if (AuthorId == 0) throw new ArgumentException("Invalid author!");
            if (ToursEligible == null) throw new ArgumentException("Invalid tours list!");
            if (ToursEligible.Count == 0) throw new ArgumentException("List cannot be empty!");
            if (CouponStatus > 2) throw new ArgumentException("Invalid value for status!");
        }

    }
}
