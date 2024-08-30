using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories.Users
{
    public class UsersRepository : BaseRepository<User>, IUserRepository
    {
        public UsersRepository(AppDbContext appDb) : base(appDb)
        { }
    }
}
