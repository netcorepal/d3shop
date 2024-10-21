namespace NetCorePal.D3Shop.Web
{
    public class AppConfiguration
    {
        public string Secret { get; set; } = string.Empty;
        public int TokenExpiryInMinutes { get; set; }
    }
}
