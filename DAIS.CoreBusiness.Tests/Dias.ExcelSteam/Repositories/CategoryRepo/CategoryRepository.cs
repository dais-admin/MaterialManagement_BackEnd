
using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _connection;

        public CategoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Category> GetAsync(CategoryDto category, IDbTransaction transaction)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<Category>(
           "SELECT * FROM Categories WHERE CategoryName = @Name",
           new { category.Name},
           transaction);
            return result!;
        }

        public async Task<Category> InsertAsync(CategoryDto category, Guid projectId, IDbTransaction transaction)
        {
            var result = await GetAsync(category, transaction);
            if (result != null)
            {
                return result;
            }
            var cate = new Category
            {
               CategoryName = category.Name,
                ProjectId = projectId
            };
            var query = "INSERT INTO Categories (Id, CategoryName, CategoryCode, ProjectId,CreatedDate,UpdatedDate,IsDeleted) VALUES (@Id, @CategoryName, @CategoryCode, @ProjectId, @CreatedDate,@UpdatedDate,@IsDeleted)";
            await _connection.ExecuteAsync(query, cate, transaction);
            return cate;
        }
    }
}
