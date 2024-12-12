using NetCorePal.D3Shop.Admin.Shared.Permission;

namespace NetCorePal.D3Shop.Web.Admin.Client.Components.Identity;

public sealed partial class PermissionManager : ComponentBase
{
    [Inject] private MessageService Message { get; set; } = default!;
    [Parameter] public List<string> AssignedPermissionCodes { get; set; } = [];
    [Parameter] public EventCallback<List<string>> AssignedPermissionCodesChanged { get; set; }
    public string[] DisabledPermissionCodes { get; set; } = [];

    private readonly Dictionary<string, Tree<Permission>> _treeDir = new();
    private readonly Dictionary<string, IEnumerable<string>> _treeKeysDir = new();
    private PermissionGroup[] PermissionGroups { get; set; } = [];
    private Guid TreeComponentKey { get; set; }

    protected override void OnInitialized()
    {
        PermissionGroups = PermissionDefinitionContext.PermissionGroups.ToArray();
        foreach (var group in PermissionGroups)
        {
            var groupName = group.Name;
            _treeDir[groupName] = default!;
            _treeKeysDir[groupName] = group.PermissionsWithChildren.Select(p => p.Code);
        }
    }

    private async Task OnTreeChange(TreeEventArgs<Permission> args, string groupName)
    {
        foreach (var key in _treeKeysDir[groupName])
        {
            AssignedPermissionCodes.Remove(key);
        }
        AssignedPermissionCodes.AddRange(args.Tree.CheckedKeys);

        await AssignedPermissionCodesChanged.InvokeAsync(AssignedPermissionCodes);
    }

    private async Task HandleCheckAll(bool isChecked, string? groupName = null)
    {
        if (groupName == null)
        {
            foreach (var tree in _treeDir.Values)
            {
                if (isChecked)
                    tree.CheckAll();
                else
                    tree.UncheckAll();
            }

            AssignedPermissionCodes.Clear();
            if (isChecked)
            {
                AssignedPermissionCodes.AddRange(_treeKeysDir.Values.SelectMany(k => k));
            }
        }
        else
        {
            if (_treeDir.TryGetValue(groupName, out var tree))
            {
                if (isChecked)
                    tree.CheckAll();
                else
                    tree.UncheckAll();
            }

            AssignedPermissionCodes.RemoveAll(code => _treeKeysDir[groupName].Contains(code));
            if (isChecked)
            {
                AssignedPermissionCodes.AddRange(_treeKeysDir[groupName]);
            }
        }

        await AssignedPermissionCodesChanged.InvokeAsync(AssignedPermissionCodes);
    }

    private Task OnCheckAll() => HandleCheckAll(true);

    private Task OnUncheckAll() => HandleCheckAll(false);

    private Task OnCheckGroupAll(string groupName) => HandleCheckAll(true, groupName);

    private Task OnUncheckGroupAll(string groupName) => HandleCheckAll(false, groupName);

    public void ReRenderTree()
    {
        TreeComponentKey = Guid.NewGuid();
    }
}
