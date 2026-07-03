using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Services;
using OnlineLibrary.Application.Services;

namespace OnlineLibrary.Application;

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
