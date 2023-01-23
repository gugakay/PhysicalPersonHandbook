
namespace DataAccess
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAllAsNoTrackingQueryable();
        IQueryable<T> GetAllQueryable();
        //Task<IEnumerable<T>> GetByCriteria(PersonFilterDto personFilter, CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        void Delete(T entity);
        void AddRange(List<T> entities);
        Task<T> GetByIdAsync(object id, CancellationToken cancellationToken);

        Task BulkDeleteAsync(List<T> entity);
    }
}
