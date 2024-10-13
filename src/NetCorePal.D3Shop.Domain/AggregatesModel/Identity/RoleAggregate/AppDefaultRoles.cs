using System.Collections.ObjectModel;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public static class AppDefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string Basic = nameof(Basic);

        public static IReadOnlyList<string> DefaultRoles { get; }
            = new ReadOnlyCollection<string>(
            [
                Admin,
                Basic
            ]);

        public static bool IsDefault(string roleName)
            => DefaultRoles.Any(r => r == roleName);
    }
}
