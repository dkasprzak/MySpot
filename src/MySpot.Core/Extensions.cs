using Microsoft.Extensions.DependencyInjection;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}
