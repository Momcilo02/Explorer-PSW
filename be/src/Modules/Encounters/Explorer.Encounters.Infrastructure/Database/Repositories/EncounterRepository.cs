using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Domain;
using AutoMapper;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncounterRepository : CrudDatabaseRepository<Encounter, EncountersContext>, IEncounterRepository
    {
        private readonly EncountersContext _dbContext;
        private readonly IMapper _mapper;

        public EncounterRepository(EncountersContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Encounter> GetAll()
        {

            List<Encounter> encounters = _dbContext.Encounters.ToList();
            List<Encounter> ValidEncounters = new List<Encounter>();
            foreach (var encounter in encounters)
            {
                if(encounter.TouristRequestStatus.Value == Core.Domain.TouristEncounterStatus.ACCEPTED || encounter.TouristRequestStatus.Value == Core.Domain.TouristEncounterStatus.NOTTOURISTENCOUNTER)
                    ValidEncounters.Add(encounter);
            }
            return ValidEncounters; 
        }

        public List<Encounter> GetTouristRequestEncounters()
        {
            List<Encounter> encounters = _dbContext.Encounters.ToList();
            List<Encounter> ValidEncounters = new List<Encounter>();
            foreach (var encounter in encounters)
            {
                if (encounter.TouristRequestStatus.Value != Core.Domain.TouristEncounterStatus.NOTTOURISTENCOUNTER)
                    ValidEncounters.Add(encounter);
            }
            return ValidEncounters;
        }

        public Encounter GetById(long id)
        {
            return _dbContext.Encounters
             .Where(enc => enc.Id == id)
             .FirstOrDefault();

        }

        public List<Encounter> GetAllByTourId(int tourId)
        {
            return _dbContext.Encounters
             .Where(enc => enc.TourId == tourId)
             .ToList();

        }

        public Encounter Create(Encounter encounterDto)
        {
            try
            {
                
                Encounter encounter;

                if (encounterDto.EncounterType == EncounterType.SOCIAL)
                {
                    encounter = _mapper.Map<SocialEncounter>(encounterDto);
                    _dbContext.SocialEncounters.Add((SocialEncounter)encounter);
                }
                else if (encounterDto.EncounterType == EncounterType.HIDDENLOCATION)
                {
                    encounter = _mapper.Map<HiddenLocationEncounter>(encounterDto);
                    _dbContext.HiddenLocationEncounters.Add((HiddenLocationEncounter)encounter);
                }
                else if (encounterDto.EncounterType == EncounterType.MISC)
                {
                    encounter = _mapper.Map<MiscEncounter>(encounterDto);
                    _dbContext.MiscEncounters.Add((MiscEncounter)encounter);
                }
                else
                {
                    encounter = _mapper.Map<Encounter>(encounterDto);
                    _dbContext.Encounters.Add(encounter);
                }

                _dbContext.SaveChanges();

                // Vratimo mapirani DTO
                return _mapper.Map<Encounter>(encounter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating encounter", ex);
            }
        }



        public Encounter Update(Encounter encounterDto)
        {
            Encounter encounter;

            if (encounterDto.EncounterType == EncounterType.SOCIAL)
            {
                encounter = _mapper.Map<SocialEncounter>(encounterDto);
                _dbContext.SocialEncounters.Update((SocialEncounter)encounter);
            }
            else if (encounterDto.EncounterType == EncounterType.HIDDENLOCATION)
            {
                encounter = _mapper.Map<HiddenLocationEncounter>(encounterDto);
                _dbContext.HiddenLocationEncounters.Update((HiddenLocationEncounter)encounter);
            }
            else if (encounterDto.EncounterType == EncounterType.MISC)
            {
                encounter = _mapper.Map<MiscEncounter>(encounterDto);
                _dbContext.MiscEncounters.Update((MiscEncounter)encounter);
            }
            else
            { 
                encounter = _mapper.Map<Encounter>(encounterDto); 
                _dbContext.Encounters.Update(encounter);
            }

            _dbContext.SaveChanges();

            // Vratimo mapirani DTO
            return _mapper.Map<Encounter>(encounter);
        }




    }
}
