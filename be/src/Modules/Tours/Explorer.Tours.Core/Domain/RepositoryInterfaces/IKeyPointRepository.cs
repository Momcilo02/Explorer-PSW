using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IKeyPointRepository
{
    KeyPoint Create(KeyPoint keyPoint);
    List<KeyPoint> GetAll();
    void Delete(long id);
    KeyPoint Update(KeyPoint keyPoint);
    KeyPoint Get(long id);
    bool DoesExistByCoordinates(float longitude, float latitude);
    PagedResult<KeyPoint> GetPublicKeyPoints(int page, int pageSize);
}
