using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces;

public interface IBundleRepository
{
    Bundle Create(Bundle bundle);
    PagedResult<Bundle> GetPaged(int page, int pageSize);
    void Delete(long id);
    Bundle Update(Bundle bundle);
    Bundle Get(long id);
    PagedResult<Bundle> GetPagedByCreatorId(long creatorId, int page, int pageSize);
    PagedResult<Bundle> GetPublished(int page, int pageSize);
    Bundle GetById(long id);
}
