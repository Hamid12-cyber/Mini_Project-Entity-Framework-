using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;
using Mini_Project_Entitiy_Framework_.Applications.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Persistence;
using Mini_Project_Entitiy_Framework_.Infrastructure.Repositories;

namespace OnlineLibrary.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IReservedItemRepository, ReservedItemRepository>();

        return services;
    }
}
