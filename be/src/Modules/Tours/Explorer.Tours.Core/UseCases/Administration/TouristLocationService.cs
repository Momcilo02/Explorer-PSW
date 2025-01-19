using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases
{
    public class TouristLocationService : CrudService<TouristLocationDto, TouristLocation>, ITouristLocationService
    {
        private readonly ITouristLocationRepository _touristLocationRepository;
        public TouristLocationService(ICrudRepository<TouristLocation> repository, IMapper mapper,ITouristLocationRepository touristLocationRepository) : base(repository, mapper)
        {
            _touristLocationRepository = touristLocationRepository;
        }
        public Result<TouristLocationDto> GetByTouristId(long id)
        {
            var result = _touristLocationRepository.GetByTouristId(id);
            return MapToDto(result);
        }
    }
}
