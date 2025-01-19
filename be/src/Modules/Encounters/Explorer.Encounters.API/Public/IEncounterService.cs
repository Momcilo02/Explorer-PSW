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
    public interface IEncounterService
    {

        Result<EncounterDto> CreateEncounter(EncounterDto encounter);

        Result<EncounterDto> Update(EncounterDto encounter);

        Result<EncounterDto> GetById(long id);
        Result<List<EncounterDto>> GetByTourId(int tourId);
        
        Result<List<EncounterDto>> GetAll();

        Result<List<EncounterDto>> GetTouristRequestEncounters();
    }
}
