using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<Category> GetAsync(CategoryDto category, IDbTransaction transaction);
        Task<Category> InsertAsync(CategoryDto category, Guid projectId, IDbTransaction transaction);

    }
}
