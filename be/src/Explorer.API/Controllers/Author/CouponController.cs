using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy="authorPolicy")]
    [Route("api/coupons")]
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public ActionResult<PagedResult<CouponDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _couponService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<CouponDto> Create([FromBody] CouponDto dto)
        {
            var result = _couponService.Create(dto);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CouponDto> Update([FromBody] CouponDto dto)
        {
            var result = _couponService.Update(dto);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _couponService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("identifier")]
        public ActionResult<CouponDto> GetByIdentifier([FromQuery] string identifier)
        {
            var result = _couponService.GetByIdentifier(identifier);
            return CreateResponse(result);
        }
    }
}
