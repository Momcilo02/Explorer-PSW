using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public
{
    public interface ICouponService
    {
        Result<PagedResult<CouponDto>> GetPaged(int page, int pageSize);
        Result<CouponDto> Create(CouponDto coupon);
        Result<CouponDto> Update(CouponDto coupon);
        Result Delete(int id);
        Result<CouponDto> GetByIdentifier(string identifier);
    }
}
