using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration;

public interface IKeyPointService
{
    Result<KeyPointDto> Create(KeyPointDto keyPoint);
    Result<List<KeyPointDto>> GetAll();
    Result<KeyPointDto> Update(KeyPointDto keyPoint);

    Result<KeyPointDto> UpdateStatus(KeyPointDto keyPoint);
    
    Result<PagedResult<KeyPointDto>> GetPublicKeyPoints(int page, int pageSize);
}
