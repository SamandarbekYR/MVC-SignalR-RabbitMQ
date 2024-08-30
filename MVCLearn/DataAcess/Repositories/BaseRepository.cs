using Microsoft.EntityFrameworkCore;
using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        public AppDbContext _dbContext;
        public DbSet<TEntity> _dbSet;
        public BaseRepository(AppDbContext appDb)
        {
            _dbContext = appDb;
            _dbSet = appDb.Set<TEntity>();
        }
        public async Task<Guid> Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            int result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                throw new Exception("Xatolik yuz berdi");

            return entity.Id;
        }
        public IQueryable<TEntity> GetAll()
        => _dbSet.AsQueryable();
    }
}
