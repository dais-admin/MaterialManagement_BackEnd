using System.Linq.Expressions;

namespace DAIS.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        Task<T> FindAsync(int id);
        Task<IQueryable<T>> Query(string columnName, Guid id);
        Task<List<T>> GetMasterRecordCountsAsync(string storedProcedure);
    }
}
