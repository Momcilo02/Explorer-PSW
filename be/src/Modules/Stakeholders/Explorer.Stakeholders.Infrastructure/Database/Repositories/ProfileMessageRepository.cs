using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ProfileMessageRepository : CrudDatabaseRepository<ProfileMessage, StakeholdersContext>, IProfileMessageRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ProfileMessageRepository(StakeholdersContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }
        ICollection<ProfileMessage> GetAll()
        {
            return _dbContext.ProfileMessages.ToList();
        }
        void Create(ProfileMessage message)
        {
            _dbContext.ProfileMessages.Add(message);
            _dbContext.SaveChanges();
        }

        ICollection<ProfileMessage> IProfileMessageRepository.GetAll()
        {
            return _dbContext.ProfileMessages.ToList();
        }

        void IProfileMessageRepository.Create(ProfileMessage message)
        {
            _dbContext.ProfileMessages.Add(message);
            _dbContext.SaveChanges();
        }
    }
}
