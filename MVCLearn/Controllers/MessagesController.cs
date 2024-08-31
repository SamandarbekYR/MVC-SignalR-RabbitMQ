using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.DataAcess.Interfaces.UsersMessages;
using MVCLearn.DTOs;
using MVCLearn.Hubs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Interfaces.RabbitMQ;
using MVCLearn.Models;
using Newtonsoft.Json;

namespace MVCLearn.Controllers
{
    public class MessagesController : Controller
    {
        private IUserRepository _usersrepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IRabbitMQProducerService _rabbitMQProducer;
        private readonly IUsersMessagesRepository _usersMessage;

        public MessagesController(IUserRepository userRepository,
                                  IHubContext<NotificationHub> hubContext,
                                  IRabbitMQProducerService rabbitMQProducer,IUsersMessagesRepository userMessage)
        {
            _usersrepository = userRepository;
            _hubContext = hubContext;
            _rabbitMQProducer = rabbitMQProducer;
            _usersMessage = userMessage;
        }
        [HttpGet]
        public async Task<IActionResult> Boss()
        {
            //var userId = HttpContext.Session.GetString("UserId");

            //if (userId == null)
            //{
            //    return RedirectToAction("Login", "Accaunt");
            //}

            var users = _usersrepository.GetAll().Where(u => u.RoleName == "Worker").ToList();
            return View(users);
        }
        [HttpPost]
        public  IActionResult SendMessage([FromBody] SendMessageRequestDto sendMessage)
        {
            try
            {
                string jsonMessage = JsonConvert.SerializeObject(sendMessage);
                _rabbitMQProducer.SendMessage(jsonMessage);

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
