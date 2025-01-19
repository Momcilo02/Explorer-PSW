using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/encounters")]
    public class AdminEncounterController : BaseApiController
    {

        private readonly IEncounterService _encounterService;

        public AdminEncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        [HttpGet]
        public ActionResult<List<EncounterDto>> GetAllEncounters()
        {
            var result = _encounterService.GetAll();
            return CreateResponse(result);
        }

        [HttpGet("requestedEncounters")]
        public ActionResult<List<EncounterDto>> GetAllTouristEncounters()
        {
            var result = _encounterService.GetTouristRequestEncounters();
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<EncounterDto> CreateEncounter([FromBody] EncounterDto encounter)
        {
            var result = _encounterService.CreateEncounter(encounter);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<EncounterDto> UpdateEncounter([FromBody] EncounterDto encounter, int id)
        {
            encounter.Id = id; // Ensure the ID is set on the DTO
            var result = _encounterService.Update(encounter);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<EncounterDto> GetEncounterById(int id)
        {
            var result = _encounterService.GetById(id);
            return CreateResponse(result);
        }

      


    }
}
