using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping;

[Authorize(Policy = "userPolicy")]
[Route("api/shopping/bundle")]
public class BundleController : BaseApiController
{
    private readonly IBundleService _bundleService;

    public BundleController(IBundleService bundleService)
    {
        _bundleService = bundleService;
    }

    [HttpGet]
    public ActionResult<PagedResult<BundleDto>> GetPublished([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _bundleService.GetPublished(page, pageSize);
        return CreateResponse(result);
    }
}
