using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterService : CrudService<EncounterDto, Encounter>, IEncounterService
    {
        private readonly IMapper _mapper;
        private readonly IEncounterRepository _encounterRepository;
        public EncounterService(IEncounterRepository repository, IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
            _encounterRepository = repository;
        }

        public Result<EncounterDto> CreateEncounter(EncounterDto encounterDto)
        {
           
                Encounter encounter;

                if (encounterDto.EncounterType == API.Dtos.EncounterType.SOCIAL)
                {
                    encounter = _mapper.Map<SocialEncounter>(encounterDto);
                }
                else if (encounterDto.EncounterType == API.Dtos.EncounterType.HIDDENLOCATION)
                {
                    encounter = _mapper.Map<HiddenLocationEncounter>(encounterDto);
                }
                else if (encounterDto.EncounterType == API.Dtos.EncounterType.MISC)
                {
                    encounter = _mapper.Map<MiscEncounter>(encounterDto);
                }
                else
                {
                     throw new Exception("Error creating encounter,encounter must have a type");
                }

            encounter = _encounterRepository.Create(encounter);
            return _mapper.Map<EncounterDto>(encounter);


        }

        public Result<List<EncounterDto>> GetAll()
        {
            var result = _encounterRepository.GetAll();
            return MapToDto(result);
        }

        public Result<List<EncounterDto>> GetTouristRequestEncounters()
        {
            var result = _encounterRepository.GetTouristRequestEncounters();
            return MapToDto(result);
        }

        public Result<EncounterDto> GetById(long id)
        {
            var result = _encounterRepository.GetById(id);
            return MapToDto(result);
        }

        public Result<List<EncounterDto>> GetByTourId(int tourId)
        {
            var result = _encounterRepository.GetAllByTourId(tourId);
            return MapToDto(result);
        }

        public Result<EncounterDto> Update(EncounterDto encounterDto)
        {

            Encounter encounterUp;
            
                if (encounterDto.EncounterType == API.Dtos.EncounterType.SOCIAL)
                {
                    encounterUp = _mapper.Map<SocialEncounter>(encounterDto);
                }
                else if (encounterDto.EncounterType == API.Dtos.EncounterType.HIDDENLOCATION)
                {
                    encounterUp = _mapper.Map<HiddenLocationEncounter>(encounterDto);
                }
                else if (encounterDto.EncounterType == API.Dtos.EncounterType.MISC)
                {
                    encounterUp = _mapper.Map<MiscEncounter>(encounterDto);
                }
                else
                {
                    throw new Exception("Error updating encounter");
                }
           
           encounterUp = _encounterRepository.Update(encounterUp);
            
           if(encounterUp == null)
              return Result.Fail(FailureCode.NotFound).WithError("Encounter not found");


           return _mapper.Map<EncounterDto>(encounterUp);

        }
    }
}
