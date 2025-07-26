

using DAIS.DataAccess.Data;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAIS.DataAccess.Repositories
{
    public class BulkUploadRepository<T> : IBulkUploadRepository<T> where T : class
    {
        protected AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly string curentUserName;
        public BulkUploadRepository(AppDbContext context)
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

        public T Add(T entity)
        {
            entity.GetType().GetProperty("CreatedBy").SetValue(entity, curentUserName);
            var dbEntity =  _dbSet.Add(entity);
            _context.SaveChanges();
            return dbEntity.Entity;
        }
        public IQueryable<T> Query()
        {
            return _dbSet;
        }
    }
}
