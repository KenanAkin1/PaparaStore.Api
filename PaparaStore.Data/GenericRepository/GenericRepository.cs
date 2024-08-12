using Microsoft.EntityFrameworkCore;
using PaparaStore.Base.Entity;
using PaparaStore.Data.Context;
using System.Linq.Expressions;

namespace PaparaStore.Data.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly PaparaStoreDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(PaparaStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public async Task Delete(long Id)
        {
            var entity = await dbSet.FindAsync(Id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public async Task<List<TEntity>> GetAll(params string[] includes)
        {
            var query = dbSet.AsQueryable();
            query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetById(long Id, params string[] includes)
        {
            var query = dbSet.AsQueryable();
            query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            return await query.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params string[] includes) // Bu metodu ekleyin
        {
            var query = dbSet.AsQueryable();
            query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task Insert(TEntity entity)
        {
            entity.InsertDate = DateTime.UtcNow;
            await dbSet.AddAsync(entity);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = dbSet.AsQueryable();
            query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            return await query.Where(expression).ToListAsync();
        }
    }
}
