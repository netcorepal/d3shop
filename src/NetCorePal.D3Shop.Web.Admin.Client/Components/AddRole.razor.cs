using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Web.Admin.Client.Services;

namespace NetCorePal.D3Shop.Web.Admin.Client.Components;

public partial class AddRole
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private IPermissionsService PermissionsService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;

    [Parameter] public EventCallback OnItemAdded { get; set; }

    private List<Permission> _allPermissions = [];
    private bool _modalVisible;
    private bool _modalConfirmLoading;
    private Form<CreateRoleModel> _form = default!;
    private Tabs _tabs = default!;
    private string[] _treeCheckedKeys = [];


    private CreateRoleModel _newRoleModel = new();

    private async Task ShowModal()
    {
        _modalVisible = true;
        _allPermissions = await GetAllPermissions();
    }

    private async Task<List<Permission>> GetAllPermissions()
    {
        var response = await PermissionsService.GetAll();
        if (response.Success) return response.Data.ToList();
        await Message.Error(response.Message);
        return [];
    }

    private async Task Form_OnFinish(EditContext editContext)
    {
        _modalConfirmLoading = true;
        StateHasChanged();
        var request =
            new CreateRoleRequest(_newRoleModel.Name, _newRoleModel.Description, _newRoleModel.PermissionCodes);
        var response = await RolesService.CreateRole(request);
        _modalConfirmLoading = false;
        if (response.Success)
        {
            await Message.Success("创建成功！");
            _modalVisible = false;
        }
        else
        {
            await Message.Error(response.Message);
        }
        await OnItemAdded.InvokeAsync();
    }

    private void Form_OnFinishFailed(EditContext editContext)
    {
        _tabs.GoTo(0);
    }

    private void Modal_HandleOk(MouseEventArgs e)
    {
        _form.Submit();
    }

    private void Modal_HandleCancel(MouseEventArgs e)
    {
        Console.WriteLine(@"Clicked cancel button");
        _modalVisible = false;
    }

    private void Tree_OnCheck(TreeEventArgs<string> e)
    {
        _newRoleModel.PermissionCodes = _allPermissions
            .Where(p => _treeCheckedKeys.Contains(p.Code))
            .Select(p => p.Code)
            .ToList();
    }
}

public class CreateRoleModel
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public List<string> PermissionCodes { get; set; } = [];
}