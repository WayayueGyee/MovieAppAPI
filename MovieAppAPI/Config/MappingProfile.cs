using AutoMapper;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.Countries;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Config;

public class MappingProfile : Profile {
    public MappingProfile() {
        // User
        CreateMap<User, UserCreateModel>()
            // .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap()
            .IgnoreNullProperties();
        CreateMap<User, UserUpdateModel>()
            .ReverseMap()
            .ForMember(dest => dest.BirthDate, opt => opt.Condition((src, dest) => src.BirthDate != null))
            .IgnoreNullProperties();
        CreateMap<User, UserShortModel>();
        
        // Auth
        CreateMap<UserRegisterModel, UserCreateModel>();
        CreateMap<UserLogoutModel, InvalidToken>();

        // Movies
        CreateMap<MovieCreateModel, Movie>();
        CreateMap<MovieUpdateModel, Movie>().IgnoreNullProperties();
        CreateMap<Movie, MovieCreateResponseModel>();
        
        // Countries
        CreateMap<CountryCreateModel, Country>();
        CreateMap<CountryUpdateModel, Country>();
        
        // Reviews
        CreateMap<ReviewCreateModel, Review>();
        CreateMap<ReviewUpdateModel, Review>().IgnoreNullProperties();
        CreateMap<Review, ReviewModel>().AfterMap<MapReviewAndReviewModelAction>();
    }
}

public class MapReviewAndReviewModelAction : IMappingAction<Review, ReviewModel> {
    public void Process(Review source, ReviewModel destination, ResolutionContext context) {
        context.Mapper.Map(source.Author, destination.Author);
    }
}

public static class MappingExpressionExtension {
    public static void IgnoreNullProperties<TDestination, TSource>(
        this IMappingExpression<TDestination, TSource> expression) {
        expression.ForAllMembers(options =>
            options.Condition((src, dest, srcMember) => srcMember != null));
    }
}