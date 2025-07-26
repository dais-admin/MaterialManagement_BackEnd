namespace DAIS.DataAccess.Interfaces
{
    public interface IBulkUploadRepository<T> where T : class
    {
        T Add(T entity);
        IQueryable<T> Query();
    }
}
