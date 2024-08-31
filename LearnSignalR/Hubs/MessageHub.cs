using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace LearnSignalR.Hubs
{
    public class MessageHub : Hub
    {

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",message);
        }
        public async Task SendMessageToCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }
}
