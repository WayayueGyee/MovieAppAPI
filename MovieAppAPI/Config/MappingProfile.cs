using AutoMapper;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.Users;
using NuGet.Common;

namespace MovieAppAPI.Config;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<User, UserCreateModel>()
            // .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap()
            .IgnoreNullProperties();
        CreateMap<User, UserUpdateModel>()
            .ReverseMap()
            .ForMember(dest => dest.BirthDate, opt => opt.Condition((src, dest) => src.BirthDate != null))
            .IgnoreNullProperties();

        CreateMap<UserRegisterModel, UserCreateModel>();
        CreateMap<UserLogoutModel, InvalidToken>();
    }
}

public static class MappingExpressionExtension {
    public static void IgnoreNullProperties<TDestination, TSource>(
        this IMappingExpression<TDestination, TSource> expression) {
        expression.ForAllMembers(options =>
            options.Condition((src, dest, srcMember) => srcMember != null));
    }
}