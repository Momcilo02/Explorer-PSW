using Explorer.BuildingBlocks.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncounterExecutionRepository : CrudDatabaseRepository<EncounterExecution, EncountersContext>, IEncounterExecutionRepository
    {

        private readonly EncountersContext _dbContext;
        public EncounterExecutionRepository(EncountersContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
        public Result Create(EncounterExecution encounterExecution)
        {
            try
            {
                _dbContext.EncounterExecution.Add(encounterExecution);
                _dbContext.SaveChanges();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        public EncounterExecution Update(EncounterExecution encounterExecution)
        {
            _dbContext.Update(encounterExecution);
            _dbContext.SaveChanges();

            return encounterExecution;
        }

        public void Delete(EncounterExecution encounterExecution)
        {

            _dbContext.EncounterExecution.Remove(encounterExecution);
            _dbContext.SaveChanges();
        }

        public List<EncounterExecution> GetAll()
        {
            return _dbContext.EncounterExecution.ToList();
        }

        public EncounterExecution Get(int id)
        {
            return _dbContext.EncounterExecution
                .Where(te => te.Id == id)
                .FirstOrDefault();
        }

        public List<EncounterExecution> GetAllByUserId(int touristId)
        {
            return _dbContext.EncounterExecution
                .Where(te => te.TouristId == touristId)
                .ToList();

        }


        public bool HasAlreadyActivatedEncounter(int touristId)
        {
            var encExec = _dbContext.EncounterExecution
                .Where(te => te.TouristId == touristId && te.Status == EncounterExecutionStatus.ACTIVATED)
                .FirstOrDefault();
            return encExec != null;

        }
        public EncounterExecution GetUserActiveEncounter(int touristId)
        {
            return _dbContext.EncounterExecution
                    .FirstOrDefault(ee => ee.TouristId == touristId
                                 && ee.Status == EncounterExecutionStatus.ACTIVATED);
        }



        /****************** IVA ******************/
        public bool TouristCompletedEncounterForTour(int touristId, int encounterId)
        {
            var encExec = _dbContext.EncounterExecution
                .Where(te => te.TouristId == touristId && te.EncounterId==encounterId && te.Status == EncounterExecutionStatus.COMPLETED)
                .FirstOrDefault();
            return encExec != null;
        }
    }
}
