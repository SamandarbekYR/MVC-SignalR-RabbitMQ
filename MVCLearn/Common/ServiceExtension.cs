using Newtonsoft.Json;

namespace MVCLearn.Common
{
    public static class ServiceExtension
    {
        public static void AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
        }
    }
}
