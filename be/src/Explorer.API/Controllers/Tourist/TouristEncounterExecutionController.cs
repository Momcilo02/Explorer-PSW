using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EncounterExecutionStatus = Explorer.Encounters.API.Dtos.EncounterExecutionStatus;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounterExecution")]
    public class TouristEncounterExecutionController : BaseApiController
    {
        private readonly IEncounterExecutionService _encounterExecutionService;
        public TouristEncounterExecutionController(IEncounterExecutionService encounterExecutionService)
        {
            _encounterExecutionService = encounterExecutionService;
        }

        [HttpGet("/byTourist/{touristId:int}")]
        public ActionResult<List<EncounterExecutionDto>> GetAllTouristsEncounters(int touristId)
        {
            var result = _encounterExecutionService.GetAllTouristEncounters(touristId);
            return CreateResponse(result);
        }

        [HttpGet("/activatedByTourist/{touristId:int}")]
        public ActionResult<EncounterExecutionDto> GetAllActivatedTouristsEncounter(int touristId)
        {
            var result = _encounterExecutionService.GetActivatedTouristEncounter(touristId);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<EncounterExecutionDto>> GetById(int id)
        {
            var result = _encounterExecutionService.Get(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<EncounterExecutionDto> ActivateEncounter([FromBody] EncounterExecutionDto encounterExecution)
        {
            var result = _encounterExecutionService.ActivateEncounter(encounterExecution);
            return CreateResponse(result);
        }

        [HttpPut("{numberOfPeople:int}")]
        public ActionResult<EncounterExecutionDto> UpdateTouristLocation([FromBody] EncounterExecutionDto encounterExecution, int numberOfPeople)
        {
            var result = _encounterExecutionService.UpdateTouristLocation(encounterExecution, numberOfPeople);
            return Ok(result);
        }

        [HttpPut("leave")]
        public ActionResult<EncounterExecutionDto> LeaveEncounter([FromBody] EncounterExecutionDto encounterExecution)
        {
            var result = _encounterExecutionService.LeaveEncounter(encounterExecution);
            return Ok(result);
        }


        /** IVA **/
        [HttpPut("completedEncounter")]
        public ActionResult<EncounterExecutionDto> CompletedEncounter([FromBody] EncounterExecutionDto encounterExecution)
        {
            encounterExecution.Status = EncounterExecutionStatus.COMPLETED;
            var result = _encounterExecutionService.Update(encounterExecution);
            return Ok(result);
        }



        [HttpGet("tourist/{touristId}/encounter/{encounterId}")]
        public IActionResult HasTouristCompletedEncounter(int touristId, int encounterId)
        {
            var result = _encounterExecutionService.TouristCompletedEncounterForTour(touristId, encounterId);
            return Ok(result.Value);
        }

        /*********/
    }
}
