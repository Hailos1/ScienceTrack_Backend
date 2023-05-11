namespace ScienceTrack.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        TEntity Create(TEntity item);
        void Delete(int id);
        int Count();
        TEntity Get(int id);
        Task<IEnumerable<TEntity>> GetList(int start, int end);
        Task Save();
        void Update(TEntity item);
    }
}
