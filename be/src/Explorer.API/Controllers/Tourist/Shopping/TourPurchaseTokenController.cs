using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping;

[Authorize(Policy = "touristPolicy")]
[Route("api/shopping/tour-purchase-token")]
public class TourPurchaseTokenController : BaseApiController
{
    private readonly ITourPurchaseTokenService _tourPurchaseTokenService;

    public TourPurchaseTokenController(ITourPurchaseTokenService tourPurchaseTokenService)
    {
        _tourPurchaseTokenService = tourPurchaseTokenService;
    }

    [HttpGet("{tourId:int}/{touristId:int}")]
    public ActionResult<bool> ExistsByTourAndUser(int tourId, int touristId)
    {
        var exists = _tourPurchaseTokenService.ExistsByTourAndUser(tourId, touristId);
        if (exists)
        {
            return Ok(exists);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpGet("mytours/{touristId:int}")]
    public ActionResult<List<long>> GetTouristPurchases(int touristId)
    {
        var result = _tourPurchaseTokenService.GetTouristPurchases(touristId);

        return result;

    }
}
