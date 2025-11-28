using DAIS.DataAccess.Data;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Net;

namespace DAIS.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly string curentUserName;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            if (_context._httpContextAccessor.HttpContext is not null)
            {
                if ((bool)(_context._httpContextAccessor?.HttpContext.User.Identity.IsAuthenticated))
                {
                    curentUserName = _context._httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                }
                else
                {
                    curentUserName = "admin";
                }
            }

        }

        public async Task<T> GetById(Guid id) => await _context.Set<T>().FindAsync(id);

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
            => _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<T> Add(T entity)
        {
            entity.GetType().GetProperty("CreatedBy").SetValue(entity, curentUserName);
            var dbEntity = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return dbEntity.Entity;
        }

        public async Task Update(T entity)
        {
            // In case AsNoTracking is used
            entity.GetType().GetProperty("UpdatedBy").SetValue(entity, curentUserName);
            var dbEntity = _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return _context.Set<T>().AsNoTracking().AsQueryable();
        }
        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => _dbSet.CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => _dbSet.CountAsync(predicate);

        public IQueryable<T> Query()
        {
            return _dbSet;
        }

        public async Task<IQueryable<T>> Query(string columnName, Guid id)
        {
            var propertyInfo = typeof(T).GetProperty(columnName) ?? throw new ArgumentException($"Property '{columnName}' does not exist on type '{typeof(T).Name}'.");

            // Ensure the property type matches the expected type
            if (propertyInfo.PropertyType != typeof(string))
            {
                throw new ArgumentException($"Property '{columnName}' must be of type 'string'.");
            }

            // Use expression trees to build the query
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var idValue = Expression.Constant(id);
            var equality = Expression.Equal(property, idValue);

            var predicate = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return (IQueryable<T>)await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetMasterRecordCountsAsync(string storedProcedure)
        {
            var result = await _context.Set<T>()
                .FromSqlRaw("EXEC "+storedProcedure)
                .ToListAsync();

            return result;
        }
    }
}
