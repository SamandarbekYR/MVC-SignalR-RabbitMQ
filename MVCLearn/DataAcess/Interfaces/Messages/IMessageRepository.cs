using MVCLearn.Models;

namespace MVCLearn.DataAcess.Interfaces.Messages
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<Message?> UpdateMessageStatus(Guid messageId, bool IsRead);
        Task<List<Message>> GetBySenderIdAllMessagesAsync(Guid senderId);
        Task<List<Message>> GetByReceiverIdAllMessagesAsync(Guid receiverId);
    }  
}
