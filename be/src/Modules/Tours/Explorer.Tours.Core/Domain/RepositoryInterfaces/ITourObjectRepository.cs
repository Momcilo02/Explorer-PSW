using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourObjectRepository
    {
        List<TourObject> GetAll();
        PagedResult<TourObject> GetPublicObjects(int page, int pageSize);
    }
}
