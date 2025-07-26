using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.ProjectRepo
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbConnection _connection;

        public ProjectRepository(IDbConnection connection)
        {
            _connection = connection;
        }   

        public async Task<Project> GetAsync(ProjectDto projectDto, IDbTransaction transaction)
        {
            var existingProject = await _connection.QueryFirstOrDefaultAsync<Project>(
            "SELECT * FROM Projects WHERE ProjectName = @ProjectName",
            new { projectDto.ProjectName },
            transaction);
            return existingProject!;
        }

        public async Task<Project> InsertAsync(ProjectDto projectDto, IDbTransaction transaction)
        {
            Project result = await GetAsync(projectDto, transaction);
            if (result is null)
            {
                var newProject = new Project
                {
                    Id = Guid.NewGuid(),  
                    ProjectName = projectDto.ProjectName,                    
                    CreatedDate = DateTime.UtcNow                   
                };
                var insertProjectQuery = "INSERT INTO Projects (Id, ProjectName, ProjectCode,CreatedDate,UpdatedDate,IsDeleted) VALUES (@Id, @ProjectName, @ProjectCode, @CreatedDate, @UpdatedDate,@IsDeleted)";
                await _connection.ExecuteAsync(insertProjectQuery, newProject, transaction);

                return newProject;
            }
            return result!;
        }
    }
}
