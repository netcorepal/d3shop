using NetCorePal.D3Shop.Admin.Shared.Permission;

namespace NetCorePal.D3Shop.Web.Admin.Client.Components.Identity;

public sealed partial class PermissionManager : ComponentBase
{
    [Inject] private MessageService Message { get; set; } = default!;
    [Parameter] public List<string> AssignedPermissionCodes { get; set; } = [];
    [Parameter] public EventCallback<List<string>> AssignedPermissionCodesChanged { get; set; }
    public string[] DisabledPermissionCodes { get; set; } = [];

    private readonly Dictionary<string, Tree<Permission>> _treeDir = [];
    private readonly Dictionary<string, IEnumerable<string>> _treeKeysDir = [];
    private readonly Dictionary<string, Func<TreeEventArgs<Permission>, Task>> _treeOnChangeEventDir = [];
    private PermissionGroup[] PermissionGroups { get; set; } = [];
    private Guid Id { get; set; }

    protected override void OnInitialized()
    {
        PermissionGroups = PermissionDefinitionContext.PermissionGroups.ToArray();
        foreach (var group in PermissionGroups)
        {
            var treeId = group.Name;
            _treeDir.Add(treeId, default!);
            _treeKeysDir.Add(treeId, group.PermissionsWithChildren.Select(p => p.Code));
            _treeOnChangeEventDir.Add(treeId, async args =>
            {
                foreach (var key in _treeKeysDir[args.Tree.Id])
                {
                    AssignedPermissionCodes.Remove(key);
                }

                AssignedPermissionCodes.AddRange(args.Tree.CheckedKeys);

                await AssignedPermissionCodesChanged.InvokeAsync(AssignedPermissionCodes);
            });
        }
    }

    public void Update()
    {
        Id = Guid.NewGuid();
    }
}