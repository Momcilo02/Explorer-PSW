using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public;

public interface IBundleService
{
    Result<BundleDto> Create(BundleDto bundle);
    Result<BundleDto> Update(BundleDto bundle);
    Result Delete (long id);
    Result<BundleDto> Get(long id);
    Result<PagedResult<BundleDto>> GetPaged(int page, int pageSize);
    Result<PagedResult<BundleDto>> GetPagedByCreatorId(long creatorId, int page, int pageSize);
    Result<BundleDto> Publish(BundleDto bundleDto);
    Result<BundleDto> Archive(BundleDto bundleDto);
    Result<PagedResult<BundleDto>> GetPublished(int page, int pageSize);
}

