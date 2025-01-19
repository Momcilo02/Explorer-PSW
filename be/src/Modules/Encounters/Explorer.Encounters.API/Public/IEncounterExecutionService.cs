using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public
{
    public interface IEncounterExecutionService
    {
        Result<PagedResult<EncounterExecutionDto>> GetPaged(int page, int pageSize);
        Result<EncounterExecutionDto> Update(EncounterExecutionDto encounter);
        Result<EncounterExecutionDto> Get(int id);
        Result<List<EncounterExecutionDto>> GetAllTouristEncounters(int touristId);
        Result ActivateEncounter(EncounterExecutionDto encounterExecution);
        Result<EncounterExecutionDto> GetActivatedTouristEncounter(int touristId);
        EncounterExecutionDto UpdateTouristLocation(EncounterExecutionDto updatedExecution, int numberOfPeople);
        EncounterExecutionDto LeaveEncounter(EncounterExecutionDto updatedExecution);
        Result<bool> TouristCompletedEncounterForTour(int touristId, int encounterId);

    }
}
