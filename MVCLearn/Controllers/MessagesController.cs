using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.DataAcess.Interfaces.UsersMessages;
using MVCLearn.DTOs;
using MVCLearn.Hubs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Models;

namespace MVCLearn.Controllers
{
    public class MessagesController : Controller
    {
        private IUserRepository _usersrepository;
        private IUsersMessagesRepository _usersMessage;
        private IMessageRepository _message;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IEmailSenderService _emailSender;

        public MessagesController(IUserRepository userRepository,
                                  IUsersMessagesRepository usersMessages,
                                  IMessageRepository message,
                                  IHubContext<NotificationHub> hubContext,
                                  IEmailSenderService emailSender)
        {
            _usersrepository = userRepository;
            _usersMessage = usersMessages;
            _message = message;
            _hubContext = hubContext;
            _emailSender = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> Boss()
        {
            var users = _usersrepository.GetAll().Where(u => u.RoleName == "Worker").ToList();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDto sendMessage)
        {
            try
            {
                var message = new Message { Content = sendMessage.messageBody, SendAt = DateTime.UtcNow.AddHours(5) };
                Guid messageId = await _message.Add(message);

                foreach (var userId in sendMessage.users)
                {
                    var userMessage = new UserMessage { UserId = userId.Id, MessageId = messageId };
                    await _usersMessage.Add(userMessage);

                    // Faqat kerakli foydalanuvchiga yuborish
                    await _hubContext.Clients.Client(userId.Id.ToString())
                        .SendAsync("ReceiveMessage", sendMessage.messageBody, DateTime.UtcNow.AddHours(5));

                    MessageDto messageDto = new();
                    messageDto.Title = "Boss's message\n";
                    messageDto.Body = sendMessage.messageBody;
                    messageDto.To = userId.Email;
                    await _emailSender.SendEmailAsync(messageDto);
                }

                return RedirectToAction("Boss");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Worker()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login","Accaunt"); 
            }
            
            var messages = await _usersMessage.GetbyWorkerIDFullInfo(Guid.Parse(userId!));

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForWorker()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var messages = await _usersMessage.GetbyWorkerIDFullInfo(Guid.Parse(userId!));

            return Json(messages);
        }
    }
}
