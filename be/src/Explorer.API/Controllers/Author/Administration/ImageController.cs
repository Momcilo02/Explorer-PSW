using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author.Administration;

[Route("api/tour/image")]
public class ImageController : BaseApiController
{
    private readonly IWebHostEnvironment _env;

    public ImageController(IWebHostEnvironment env)
    {
        _env = env;
    }
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var solutionRoot = Directory.GetParent(_env.ContentRootPath).FullName;
        string targetDirectory = Path.Combine(solutionRoot, "Modules", "Tours", "Explorer.Tours.Infrastructure", "Images");
        string path = Path.Combine(targetDirectory, file.FileName);

        if(System.IO.File.Exists(path))
            return Ok(new { filePath = path });

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath = path });
    }

    [HttpGet]
    public IActionResult GetImage([FromQuery] string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "image/jpeg");
    }

}
