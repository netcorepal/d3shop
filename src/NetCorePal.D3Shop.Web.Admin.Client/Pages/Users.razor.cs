using AntDesign.TableModels;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Users : IDisposable
{
    [Inject] private IAdminUserService AdminUserService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;

    private PersistingComponentStateSubscription _persistingSubscription;

    private PagedData<AdminUserResponse> _pagedAdminUsers = default!;

    protected override async Task OnInitializedAsync()
    {
        const string persistKey = "adminUsers";
        _persistingSubscription = ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson(persistKey, _pagedAdminUsers);
            return Task.CompletedTask;
        });

        if (ApplicationState.TryTakeFromJson<PagedData<AdminUserResponse>>(persistKey, out var restored))
            _pagedAdminUsers = restored!;
        else
            await GetPagedAdminUsers();
    }

    private readonly AdminUserQueryRequest _adminUserQueryRequest =
        new() { PageIndex = 1, PageSize = 10, CountTotal = true };

    private async Task GetPagedAdminUsers()
    {
        var response = await AdminUserService.GetAllAdminUsers(_adminUserQueryRequest);
        if (response.Success)
        {
            _pagedAdminUsers = response.Data;
            _adminUserQueryRequest.PageIndex = _pagedAdminUsers.PageIndex;
            _adminUserQueryRequest.PageSize = _pagedAdminUsers.PageSize;
        }
        else _ = Message.Error(response.Message);
    }

    private async Task HandleItemAdded()
    {
        await GetPagedAdminUsers();
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

    private string _searchString = default!;

    private async Task OnSearch()
    {
        await GetPagedAdminUsers();
    }

    private async Task Table_OnChange(QueryModel<AdminUserResponse> obj)
    {
        await GetPagedAdminUsers();
    }

    public void Dispose()
    {
        _persistingSubscription.Dispose();
    }
}