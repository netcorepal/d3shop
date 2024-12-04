using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Extensions
{
    public static class SeedDatabaseExtension
    {
        internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (dbContext.AdminUsers.Any(u => u.Name == AppDefaultCredentials.Name)) return app;

            var adminUser = new AdminUser(AppDefaultCredentials.Name, "",
                PasswordHasher.HashPassword(AppDefaultCredentials.Password), [], []);
            dbContext.AdminUsers.Add(adminUser);
            dbContext.SaveChanges();
            return app;
        }
    }
}