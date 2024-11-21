using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


//using In
namespace Persistence.Service
{
    public static class ConfigureDatabase
    {
        public static DbContextOptionsBuilder AddSqlConfiguration(this DbContextOptionsBuilder options, IConfiguration config)
        {
            var appSetting = new AppSetting();
            config.GetSection("AppSetting").Bind(appSetting);

            options.UseSqlServer(appSetting.GetDefaultConnection);
            return options;
        }
    }
}
