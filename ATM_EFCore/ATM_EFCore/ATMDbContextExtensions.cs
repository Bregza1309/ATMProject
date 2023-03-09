using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;//ISerrviceCollection
namespace ATM_EFCore;
public static class ATMDbContextExtensions
{
    public static IServiceCollection AddAtmContext(
        this IServiceCollection services , string connectionString = 
        @"Data Source=.;Initial Catalog=ATMDb;Integrated Security=true;Encrypt=false;MultipleActiveResultSets=true;")
    {
        services.AddDbContext<ATMdb>(options => 
        options.UseSqlServer(connectionString));

        return services;
    }
}
