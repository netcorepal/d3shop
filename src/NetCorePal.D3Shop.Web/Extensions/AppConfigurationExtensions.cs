namespace NetCorePal.D3Shop.Web.Extensions
{
    public static class AppConfigurationExtensions
    {
        internal static AppConfiguration GetApplicationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>()
                ?? throw new InvalidOperationException("Application settings is not configured.");
        }
    }
}
