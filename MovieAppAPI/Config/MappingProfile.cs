using AutoMapper;
using MovieAppAPI.Entities;
using MovieAppAPI.Models;

namespace MovieAppAPI.Config;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<User, UserCreateModel>().ReverseMap().IgnoreNullProperties();
        CreateMap<User, UserUpdateModel>().ReverseMap().IgnoreNullProperties();
    }
}

public static class MappingExpressionExtension {
    public static void IgnoreNullProperties<TDestination, TSource>(
        this IMappingExpression<TDestination, TSource> expression) {
        expression.ForAllMembers(options =>
            options.Condition((src, dest, srcMember) => srcMember != null));
    }
}