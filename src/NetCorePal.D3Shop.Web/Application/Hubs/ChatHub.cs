namespace NetCorePal.D3Shop.Web.Application.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }

    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}
