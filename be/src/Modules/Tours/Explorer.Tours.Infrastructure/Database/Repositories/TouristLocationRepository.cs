using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TouristLocationRepository : ITouristLocationRepository
    {
        public readonly ToursContext _context;

        public TouristLocationRepository(ToursContext context)
        {
            _context = context;
        }
        public TouristLocation GetByTouristId(long touristId)
        {
            return _context.TouristLocation.FirstOrDefault(tr => tr.TouristId == touristId);
        }
    }
}
