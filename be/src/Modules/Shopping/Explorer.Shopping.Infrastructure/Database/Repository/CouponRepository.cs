using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ShoppingContext _context;

        public CouponRepository(ShoppingContext context)
        {
            _context = context;
        }

        public List<Coupon> GetAll()
        {
            return _context.Coupons.ToList();
        }

        public Coupon GetByIdentifier(string identifier)
        {
            Coupon coupon = _context.Coupons.FirstOrDefault(cp => cp.Identifier.Equals(identifier));
            return coupon;
        }
    }
}
