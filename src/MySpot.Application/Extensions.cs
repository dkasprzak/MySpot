using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddScoped<IReservationsService, ReservationsService>();
        
        return services;
    }
}
