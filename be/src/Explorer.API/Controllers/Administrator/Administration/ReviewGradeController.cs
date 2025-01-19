using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]  // Autorizacija za administratorsku rolu
    [Route("api/administration/applicationGrade")]
    public class ReviewGradeController : BaseApiController
    {
        private readonly IApplicationGradeService _applicationGradeService;

        // Injektovanje servisa putem konstruktora
        public ReviewGradeController(IApplicationGradeService applicationGradeService)
        {
            _applicationGradeService = applicationGradeService;
        }

        // Metoda za pregled ocena sa paginacijom
        //[HttpGet]
        //public IActionResult ReviewGrades([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        //{
        //    // Pozivanje servisne metode za preuzimanje ocena sa paginacijom
        //    var result = _applicationGradeService.GetGrades(page, pageSize);
        //    // Kreiranje HTTP odgovora na osnovu rezultata
        //    return CreateResponse(result);
        //}

        //// Opciono: Dodatna metoda za prikaz jedne ocene ili ocene po korisniku
        //[HttpGet("user-exists/{userId}")]
        //public IActionResult UserExists(int userId)
        //{
        //    // Provera da li korisnik sa datim ID-jem već postoji
        //    var result = _applicationGradeService.UserExists(userId);
        //    return CreateResponse(result);
        //}

        //// POST metoda za dodavanje ocene
        //[HttpPost]
        //public IActionResult AddGrade([FromBody] ApplicationGradeDto applicationGrade)
        //{
        //    var result = _applicationGradeService.AddGrade(applicationGrade);
        //    return CreateResponse(result);
        //}

    }
}

