using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TouristEquipmentRepository : ITouristEquipmentRepository
    {
        private readonly ToursContext _dbContext;
        public TouristEquipmentRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<TouristEquipment> GetAll(long id)
        {
            return _dbContext.TouristEquipments.Where(t => t.TouristId == id).ToList();
        }
        public TouristEquipment GetById(int id)
        {
            return _dbContext.TouristEquipments.FirstOrDefault(e => e.Id == id);
        }

    }
}