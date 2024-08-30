using Microsoft.EntityFrameworkCore;
using MVCLearn.DataAcess.Data;

namespace ReceiverApp.Configurations
{
    public static class CongiguretionDatabase
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
