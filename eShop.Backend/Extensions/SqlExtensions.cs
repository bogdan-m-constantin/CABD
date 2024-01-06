using Microsoft.AspNetCore.Cors.Infrastructure;

namespace eShop.Backend.Extensions
{
    public record SqlOptions(string ConnectionString);

    public static class SqlExtensions
    {
        public static void AddSql(this IServiceCollection services, IConfiguration config) {
            services.AddSingleton(new SqlOptions(config.GetConnectionString("Main")!));
        }
    }
    public static class CorsExtensions
    {
        public const string CorsPolicyName = "CorsPolicy";
        public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            CorsOptions corsConfig = new();
            configuration.GetSection("cors").Bind(corsConfig);
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                ;
                    });
            });
        }
    }
}
