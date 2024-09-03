using MVCLearn.Models;

namespace MVCLearn.DataAcess.Interfaces.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> UpdateUserIsOnline(string gmail, bool isOnline);
        public Task<User?> GetById(Guid receiveId);
    }
}
