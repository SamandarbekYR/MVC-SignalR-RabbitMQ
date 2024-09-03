using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;

namespace LearnSignalR.Hubs
{
    public class MessageHub : Hub
    {
        private static readonly Dictionary<string, string> _connection = new Dictionary<string, string>();

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
            _connection.Remove(Context.ConnectionId, out _);
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
