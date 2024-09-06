using Microsoft.AspNetCore.Mvc;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.DTOs;
using MVCLearn.Interfaces.RabbitMQ;
using MVCLearn.Models;
using Newtonsoft.Json;

namespace MVCLearn.Controllers
{
    public class MessagesController : Controller
    {
        private IUserRepository _usersrepository;
        private readonly IRabbitMQProducerService _rabbitMQProducer;
        private readonly IMessageRepository _message;

        public MessagesController(IUserRepository userRepository,
                                  IRabbitMQProducerService rabbitMQProducer,
                                  IMessageRepository message)
        {
            _usersrepository = userRepository;
            _rabbitMQProducer = rabbitMQProducer;
            _message = message;
        }
        [HttpGet]
        public IActionResult Boss()
        {
            var bossId = HttpContext.Session.GetString("BossId");

            if (bossId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var users = _usersrepository.GetAll().Where(u => u.RoleName == "Worker").ToList();
            return View(users);
        }
        [HttpPost]
        public IActionResult SendMessage([FromBody] SendMessageRequestDto sendMessage)
        {
            try
            {
                var bossId = HttpContext.Session.GetString("BossId");

                if (bossId == null)
                {
                    return RedirectToAction("Login", "Accaunt");
                }

                sendMessage.SenderId = Guid.Parse(bossId);
                string jsonMessage = JsonConvert.SerializeObject(sendMessage);
                _rabbitMQProducer.SendMessage(jsonMessage);

                return RedirectToAction("Boss", "Messages");
            }
            catch
            {
                return RedirectToAction("Boss");
            }
        }
        [HttpGet]
        public IActionResult Worker()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var messages = _message.GetAll().Where(i => i.ReceiverId == Guid.Parse(userId)).ToList();

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

            var messages = await _message.GetByReceiverIdAllMessagesAsync(Guid.Parse(userId!));

            return Json(messages); 
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var bossId = HttpContext.Session.GetString("BossId");

            if (bossId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var messages = await _message.GetBySenderIdAllMessagesAsync(Guid.Parse(bossId!));
            List<MessageView> messageViews = new List<MessageView>();

            foreach (var message in messages)
            {
                var receiverInfo = await _usersrepository.GetById(message.ReceiverId);
                
                if (receiverInfo is not null)
                {
                    MessageView messageView = new MessageView();
                    messageView.ReceiveName = receiverInfo.FirstName;
                    messageView.MessageContent = message.MessageContent;
                    messageView.SendTime = message.SendTime;
                    messageView.ReadTime = message.ReadTime;
                    messageView.ReadStatus = message.IsRead;
                    messageViews.Add(messageView);
                }
            }
            return Json(messages);
        }
    }
}
