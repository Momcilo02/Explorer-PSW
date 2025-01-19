using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.UseCases
{
    public class CouponService : CrudService<CouponDto, Coupon>, ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        public CouponService(ICrudRepository<Coupon> repository, ICouponRepository couponRepository, IMapper mapper) : base(repository, mapper) 
        {
            _couponRepository = couponRepository;
        }

        public Result<CouponDto> GetByIdentifier(string identifier)
        {
            try
            {
                var result = _couponRepository.GetByIdentifier(identifier);

                if (result == null)
                {
                    throw new KeyNotFoundException($"Coupon with identifier {identifier} not found.");
                }

                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}
