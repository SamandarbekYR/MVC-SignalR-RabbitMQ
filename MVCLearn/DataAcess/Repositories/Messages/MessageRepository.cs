using Microsoft.EntityFrameworkCore;
using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories.Messages
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private DbSet<Message> _messages;
        private AppDbContext _appDbContext;
        public MessageRepository(AppDbContext appDb) : base(appDb)
        {
            _messages = appDb.Set<Message>();
            _appDbContext = appDb;
        }

        public async Task<List<Message>> GetByReceiverIdAllMessagesAsync(Guid receiverId)
        => await _messages.Where(receiver => receiver.Id == receiverId).ToListAsync();

        public async Task<List<Message>> GetBySenderIdAllMessagesAsync(Guid senderId)
        => await _messages.Where(sender => sender.SenderId == senderId).ToListAsync();
        
        public async Task<Message?> UpdateMessageStatus(Guid messageId, bool IsRead)
        {
            var message = await _messages.FirstOrDefaultAsync(i => i.Id == messageId);
            
            if (message is not null)
            {
                message.IsRead = IsRead;
                message.ReadTime = DateTime.UtcNow.AddHours(5);
                await _appDbContext.SaveChangesAsync();

                return message;
            }

            return null;
        }
    }
}
