using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.TourExecutions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourExecutionRepository : CrudDatabaseRepository<TourExecution, ToursContext>, ITourExecutionRepository
    {

        private readonly ToursContext _dbContext;

        public TourExecutionRepository(ToursContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;

        }

        public Result Create(TourExecution tourExecution)
        {
            try
            {
                _dbContext.TourExecutions.Add(tourExecution);
                _dbContext.SaveChanges();

                return Result.Ok(); 
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        public TourExecution Update(TourExecution aggregateRoot)
        {
            
            _dbContext.Entry(aggregateRoot).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return aggregateRoot;
        }

        public void Delete(TourExecution aggregateRoot)
        {
            
            _dbContext.TourExecutions.Remove(aggregateRoot);
            _dbContext.SaveChanges();
        }

        public TourExecution Get(int id)
        {
            return _dbContext.TourExecutions
               .Where(te => te.Id == id)
               .FirstOrDefault();
        }

        public TourExecution GetByUserAndTourIds(int touristId, int tourId)
        {
            return _dbContext.TourExecutions
               .Where(te => te.TouristId == touristId
                           && te.TourId == tourId)
               .FirstOrDefault();
        }

        public List<TourExecution> GetAllByUserId(int touristId)
        {
            return _dbContext.TourExecutions
               .Where(te => te.TouristId == touristId)
               .ToList();

        }

        public TourExecution GetUserActiveTour(int touristId)
        {
            return _dbContext.TourExecutions
                .Where(te => te.TouristId == touristId
                            && te.Status == ExecutionStatus.ONGOING)
                .FirstOrDefault();
        }
    }
}
