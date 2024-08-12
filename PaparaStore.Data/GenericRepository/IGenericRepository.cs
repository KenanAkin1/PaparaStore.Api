using System.Linq.Expressions;

namespace PaparaStore.Data.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll(params string[] includes);
        Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params string[] includes); // Bu satırı ekleyin
        Task<TEntity> GetById(long Id, params string[] includes);
        Task Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Delete(long Id);
        Task Save();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
