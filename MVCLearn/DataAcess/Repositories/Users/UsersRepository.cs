using Microsoft.EntityFrameworkCore;
using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories.Users
{
    public class UsersRepository : BaseRepository<User>, IUserRepository
    {
        private AppDbContext _appDbContext;
        private DbSet<User> _users;
        public UsersRepository(AppDbContext appDb) : base(appDb)
        {
            _appDbContext = appDb;
            _users = appDb.Set<User>();
        }

        public async Task<User?> GetById(Guid receiveId)
        => await _users.FirstOrDefaultAsync(x => x.Id == receiveId);

        public async Task<User?> UpdateUserIsOnline(string gmail, bool isOnline)
        {
            if (string.IsNullOrEmpty(gmail))
                return null; 

            var user = await _users.FirstOrDefaultAsync(g => g.Gmail == gmail);

            if (user == null)
                return null; 
            
            if (user.IsOnline != isOnline) 
            {
                user.IsOnline = isOnline;
                await _appDbContext.SaveChangesAsync();
            }

            return user;
        }
    }
}
