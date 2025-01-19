using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions;
using TourObject = Explorer.Tours.Core.Domain.TourObject;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<TourEquipmentDto, TourEquipment>().ReverseMap();
        CreateMap<TourReviewDto, TourReview>().ReverseMap();
        CreateMap<KeyPointDto, KeyPoint>().ReverseMap();
        CreateMap<TourDto, Tour>().ReverseMap();
        CreateMap<TourObjectDto, TourObject>().ReverseMap();
        CreateMap<TouristLocationDto, TouristLocation>().ReverseMap();
        CreateMap<CompletedKeyPointsDto, CompletedKeyPoints>().ReverseMap();
        CreateMap<TourExecutionDto, TourExecution>()
                .ForMember(dest => dest.CompletedKeyPoints,
                 opt => opt.MapFrom(src => src.CompletedKeyPoints != null
                ? src.CompletedKeyPoints
                .Select(point => new CompletedKeyPoints(point.ExecutionTime, point.CompletedKeyPointId))
                .ToList()
            : new List<CompletedKeyPoints>())).ReverseMap();

        CreateMap<TourDurationDto, TourDuration>().ReverseMap();
        CreateMap<TourExecutionDto, TourExecution>().ReverseMap();
        CreateMap<BasicTourDetailsDto, Tour>().ReverseMap();
        CreateMap<TourDto,Tour>()
                .ForMember(dur=>dur.TourDurations,
                 opt=>opt.MapFrom(src=>src.TourDurations != null
                ? src.TourDurations
                .Select(duration=>new TourDuration(duration.Duration,(Domain.TransportType)duration.TransportType,(Domain.TimeUnit)duration.TimeUnit))
                .ToList()
                :new List<TourDuration>())).ReverseMap();
        CreateMap<QuizDto, Quiz>()
      .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
      .ForMember(dest => dest.Reward, opt => opt.MapFrom(src => src.Reward))
      .ReverseMap()
      .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
      .ForMember(dest => dest.Reward, opt => opt.MapFrom(src => src.Reward));

        CreateMap<QuizQuestionDto, QuizQuestion>()
    .ForMember(dest => dest.Answers,
        opt => opt.MapFrom(src => src.Answers.Select(a => new QuizAnswer(a.AnswerText))))
    .ForMember(dest => dest.CorrectAnswerId,
        opt => opt.MapFrom(src => src.CorrectAnswerIndex)) // Popravljeno mapiranje
    .ReverseMap()
    .ForMember(dest => dest.CorrectAnswerIndex,
        opt => opt.MapFrom(src => src.CorrectAnswerId)); // Obrnuto mapiranje


        CreateMap<QuizAnswerDto, QuizAnswer>()
            .ConstructUsing(dto => new QuizAnswer(dto.AnswerText))
            .ReverseMap()
            .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText));

        CreateMap<Reward, RewardDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<RewardType>(src.Type)));
        CreateMap<TouristEquipmentDto, TouristEquipment>().ReverseMap();
    }
}