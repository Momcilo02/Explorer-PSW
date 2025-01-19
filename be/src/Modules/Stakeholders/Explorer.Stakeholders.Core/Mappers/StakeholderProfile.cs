using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Explorer.Tours.API.Dtos;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {

        CreateMap<TouristClubDto, TouristClub>().ReverseMap();



        CreateMap<PersonDto, Person>().ReverseMap();
        CreateMap<TourProblemReportDto, TourProblemReport>().ReverseMap();
        CreateMap<MessageDto, Message>().ReverseMap();
        CreateMap<NotificationDto, Notification>().ReverseMap();
        //CreateMap<TourProblemReportDto, TourProblemReport>().IncludeAllDerived().ForMember(key => key.Messages, opt => opt.MapFrom(src => src.Messages.Select((message, index) => new Message(message.UserId, message.ReportId, message.Content))));
        // Mapiramo DTO na domensku klasu i obratno
        CreateMap<ApplicationGradeDto, ApplicationGrade>().ReverseMap();
        CreateMap<ProfileMessageDto, ProfileMessage>().ReverseMap();
        CreateMap<ClubMessageDto, ClubMessage>().ReverseMap();
    }
}