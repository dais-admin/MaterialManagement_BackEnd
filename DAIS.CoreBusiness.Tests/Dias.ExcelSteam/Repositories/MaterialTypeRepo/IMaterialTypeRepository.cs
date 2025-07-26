using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.MaterialTypeRepo
{
    public interface IMaterialTypeRepository
    {
        Task<MaterialType> GetAsync(MaterialTypeDto materialType, IDbTransaction transaction);
        Task<MaterialType> InsertAsync(MaterialTypeDto materialTypeDto, Guid projectId, IDbTransaction transaction);
    }
}
