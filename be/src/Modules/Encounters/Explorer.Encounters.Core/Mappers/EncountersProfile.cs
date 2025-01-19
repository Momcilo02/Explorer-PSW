using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Mappers
{
    public class EncountersProfile : Profile
    {
        public EncountersProfile() {

            // Mapiranje između DTO i osnovnih entiteta
            CreateMap<Encounter, EncounterDto>().Include<SocialEncounter, EncounterDto>()
                                             .Include<HiddenLocationEncounter, EncounterDto>()
                                             .Include<MiscEncounter, EncounterDto>()
                                             .ReverseMap();

            // Mapiranje za specifične entitete
            CreateMap<SocialEncounter, EncounterDto>().ReverseMap();
            CreateMap<HiddenLocationEncounter, EncounterDto>().ReverseMap();
            CreateMap<MiscEncounter, EncounterDto>().ReverseMap();


            CreateMap<EncounterExecutionDto, EncounterExecution>().ReverseMap();

        }
    }
}
