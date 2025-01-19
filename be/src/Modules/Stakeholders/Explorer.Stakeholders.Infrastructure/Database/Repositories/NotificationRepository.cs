using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
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
    public class NotificationRepository : CrudDatabaseRepository<Notification, StakeholdersContext>, INotificationRepository
    {
        private readonly StakeholdersContext _context;

        public NotificationRepository(StakeholdersContext context) : base(context)
        {
            _context = context;
        }

        public Notification? Get(int id)
        {
            return DbContext.Notifications
                .AsNoTracking() // Prevents tracking the entity instance
                .FirstOrDefault(t => t.Id == id);
        }

        public PagedResult<Notification> GetByLoggedUser(int idLogged, int page, int pageSize)
        {
            var task = _context.Notifications
                .Where(tpr => tpr.RecipientId == idLogged && tpr.IsRead == false)
                .GetPagedById(page, pageSize);

            task.Wait();
            return task.Result;
        }

       


    }
}
