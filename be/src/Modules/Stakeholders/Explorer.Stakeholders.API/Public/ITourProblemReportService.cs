using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using Status = Explorer.Stakeholders.API.Dtos.Status;


namespace Explorer.Stakeholders.API.Public
{
    public interface ITourProblemReportService
    {
        Result<TourProblemReportDto> Get(int id);
        Result<PagedResult<TourProblemReportDto>> GetPaged(int page, int pageSize);
        Result<PagedResult<TourProblemReportDto>> GetByTouristId(int id, int page, int pageSize);
        PagedResult<TourProblemReportDto> GetByAuthorId(int authorId, int page, int pageSize);
        Result<TourProblemReportDto> Create(TourProblemReportDto tourProblemReport);
        Result<TourProblemReportDto> Update(TourProblemReportDto tourProblemReport);
        Result Delete(int id);
        Result<TourProblemReportDto> AddMessage(MessageDto messageDto, int userId, int reportId);
        Result<TourProblemReportDto> SetSolvingDeadline(int id, TourProblemReportDto tourProblemReportDto);
        Result<TourProblemReportDto> PenalizeAuthorAndCloseProblem(int id);
        Result<TourProblemReportDto> SetProblemAsSolvedOrUnsolved(int id, bool isSolved, string comment);
    }
}
