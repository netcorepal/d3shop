namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Roles : IDisposable
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;

    private PersistingComponentStateSubscription _persistingSubscription;

    private List<RoleResponse> _roleList = [];

    private ITable _table = default!;

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

    private async Task<List<RoleResponse>> GetAllRoles(string? name = null, string? description = null)
    {
        var queryRequest = new RoleQueryRequest(name, description);
        var response = await RolesService.GetAllRoles(queryRequest);
        if (response.Success) return response.Data.ToList();
        _ = Message.Error(response.Message);
        return [];
    }

    private async Task HandleItemAdded()
    {
        _roleList = await GetAllRoles();
    }

    private void HandleItemUpdated()
    {
        _table.ReloadData();
    }

    private async Task Delete(RoleResponse row)
    {
        if (!await Confirm($"确认删除角色：{row.Name}?"))
            return;
        var response = await RolesService.DeleteRole(row.Id);
        if (response.Success)
        {
            _ = Message.Success("删除成功！");
            _roleList = await GetAllRoles();
        }
        else
        {
            _ = Message.Error(response.Message);
        }
    }

    private async Task<bool> Confirm(string message)
    {
        return await ConfirmService.Show(message, "警告", ConfirmButtons.YesNo, ConfirmIcon.Warning) == ConfirmResult.Yes;
    }

    private string _searchString = default!;

    private async Task OnSearch()
    {
        _roleList = await GetAllRoles(_searchString);
    }

    public void Dispose()
    {
        _persistingSubscription.Dispose();
    }
}