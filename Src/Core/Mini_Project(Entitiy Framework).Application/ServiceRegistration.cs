using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;
using Mini_Project_Entitiy_Framework_.Application.Services;

namespace Mini_Project_Entitiy_Framework_.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }
}
