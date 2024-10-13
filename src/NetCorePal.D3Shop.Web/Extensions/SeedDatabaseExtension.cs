using NetCorePal.D3Shop.Infrastructure;

namespace NetCorePal.D3Shop.Web.Extensions
{
    public static class SeedDatabaseExtension
    {
        internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var seeders = serviceScope.ServiceProvider.GetServices<ApplicationDbSeeder>();

            foreach (var seeder in seeders)
            {
                seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
            }
            return app;
        }
    }
}
