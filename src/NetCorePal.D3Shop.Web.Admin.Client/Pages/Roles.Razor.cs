using Microsoft.AspNetCore.Components;
using NetCorePal.D3Shop.Web.Admin.Client.Services;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Roles : IDisposable
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;

    private PersistingComponentStateSubscription _persistingSubscription;

    private List<RoleResponse> _roleList = [];

    protected override async Task OnInitializedAsync()
    {
        const string persistKey = "roles";
        _persistingSubscription = ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson(persistKey, _roleList);
            return Task.CompletedTask;
        });

        if (ApplicationState.TryTakeFromJson<List<RoleResponse>>(persistKey, out var restored))
            _roleList = restored!;
        else
            _roleList = await GetAllRoles();
    }

    private async Task<List<RoleResponse>> GetAllRoles()
    {
        var response = await RolesService.GetAllRoles();
        if (response.Success) return response.Data.ToList();
        await Message.Error(response.Message);
        return [];
    }

    string _searchString = default!;
    
    public void Dispose()
    {
        _persistingSubscription.Dispose();
    }
}