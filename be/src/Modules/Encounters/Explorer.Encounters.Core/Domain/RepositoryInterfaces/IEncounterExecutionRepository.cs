using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IEncounterExecutionRepository
    {
        PagedResult<EncounterExecution> GetPaged(int page, int pageSize);
        List<EncounterExecution> GetAll();
        EncounterExecution Get(int id);
        Result Create(EncounterExecution execution);
        EncounterExecution Update(EncounterExecution execution);
        List<EncounterExecution> GetAllByUserId(int touristId);
        public EncounterExecution GetUserActiveEncounter(int touristId);
        bool HasAlreadyActivatedEncounter(int touristId);

        bool TouristCompletedEncounterForTour(int touristId, int encounterId);
    }
}
