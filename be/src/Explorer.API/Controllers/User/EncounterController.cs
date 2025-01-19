using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/user/encounters")]
    public class EncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        [HttpGet]
        public ActionResult<List<EncounterDto>> GetAllEncounters()
        {
            var result = _encounterService.GetAll();
            return CreateResponse(result);
        }

        [HttpGet("byTour/{tourId:int}")]
        public ActionResult<List<EncounterDto>> GetAllEncountersByTourId(int tourId)
        {
            var result = _encounterService.GetByTourId(tourId);
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
