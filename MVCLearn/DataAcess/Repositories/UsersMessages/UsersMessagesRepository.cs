using Microsoft.EntityFrameworkCore;
using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.UsersMessages;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories.UsersMessages
{
    public class UsersMessagesRepository : BaseRepository<UserMessage>, IUsersMessagesRepository
    {
        private DbSet<UserMessage> _usersMessages;
        public UsersMessagesRepository(AppDbContext appDb) : base(appDb)
        {
            _usersMessages = appDb.Set<UserMessage>();
        }

        public async Task<List<UserMessage>> GetbyWorkerIDFullInfo(Guid WorkerID)
        {
            return await _usersMessages.Include(u => u.Message)
                                       .Include(u => u.User)
                                       .Where(u => u.UserId == WorkerID).ToListAsync();
        }
    }
}
