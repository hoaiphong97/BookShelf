using Infrastructure.Constant;
using Infrastructure.Extension;

namespace Infrastructure.Common
{
    public class AppSetting
    {
        public List<string> AllowedHosts { get; set; }
        public int? TokenLifetimeMinutes { get; set; }
        public int? PermanentTokenLifetimeDays { get; set; }
        public int? PermanentTokenRemainTimeThresholdHours { get; set; }
        public string? JWTSecret64Symbol { get; set; }
        public int? OTPTokenExpiredTimeMinutes { get; set; }
        public string? LoginUrl { get; set; }
        public string DefaultAvatar { get; set; }
        public string FileLogoName { get; set; }
        public List<string> BaseAppCoreDomains { get; set; }
        public ConnectionString? ConnectionStrings { get; set; }
        public string GetDefaultConnection => ConnectionStrings?.DefaultConnection.
            GetStringDefaultOrFromEnvValue(CoreConstant.CONNECTION_STRINGS);

        public AppSetting()
        {
            AllowedHosts = new List<string>();
            //BaseAppCoreDomains = new List<string>();
            TokenLifetimeMinutes = 1440;
            OTPTokenExpiredTimeMinutes = 15;
            PermanentTokenLifetimeDays = 7;
            PermanentTokenRemainTimeThresholdHours = 12;
            ConnectionStrings = new ConnectionString();
        }
    }
    public class ConnectionString
    {
        public string? DefaultConnection { get; set; }
    }
}
