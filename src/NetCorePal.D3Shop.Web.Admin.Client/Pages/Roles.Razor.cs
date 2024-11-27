using AntDesign.TableModels;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Roles : IDisposable
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;

    private PersistingComponentStateSubscription _persistingSubscription;

    private PagedData<RoleResponse> _pagedRoles = default!;

    private ITable _table = default!;

    protected override async Task OnInitializedAsync()
    {
        const string persistKey = "roles";
        _persistingSubscription = ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson(persistKey, _pagedRoles);
            return Task.CompletedTask;
        });

        if (ApplicationState.TryTakeFromJson<PagedData<RoleResponse>>(persistKey, out var restored))
            _pagedRoles = restored!;
        else
            await GetPagedRoles();
    }

    private readonly RoleQueryRequest _roleQueryRequest =
        new() { PageIndex = 1, PageSize = 10, CountTotal = true };

    private async Task GetPagedRoles()
    {
        var response = await RolesService.GetAllRoles(_roleQueryRequest);
        if (response.Success)
        {
            _pagedRoles = response.Data;
            _roleQueryRequest.PageIndex = _pagedRoles.PageIndex;
            _roleQueryRequest.PageSize = _pagedRoles.PageSize;
        }
        else _ = Message.Error(response.Message);
    }

    private async Task HandleItemAdded()
    {
        await GetPagedRoles();
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
            await GetPagedRoles();
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

    private async Task OnSearch()
    {
        await GetPagedRoles();
    }

    private async Task Table_OnChange(QueryModel<RoleResponse> obj)
    {
        await GetPagedRoles();
    }

    public void Dispose()
    {
        _persistingSubscription.Dispose();
    }
}