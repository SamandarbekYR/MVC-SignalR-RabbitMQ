using Microsoft.AspNetCore.SignalR;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.Models;
using System.Collections.Concurrent;

namespace MVCLearn.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connection = new ConcurrentDictionary<string, string>();
        private IUserRepository _userRepository;
        private IMessageRepository _message;

        public NotificationHub(IUserRepository userRepository, 
                               IMessageRepository message)
        {
            _userRepository = userRepository;
            _message = message;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userGmail = httpContext!.Session.GetString("UserGmail");
            if (!string.IsNullOrEmpty(userGmail))
            {
                _connection.TryAdd(Context.ConnectionId, userGmail);
                var user = await _userRepository.UpdateUserIsOnline(userGmail, true);
                if (user is not null)
                {
                    await Clients.All.SendAsync("UpdateUserStatus", user.Gmail, user.IsOnline);
                }
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connection.TryRemove(Context.ConnectionId, out var userGmail))
            {
                // Offlayn holatini yangilash
                var user = await _userRepository.UpdateUserIsOnline(userGmail, false);
                if (user != null)
                {
                    await Clients.All.SendAsync("UpdateUserStatus", user.Gmail, user.IsOnline);
                }
            }

            await base.OnDisconnectedAsync(exception);
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
        public async Task UpdateMessageStatus(Guid messageId, bool isRead)
        {
            var message = await _message.UpdateMessageStatus(messageId, isRead);
            if(message != null)
            {
                var user = await _userRepository.GetById(message.ReceiverId);
                MessageView messageView = new MessageView();
                messageView.ReceiveName = user!.FirstName;
                messageView.MessageContent = message.MessageContent;
                messageView.SendTime = message.SendTime;
                messageView.ReadTime = message.ReadTime;
                messageView.ReadStatus = message.IsRead;

                await Clients.All.SendAsync("MessageStatusUpdated", messageView);
            }
        }
    }
}
