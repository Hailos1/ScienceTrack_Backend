using ScienceTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace ScienceTrack.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        internal ScienceTrackContext context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(ScienceTrackContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public TEntity Create(TEntity item)
        {
            var newItem = dbSet.AddAsync(item).Result.Entity;
            return newItem;
        }

        public void Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public int Count()
            => dbSet.Count();

        public TEntity Get(int id)
        {
            TEntity entity = dbSet.Find(id);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetList()
            => await dbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> GetList(int start, int end)
        {
            var set = (await dbSet.ToListAsync()).Take(new Range(start, end));
            return set;
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }

        public void Update(TEntity item)
        {
            dbSet.Update(item);
        }
    }
}
