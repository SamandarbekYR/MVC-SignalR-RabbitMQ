using MVCLearn.Models; 

namespace MVCLearn.DataAcess.Interfaces.UsersMessages
{
    public interface IUsersMessagesRepository : IBaseRepository<UserMessage>
    {
        public Task<List<UserMessage>> GetbyWorkerIDFullInfo(Guid WorkerId);
    }
}
