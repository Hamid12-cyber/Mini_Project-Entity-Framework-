using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Application;
using Mini_Project_Entitiy_Framework_.Persistence;
using System.Text;

namespace Mini_Project_Entitiy_Framework_.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            var services = new ServiceCollection();

            services.AddApplicationServices();
            services.AddInfrastructureServices();

            var serviceProvider = services.BuildServiceProvider();

            var menuManager = new MenuManager(serviceProvider);
            menuManager.Run();
        }
    }
}
