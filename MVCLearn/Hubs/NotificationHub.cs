using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MVCLearn.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connection = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userGmail = httpContext!.Session.GetString("UserGmail");
            if (!string.IsNullOrEmpty(userGmail))
            {
                _connection.TryAdd(Context.ConnectionId, userGmail);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connection.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageToSelectedUsers(string[] userEmails, string message)
        {
            foreach (var email in userEmails)
            {
                var connectionId = _connection.FirstOrDefault(x => x.Value == email).Key;
                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
            }
        }
    }
}
