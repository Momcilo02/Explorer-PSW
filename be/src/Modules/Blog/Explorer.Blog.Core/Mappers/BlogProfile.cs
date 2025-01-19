using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<RatingDto, Domain.Rating>().ReverseMap();
        //CreateMap<BlogDto, Domain.Blog>().ReverseMap();
        CreateMap<BlogDto, Domain.Blog>().IncludeAllDerived()
            .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings !=null 
            ? src.Ratings.Select(rat => new Rating(rat.Grade, rat.UserId, rat.CreationTime)).ToList()
            : new List<Rating>())).ReverseMap();

        CreateMap<CommentDto,Domain.Comment>().ReverseMap();
        
    }
}