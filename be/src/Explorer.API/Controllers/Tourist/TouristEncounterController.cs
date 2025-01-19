using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounters")]
    public class TouristEncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        public TouristEncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        [HttpGet]
        public ActionResult<List<EncounterDto>> GetAllEncounters()
        {
            var result = _encounterService.GetAll();
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<EncounterDto>> GetById(int id)
        {
            var result = _encounterService.GetById(id);
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


    }
}
