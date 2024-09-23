using AutoMapper;

namespace PIMTool.Api.Extensions;

public static class IMapperConfigurationExpressionExtensions
{
    public static void AddAllProfiles<T>(this IMapperConfigurationExpression configuration) where T : Profile, new()
    {
        var profiles = typeof(T).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(Profile)))
            .Select(t => Activator.CreateInstance(t) as Profile);
        configuration.AddProfiles(profiles);
    }
}
