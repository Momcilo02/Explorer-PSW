using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        Notification Get(int id);
        PagedResult<Notification> GetByLoggedUser(int idLogged, int page, int pageSize);
        PagedResult<Notification> GetPaged(int page, int pageSize);
       
    }
}
