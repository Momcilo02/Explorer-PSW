

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IEncounterRepository
    {


        Encounter Create(Encounter encounter);

        Encounter Update(Encounter encounter);

        Encounter GetById(long id);

        List<Encounter> GetAll(); 

        List<Encounter> GetTouristRequestEncounters();
        List<Encounter> GetAllByTourId(int tourId);
    }
}
