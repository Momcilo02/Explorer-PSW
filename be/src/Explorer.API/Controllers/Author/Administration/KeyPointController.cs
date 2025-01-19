using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author.Administration;

[Authorize(Policy = "authorPolicy")]
[Route("api/tours/keypoint")]
public class KeyPointController : BaseApiController
{
    private readonly IKeyPointService _keyPointService;

    public KeyPointController(IKeyPointService keyPointService)
    {
        _keyPointService = keyPointService;
    }

    [HttpPost]
    public ActionResult<KeyPointDto> Create([FromBody] KeyPointDto keyPointDto)
        {
        var result = _keyPointService.Create(keyPointDto);
        return CreateResponse(result);
    }

    [HttpGet]
    public ActionResult<List<KeyPointDto>> GetAll()
    {
        var result = _keyPointService.GetAll();
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<KeyPointDto> Update([FromBody] KeyPointDto keyPointDto)
    {
        var result = _keyPointService.Update(keyPointDto);
        return CreateResponse(result);
    }
}
