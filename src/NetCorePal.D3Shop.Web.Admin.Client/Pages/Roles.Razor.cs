using AntDesign.TableModels;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Roles
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;

    private PagedData<RoleResponse> _pagedRoles = PagedData<RoleResponse>.Empty;

    private ITable _table = default!;

    private readonly RoleQueryRequest _roleQueryRequest = new() { CountTotal = true };

    private bool _loading;

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        _table.ReloadData(1, 10);
    }

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
        _roleQueryRequest.PageIndex = 1;
        await GetPagedRoles();
    }

    private async Task Table_OnChange(QueryModel<RoleResponse> obj)
    {
        _loading = true;
        await GetPagedRoles();
        _loading = false;
    }
}