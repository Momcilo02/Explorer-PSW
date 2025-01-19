using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author.Shopping;

[Route("api/administration/bundle")]
[Authorize(Policy = "authorPolicy")]
public class BundleController : BaseApiController
{
    private readonly IBundleService _bundleService;

    public BundleController(IBundleService bundleService)
    {
        _bundleService = bundleService;
    }

    [HttpPost]
    public ActionResult<BundleDto> Create([FromBody] BundleDto bundleDto)
    {
        bundleDto.CreatorId = this.User.PersonId();
        var result = _bundleService.Create(bundleDto);
        return CreateResponse(result);
    }
    [HttpPut("{id:int}")]
    public ActionResult<BundleDto> Update([FromBody] BundleDto bundleDto)
    {
        if(bundleDto.CreatorId != this.User.PersonId())
            return Unauthorized("This is not your bundle!");
        var result = _bundleService.Update(bundleDto);
        return CreateResponse(result);
    }
    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        var bundle = _bundleService.Get(id);
        if (bundle.Value == null)
            return NotFound("Does not exist bundle with this id!"); //new ObjectResult("Does not exist bundle with this id!") { StatusCode = 401 };
        
        if (bundle.Value.CreatorId != this.User.PersonId())
            return Unauthorized("This is not your bundle!"); //new ObjectResult("This is not your bundle!") { StatusCode = 401 };//
        
        var result = _bundleService.Delete(id);
        return CreateResponse(result);
    }
    [HttpGet]
    public ActionResult<PagedResult<BundleDto>> GetMyBundles([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _bundleService.GetPagedByCreatorId(this.User.PersonId(), page, pageSize); 
        return CreateResponse(result);
    }
    [HttpPut]
    [Route("publish/{id:int}")]
    public ActionResult<BundleDto> Publish(BundleDto bundleDto)
    {
        if(bundleDto.CreatorId != this.User.PersonId())
            return Unauthorized("This is not your bundle!");
        var result = _bundleService.Publish(bundleDto);
        return CreateResponse(result);
    }
    [HttpPut]
    [Route("archive/{id:int}")]
    public ActionResult<BundleDto> Archive(BundleDto bundleDto)
    {
        if (bundleDto.CreatorId != this.User.PersonId())
            return Unauthorized("This is not your bundle!");
        var result = _bundleService.Archive(bundleDto);
        return CreateResponse(result);
    }

}
