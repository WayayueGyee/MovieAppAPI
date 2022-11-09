using AutoMapper;
using Microsoft.Win32;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.Countries;
using MovieAppAPI.Models.Genres;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Config;

public class MappingProfile : Profile {
    public MappingProfile() {
        // Disable Null
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<TimeSpan?, TimeSpan>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);

        // User
        CreateMap<User, UserCreateModel>()
            // .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap()
            .IgnoreNullProperties();
        CreateMap<User, UserUpdateModel>()
            .ReverseMap()
            .ForMember(dest => dest.BirthDate, opt => opt.Condition((src, _) => src.BirthDate != null))
            .IgnoreNullProperties();
        CreateMap<User, UserShortModel>();

        // Auth
        CreateMap<UserRegisterModel, UserCreateModel>();
        CreateMap<UserLogoutModel, InvalidToken>();

        // Account
        CreateMap<User, ProfileModel>();
        CreateMap<ProfileUpdateModel, User>().IgnoreNullProperties();

        // Movies
        CreateMap<MovieCreateModel, Movie>();
        CreateMap<MovieUpdateModel, Movie>().IgnoreNullProperties();
        CreateMap<Movie, MovieCreateResponseModel>();
        CreateMap<Movie, MovieDetailsModel>().AfterMap<MapMovieAndMovieDetailsModel>();
        CreateMap<Movie, MovieElementModel>().AfterMap<MapMovieAndMovieElementModel>();

        // Countries
        CreateMap<CountryCreateModel, Country>();
        CreateMap<CountryUpdateModel, Country>();

        // Reviews
        CreateMap<ReviewCreateModel, Review>();
        CreateMap<ReviewUpdateModel, Review>().IgnoreNullProperties();
        CreateMap<Review, ReviewModel>().AfterMap<MapReviewAndReviewModelAction>();
        CreateMap<Review, ReviewShortModel>();

        // Genres
        CreateMap<Genre, GenreModel>();
    }
}

public class MapReviewAndReviewModelAction : IMappingAction<Review, ReviewModel> {
    public void Process(Review source, ReviewModel destination, ResolutionContext context) {
        context.Mapper.Map(source.Author, destination.Author);
    }
}

public class MapMovieAndMovieDetailsModel : IMappingAction<Movie, MovieDetailsModel> {
    public void Process(Movie source, MovieDetailsModel destination, ResolutionContext context) {
        context.Mapper.Map(source.Reviews, destination.Reviews);
        context.Mapper.Map(source.Genres, destination.Genres);
    }
}

public class MapMovieAndMovieElementModel : IMappingAction<Movie, MovieElementModel> {
    public void Process(Movie source, MovieElementModel destination, ResolutionContext context) {
        context.Mapper.Map(source.Reviews, destination.Reviews);
        context.Mapper.Map(source.Genres, destination.Genres);
    }
}

public static class MappingExpressionExtension {
    public static void IgnoreNullProperties<TDestination, TSource>(
        this IMappingExpression<TDestination, TSource> expression) {
        expression.ForAllMembers(options =>
            options.Condition((_, _, srcMember) => srcMember != null));
    }
}