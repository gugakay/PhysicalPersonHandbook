using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DefaultDbContext _dbContext;

        public Repository(DefaultDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        } 

        public virtual IQueryable<T> GetAllAsNoTrackingQueryable() =>
            _dbSet.AsNoTracking();

        public virtual IQueryable<T> GetAllQueryable() =>
            _dbSet.AsQueryable();

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual void AddRange(List<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbSet.Update(entity);
            var propertyValues = await entry.GetDatabaseValuesAsync(cancellationToken);

            if (propertyValues == null)
            {
                throw new Exception("Entity not found");
            }
            else
            {
                entry.OriginalValues.SetValues(propertyValues);
            }

            return entity;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            
            _dbSet.Remove(entity);
        }

        public virtual async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task BulkDeleteAsync(List<T> entities)
        {
            await _dbContext.BulkDeleteAsync(entities);
        }
    }
}
