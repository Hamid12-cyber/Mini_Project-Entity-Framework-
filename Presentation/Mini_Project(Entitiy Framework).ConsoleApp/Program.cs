using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Application;
using Mini_Project_Entitiy_Framework_.Persistence;

namespace Mini_Project_Entitiy_Framework_.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddApplicationServices();
            services.AddInfrastructureServices();

            var serviceProvider = services.BuildServiceProvider();

            var menuManager = new MenuManager(serviceProvider);
            menuManager.Run();
        }
    }
}
