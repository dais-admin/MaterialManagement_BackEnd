using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.ProjectRepo
{
    public interface IProjectRepository
    {
        Task<Project> GetAsync(ProjectDto projectDto, IDbTransaction transaction);
        Task<Project> InsertAsync(ProjectDto projectDto, IDbTransaction transaction);
    }
}
