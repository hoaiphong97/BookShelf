using Infrastructure.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public static class CorsPolicyService
    {
        public static IServiceCollection AddCorsPolicyServices(this IServiceCollection services,
    IConfiguration config)
        {
            var appSetting = new AppSetting();
            config.GetSection("AppSetting").Bind(appSetting);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                 builder =>
                 {
                     builder.WithOrigins(appSetting.AllowedHosts.ToArray())
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
                 });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsPolicyServices(this IApplicationBuilder app)
        {
            var appSetting = app.ApplicationServices.GetRequiredService<IOptions<AppSetting>>();

            app.UseCors(x => x.WithOrigins(appSetting.Value.AllowedHosts.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader());

            return app;
        }
    }
}
