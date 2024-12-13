using AntDesign.TableModels;
using NetCorePal.D3Shop.Web.Admin.Client.Components.Identity.User;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Users
{
    [Inject] private IAdminUserService AdminUserService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;

    private PagedData<AdminUserResponse> _pagedAdminUsers = new(default!, default, default, default);

    private Table<AdminUserResponse> _table = default!;

    private readonly AdminUserQueryRequest _adminUserQueryRequest = new() { CountTotal = true };

    private bool _loading;

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        _table.ReloadData(1, 10);
    }

    private async Task GetPagedAdminUsers()
    {
        var response = await AdminUserService.GetAllAdminUsers(_adminUserQueryRequest);
        if (response.Success)
        {
            _pagedAdminUsers = response.Data;
            _adminUserQueryRequest.PageIndex = response.Data.PageIndex;
            _adminUserQueryRequest.PageSize = response.Data.PageSize;
        }
        else _ = Message.Error(response.Message);
    }

    private async Task HandleItemAdded()
    {
        await GetPagedAdminUsers();
    }

    private void HandleRolesUpdate(EditUserRoles.RolesUpdateSucceededEventArgs args)
    {
        var userItem = _pagedAdminUsers.Items.Single(au => au.Id == args.AdminUserId);
        userItem.Roles = args.RoleNames;
    }

    private async Task Delete(AdminUserResponse row)
    {
        if (!await Confirm($"确认删除用户：{row.Name}?"))
            return;
        var response = await AdminUserService.DeleteAdminUser(row.Id);
        if (response.Success)
        {
            _ = Message.Success("删除成功！");
            await GetPagedAdminUsers();
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
        _adminUserQueryRequest.PageIndex = 1;
        await GetPagedAdminUsers();
    }

    private async Task Table_OnChange(QueryModel<AdminUserResponse> obj)
    {
        _loading = true;
        await GetPagedAdminUsers();
        _loading = false;
    }
}