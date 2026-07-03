using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;

namespace Mini_Project_Entitiy_Framework_.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<LibraryDbContext>();

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IReservedItemRepository, ReservedItemRepository>();

        return services;
    }
}
