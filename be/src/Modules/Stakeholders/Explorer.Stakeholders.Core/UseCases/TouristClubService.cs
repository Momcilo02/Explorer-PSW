using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class TouristClubService : CrudService<TouristClubDto, TouristClub>, IToursitClubService
    {
        private readonly ITouristClubRepository _touristClubRepository;

        public TouristClubService(ICrudRepository<TouristClub> repository, ITouristClubRepository touristClubRepository, IMapper mapper) : base(repository, mapper) {
            _touristClubRepository = touristClubRepository;
        }

        public Result<TouristClubDto> Get(int id)
        {
            try
            {
                var touristClubs = _touristClubRepository.GetAll();
                var touristClub = touristClubs.FirstOrDefault(t => t.Id == id);

                return MapToDto(touristClub);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
    }

}
