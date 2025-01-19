using Explorer.BuildingBlocks.Core.UseCases;
using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Explorer.Tours.API.Internal;
using FluentResults;
using ProblemPriority = Explorer.Stakeholders.API.Dtos.ProblemPriority;
using Status = Explorer.Stakeholders.API.Dtos.Status;
using System.Diagnostics;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class TourProblemReportService : CrudService<TourProblemReportDto, TourProblemReport>, ITourProblemReportService
    {
        private readonly IMapper _mapper;
        private readonly ICrudRepository<TourProblemReport> _repository;
        private readonly ITourProblemReportRepository _tourProblemReportRepository;
        private readonly IInternalTourService _internalTourService;
        private readonly INotificationService _notificationService;
        public TourProblemReportService(ICrudRepository<TourProblemReport> repository, IMapper mapper, ITourProblemReportRepository tourProblemReportRepository, IInternalTourService internalTourService, INotificationService notificationService) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _tourProblemReportRepository = tourProblemReportRepository;
            _internalTourService = internalTourService;
            _notificationService = notificationService;

        }
        public Result<TourProblemReportDto> Get(int id)
        {
            var result = _tourProblemReportRepository.Get(id);
            return MapToDto(result);
        }
        public Result<PagedResult<TourProblemReportDto>> GetPaged(int page, int pageSize)
        {
            var result = _tourProblemReportRepository.GetPaged(page, pageSize);
            return MapToDto(result);
        }

        public Result<PagedResult<TourProblemReportDto>> GetByTouristId(int id, int page, int pageSize)
        {
            var result = _tourProblemReportRepository.GetByTouristId(id, page, pageSize);
            return MapToDto(result);
        }

        public PagedResult<TourProblemReportDto> GetByAuthorId(int authorId, int page, int pageSize)
        {
            var reports = _tourProblemReportRepository.GetPaged(page, pageSize);
            var authorReports = new List<TourProblemReportDto>();

            foreach (var report in reports.Results)
            {
                var tourResult = _internalTourService.Get(report.TourId);
                if(!tourResult.IsSuccess || tourResult.Value.AuthorId != authorId) continue;
                var reportDto = new TourProblemReportDto
                {
                    Id = (int)report.Id,
                    TourId = report.TourId,
                    Category = report.Category,
                    Priority = (ProblemPriority)report.Priority,
                    Description = report.Description,
                    Time = report.Time,
                    Status = (Status)report.Status,
                    TouristId = report.TouristId,
                    Comment = report.Comment,
                    SolvingDeadline = report.SolvingDeadline ?? DateTime.MinValue,
                    Messages = report.Messages.Select(message => new MessageDto
                    {
                        UserId = message.UserId,
                        ReportId = message.ReportId,
                        Content = message.Content
                    }).ToList()
                };
                authorReports.Add(reportDto);
            }

            return new PagedResult<TourProblemReportDto>(authorReports, authorReports.Count);
        }

        public Result<TourProblemReportDto> AddMessage(MessageDto messageDto, int userId, int reportId)
        {
            try
            {
                var report = _tourProblemReportRepository.Get(reportId);
                if (report == null) throw new Exception("Report not found.");

                var message = _mapper.Map<MessageDto, Message>(messageDto);
                message.UserId = userId;
                message.ReportId = reportId;

                report.AddMessage(message);

                var result = _tourProblemReportRepository.Update(report);
                //********Iva********
                var autorId = _internalTourService.Get(report.TourId).Value.AuthorId;
                if (userId == autorId)
                {
                    NotificationDto notification = new NotificationDto();
                    notification.ReportId = reportId;
                    notification.RecipientId = _tourProblemReportRepository.Get(reportId).TouristId;
                    notification.IsRead = false;
                    notification.NotificationType = API.Dtos.NotificationType.CHAT;
                    _notificationService.Create(notification);
                }
                else
                {
                    NotificationDto notification = new NotificationDto();
                    notification.ReportId = reportId;
                    notification.RecipientId = autorId;
                    notification.IsRead = false;
                    notification.NotificationType = API.Dtos.NotificationType.CHAT;
                    _notificationService.Create(notification);
                }
                //*******************

                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourProblemReportDto> SetSolvingDeadline(int id, TourProblemReportDto tourProblemReport)
        {
            Debug.WriteLine("SetSolvingDeadline called for id: " + id);

            var aggregate = _repository.Get(id);
            if (aggregate == null)
            {
                throw new Exception("Agregat TourProblemReport nije pronađen");
            }

            aggregate.SetSolvingDeadline(tourProblemReport.SolvingDeadline);

            _repository.Update(aggregate);

            var report = _tourProblemReportRepository.Get((int)aggregate.Id);
            var autorId = _internalTourService.Get(report.TourId).Value.AuthorId;
            NotificationDto notification = new NotificationDto();
            notification.ReportId = (int)aggregate.Id;
            notification.RecipientId = autorId;
            notification.IsRead = false;
            notification.NotificationType = API.Dtos.NotificationType.DEADLINE;
            _notificationService.Create(notification);


            var dto = _mapper.Map<TourProblemReportDto>(aggregate);
            return Result.Ok(dto);
        }

        public Result<TourProblemReportDto> PenalizeAuthorAndCloseProblem(int id)
        {
            var tourProblemReport = _repository.Get(id);
            if (tourProblemReport == null)
            {
                throw new Exception("TourProblemReport not found");
            }

            if (tourProblemReport.SolvingDeadline == null || (!(tourProblemReport.SolvingDeadline < DateTime.UtcNow) ||
                                                              tourProblemReport.Status ==
                                                              Domain.TourProblemReports.Status.SOLVED))
                return Result.Fail(
                    "Cannot penalize the author because the solving deadline has not passed yet or problem is solved.");
            _internalTourService.CloseTour(tourProblemReport.TourId);

            tourProblemReport.CloseUnsolvedProblem();
            var result = _tourProblemReportRepository.Update(tourProblemReport);

            var dto = _mapper.Map<TourProblemReportDto>(tourProblemReport);
            return Result.Ok(dto);
        }

        public Result<TourProblemReportDto> SetProblemAsSolvedOrUnsolved(int id, bool isSolved, string comment)
        {
            var tourProblemReport = _repository.Get(id);
            if (tourProblemReport == null)
            {
                throw new Exception("TourProblemReport not found");
            }
            if (tourProblemReport.Status is Domain.TourProblemReports.Status.CLOSED or Domain.TourProblemReports.Status.SOLVED or Domain.TourProblemReports.Status.UNSOLVED)
                return Result.Fail(
                    "Cannot change status to solved/unsolved when current status is closed or already solved/unsolved.");
            
            if (isSolved) tourProblemReport.SetAsSolved();
            else tourProblemReport.SetAsUnsolved(comment);

            _tourProblemReportRepository.Update(tourProblemReport);

            var dto = _mapper.Map<TourProblemReportDto>(tourProblemReport);
            return Result.Ok(dto);
        }
    }
}
