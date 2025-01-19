using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class TouristClubRepository : CrudDatabaseRepository<TouristClub, StakeholdersContext>, ITouristClubRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<TouristClub> _dbSet;

        public TouristClubRepository(StakeholdersContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TouristClub>();
        }
        ICollection<TouristClub> ITouristClubRepository.GetAll()
        {
            return _dbContext.TouristClub.ToList();
        }

    }
}
