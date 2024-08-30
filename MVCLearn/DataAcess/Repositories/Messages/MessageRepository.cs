using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories.Messages
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext appDb) : base(appDb)
        { }
    }
}
