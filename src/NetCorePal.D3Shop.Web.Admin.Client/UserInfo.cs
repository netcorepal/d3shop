namespace NetCorePal.D3Shop.Web.Admin.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required IEnumerable<string> Roles { get; set; }

        public required IEnumerable<string> Permissions { get; set; }
        public required string AccessToken { get; set; }
    }
}