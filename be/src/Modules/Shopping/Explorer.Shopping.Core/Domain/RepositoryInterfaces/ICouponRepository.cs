using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces
{
    public interface ICouponRepository 
    {
        List<Coupon> GetAll();
        Coupon GetByIdentifier(string identifier);
    }
}
