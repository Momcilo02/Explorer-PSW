using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubMessageRepository : CrudDatabaseRepository<ClubMessage, StakeholdersContext>, IClubMessageRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<ClubMessage> _dbSet;

        public ClubMessageRepository(StakeholdersContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<ClubMessage>();
        }
        ICollection<ClubMessage> IClubMessageRepository.GetAll()
        {
            return _dbContext.ClubMessages.ToList();
        }

        public ClubMessage Create(ClubMessage message) {
            _dbContext.ClubMessages.Add(message);
            _dbContext.SaveChanges();
            return message; 
        }
        public ClubMessage Update(ClubMessage message)
        {
            var existingMessage = _dbContext.ClubMessages.FirstOrDefault(m => m.Id == message.Id);

            if (existingMessage == null)
            {
                throw new KeyNotFoundException($"ClubMessage with ID {message.Id} not found.");
            }

            existingMessage.Content = message.Content;
            existingMessage.SentDate = DateTime.UtcNow;
            existingMessage.SenderId = message.SenderId;

            _dbContext.ClubMessages.Update(existingMessage);
            _dbContext.SaveChanges();

            return existingMessage;
        }
        
        public ClubMessage Delete(long clubMessageId)
        {
            var entity = _dbSet.FirstOrDefault(k => k.Id == clubMessageId);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        
    }
}
