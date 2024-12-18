using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Controllers.Identity;

namespace NetCorePal.D3Shop.Web.Blazor;

public static class BlazorServiceExtensions
{
    public static void AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<IRolesService, RoleController>();
        services.AddScoped<IAdminUserService, AdminUserController>();
        services.AddScoped<IDepartmentService, DepartmentController>();
        
    }
}