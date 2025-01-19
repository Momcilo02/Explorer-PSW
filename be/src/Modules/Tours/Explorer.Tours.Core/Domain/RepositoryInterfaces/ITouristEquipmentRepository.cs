using Explorer.Tours.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITouristEquipmentRepository
    {
        TouristEquipment GetById(int id);
        List<TouristEquipment> GetAll(long id);
    }
}
