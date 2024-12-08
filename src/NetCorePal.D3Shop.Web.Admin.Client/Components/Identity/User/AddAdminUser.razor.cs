using Microsoft.AspNetCore.Components.Forms;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Admin.Client.Components.Identity.User;

public partial class AddAdminUser
{
    [Inject] private IAdminUserService AdminUserService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Parameter] public EventCallback OnItemAdded { get; set; }

    private bool _modalVisible;
    private bool _modalConfirmLoading;
    private Tabs _tabs = default!;
    private CreateAdminUserRequest _newUserModel = new();
    private Form<CreateAdminUserRequest> _form = default!;
    private CheckboxOption<RoleId>[] _roleOptions = [];
    private RoleId[] _selectedRoleIds = [];

    private async Task ShowModal()
    {
        _modalVisible = true;
        var allRoles = await GetAllRoleNames();
        _roleOptions = allRoles.Select(x => new CheckboxOption<RoleId>
        {
            Label = x.RoleName,
            Value = x.RoleId
        }).ToArray();
    }

    private async Task<List<AdminUserRoleResponse>> GetAllRoleNames()
    {
        var response = await AdminUserService.GetAllRolesForCreateUser();
        if (response.Success) return response.Data.ToList();
        _ = Message.Error(response.Message);
        return [];
    }

    private void CloseModal()
    {
        _modalVisible = false;
        _selectedRoleIds = [];
        _newUserModel = new CreateAdminUserRequest();
        _tabs.GoTo(0);
    }

    private async Task Form_OnFinish(EditContext editContext)
    {
        _modalConfirmLoading = true;
        StateHasChanged();
        _newUserModel.RoleIds = _selectedRoleIds;
        var response = await AdminUserService.CreateAdminUser(_newUserModel);
        if (response.Success)
        {
            _ = Message.Success("创建成功！");
            CloseModal();
            await OnItemAdded.InvokeAsync();
        }
        else
        {
            _ = Message.Error(response.Message);
        }

        _modalConfirmLoading = false;
    }

    private void Form_OnFinishFailed(EditContext editContext)
    {
        _tabs.GoTo(0);
    }
}