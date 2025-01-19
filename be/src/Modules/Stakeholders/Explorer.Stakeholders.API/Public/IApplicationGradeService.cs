using FluentResults;
using Explorer.Stakeholders.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.API.Public
{
    public interface IApplicationGradeService
    {
        Result<PagedResult<ApplicationGradeDto>> GetPaged(int page, int pageSize);
        Result<ApplicationGradeDto> Create(ApplicationGradeDto appgrade);
        Result<ApplicationGradeDto> Update(ApplicationGradeDto appgrade);
    }
}
