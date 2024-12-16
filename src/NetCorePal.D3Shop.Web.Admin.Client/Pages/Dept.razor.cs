using AntDesign.TableModels;

namespace NetCorePal.D3Shop.Web.Admin.Client.Pages;

public sealed partial class Dept
{
    [Inject] private IDepartmentService DepartmentService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;
    [Inject] private ConfirmService ConfirmService { get; set; } = default!;

    private PagedData<DepartmentResponse> _pagedDepartments = new(default!, default, default, default);

    private Table<DepartmentResponse> _table = default!;

    private readonly DepartmentQueryRequest _departmentQueryRequest = new() { CountTotal = true };

    private bool _loading;

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        _table.ReloadData(1, 10);
    }

    private async Task GetPagedDepartments()
    {
        var response = await DepartmentService.GetAllDepartments(_departmentQueryRequest);
        if (response.Success)
        {
            _pagedDepartments = response.Data;
            _departmentQueryRequest.PageIndex = response.Data.PageIndex;
            _departmentQueryRequest.PageSize = response.Data.PageSize;
        }
        else _ = Message.Error(response.Message);
    }

    private async Task HandleItemAdded()
    {
        await GetPagedDepartments();
    }

    private async Task Delete(DepartmentResponse row)
    {
        if (!await Confirm($"确认删除部门：{row.Name}?"))
            return;
        var response = await DepartmentService.DeleteDepartment(row.Id);
        if (response.Success)
        {
            _ = Message.Success("删除成功！");
            await GetPagedDepartments();
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
        _departmentQueryRequest.PageIndex = 1;
        await GetPagedDepartments();
    }

    private async Task Table_OnChange(QueryModel<DepartmentResponse> obj)
    {
        _loading = true;
        await GetPagedDepartments();
        _loading = false;
    }
}