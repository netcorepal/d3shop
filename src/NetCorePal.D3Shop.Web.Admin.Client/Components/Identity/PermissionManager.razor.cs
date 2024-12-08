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
    private readonly Dictionary<string, EventCallback<TreeEventArgs<Permission>>> _treeOnChangeEventDir = [];
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
            _treeOnChangeEventDir.Add(treeId, EventCallback.Factory.Create<TreeEventArgs<Permission>>(this,
                async args =>
                {
                    foreach (var key in _treeKeysDir[args.Tree.Id])
                    {
                        AssignedPermissionCodes.Remove(key);
                    }

                    AssignedPermissionCodes.AddRange(args.Tree.CheckedKeys);

                    await AssignedPermissionCodesChanged.InvokeAsync(AssignedPermissionCodes);
                }));
        }
    }

    public void Update()
    {
        Id = Guid.NewGuid();
    }

    private RenderFragment BuildTree(PermissionGroup permissionGroup) => builder =>
    {
        var treeId = permissionGroup.Name;

        builder.OpenComponent<Tree<Permission>>(0); // 动态创建 Tree 组件
        builder.AddAttribute(2, "Id", treeId); // 设置树的 Id
        builder.AddAttribute(3, "Checkable", true); // 启用 Checkable 属性
        builder.AddAttribute(4, "DataSource", permissionGroup.Permissions); // 设置 DataSource 为 Permissions
        builder.AddAttribute(5, "TitleExpression",
            (Func<TreeNode<Permission>, string>)(x => x.DataItem.DisplayName)); // 设置显示名称
        builder.AddAttribute(6, "KeyExpression",
            (Func<TreeNode<Permission>, string>)(x => x.DataItem.Code)); // 设置 KeyExpression
        builder.AddAttribute(7, "ChildrenExpression",
            (Func<TreeNode<Permission>, IEnumerable<Permission>>)(x => x.DataItem.Children)); // 设置 ChildrenExpression
        builder.AddAttribute(8, "DefaultCheckedKeys",
            AssignedPermissionCodes.Intersect(_treeKeysDir[treeId]).ToArray()); // 设置默认选中的项
        builder.AddAttribute(9, "DisabledExpression",
            (Func<TreeNode<Permission>, bool>)(x =>
                DisabledPermissionCodes.Contains(x.DataItem.Code) || !x.DataItem.IsEnabled)); // 设置禁用规则
        builder.AddAttribute(10, "CheckStrictly", true); // 精确检查 treeNode；父树节点和子树节点没有关联
        builder.AddAttribute(11, "DefaultExpandAll", true); // 默认展开全部节点
        builder.AddAttribute(12, "OnCheck", _treeOnChangeEventDir[treeId]); // 设置 OnCheck 事件处理程序

        // 捕获组件的引用
        builder.AddComponentReferenceCapture(13, treeRef => { _treeDir[treeId] = (Tree<Permission>)treeRef; });

        builder.CloseComponent(); // 关闭 Tree 组件
    };
}