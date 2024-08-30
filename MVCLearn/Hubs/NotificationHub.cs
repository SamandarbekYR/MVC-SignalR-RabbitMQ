using Microsoft.AspNetCore.SignalR;

namespace MVCLearn.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessageToUser(Guid userId, string message)
        {
            await Clients.Client(userId.ToString()).SendAsync("ReceiveMessage", message);
        }
    }
}
