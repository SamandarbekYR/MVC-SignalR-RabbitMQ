using MVCLearn.Models;

namespace MVCLearn.DataAcess.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity 
        : BaseEntity
    {
        public Task<Guid> Add(TEntity entity);
        public IQueryable<TEntity> GetAll();
    }
}
