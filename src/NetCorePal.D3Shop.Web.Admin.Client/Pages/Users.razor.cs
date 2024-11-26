namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Users : IDisposable
{
    [Inject] private IAdminUserService AdminUserService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;
    [Inject] private PersistentComponentState ApplicationState { get; set; } = default!;

    private PersistingComponentStateSubscription _persistingSubscription;

    private List<AdminUserResponse> _adminUserList = [];

    protected override async Task OnInitializedAsync()
    {
        const string persistKey = "adminUsers";
        _persistingSubscription = ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson(persistKey, _adminUserList);
            return Task.CompletedTask;
        });

        if (ApplicationState.TryTakeFromJson<List<AdminUserResponse>>(persistKey, out var restored))
            _adminUserList = restored!;
        else
            _adminUserList = await GetAllAdminUsers();
    }

    private async Task<List<AdminUserResponse>> GetAllAdminUsers(string? name = null, string? phone = null)
    {
        var queryRequest = new AdminUserQueryRequest(name, phone);
        var response = await AdminUserService.GetAllAdminUsers(queryRequest);
        if (response.Success) return response.Data.ToList();
        _ = Message.Error(response.Message);
        return [];
    }

    private async Task HandleItemAdded()
    {
        _adminUserList = await GetAllAdminUsers();
    }

    private async Task Delete(AdminUserResponse row)
    {
        if (!await Confirm($"确认删除用户：{row.Name}?"))
            return;
        var response = await AdminUserService.DeleteAdminUser(row.Id);
        if (response.Success)
        {
            _ = Message.Success("删除成功！");
            _adminUserList = await GetAllAdminUsers();
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
        _adminUserList = await GetAllAdminUsers(_searchString);
    }

    public void Dispose()
    {
        _persistingSubscription.Dispose();
    }
}