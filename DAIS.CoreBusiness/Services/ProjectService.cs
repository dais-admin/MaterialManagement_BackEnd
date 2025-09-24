using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class ProjectService : IProjectService
    {
        private IGenericRepository<Project> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectService> _logger;
        
        public ProjectService(IGenericRepository<Project> genericRepo, IMapper mapper, ILogger<ProjectService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            
        }
        public async Task<ProjectDto> AddProject(ProjectDto projectDto)
        {
            _logger.LogInformation("ProjectServices:AddProject:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(projectDto.ProjectName))
                {
                    projectDto.ProjectName = projectDto.ProjectName.ToUpper();
                }
                var project = _mapper.Map<Project>(projectDto);
                await _genericRepo.Add(project);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("ProjectService:AddProject:Method End");
            return projectDto;
        }

        public async Task DeleteProject(Guid id)
        {
            _logger.LogInformation("ProjectService:DeleteProject:Method Start");
            try
            {
                var project = await _genericRepo.GetById(id);
                await _genericRepo.Remove(project);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ProjectService:DeleteProject:Method End");
        }

        public async Task <List<ProjectDto>> GetAllProjects()
        {
            _logger.LogInformation("ProjectService:GetAllProjects:Method Start");
            List<ProjectDto> projectDtoList = new List<ProjectDto>();
            try
            {
                var projectList = await _genericRepo.Query().
                Include(x => x.Agency).ToListAsync().ConfigureAwait(false);
                projectDtoList.AddRange(_mapper.Map<List<ProjectDto>>(projectList));

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ProjectService:GetAllProjects:Method End");
            return projectDtoList;

        }

        public async Task<ProjectDto> GetProject(Guid id)
        {
            _logger.LogInformation("ProjectService:GetProject:Method Start");
            ProjectDto projectDto = new ProjectDto();
            try
            {
                var project = await _genericRepo.Query()
            .Include(x => x.Agency).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                projectDto = _mapper.Map<ProjectDto>(project);

              
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ProjectService:GetProject:Method End");
            return projectDto;
           
            
        }

        public async Task<ProjectDto> UpdateProject(ProjectDto projectDto)
        {
            _logger.LogInformation("ProjectService:UpdateProject:Method Start");
            try
            {
                
                 var project =_mapper.Map<Project>(projectDto);
                 await _genericRepo.Update(project);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ProjectService:UpdateProject:Method End");
            return projectDto ;
        }

        public async Task<ProjectDto> GetProjectIdByName(string name)
        {
            _logger.LogInformation("ProjectService:GetProjectIdByName:Method Start");
            ProjectDto projectDto = new ProjectDto();
            try
            {
                var project = await _genericRepo.Query()
                    .FirstOrDefaultAsync(x=>x.ProjectName.ToLower() == name.ToLower()).ConfigureAwait(false);
                projectDto = _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ProjectService:GetProjectIdByName:Method End");
            return projectDto;
        }
        //public async Task<List<MasterRecordCountDto>> GetRecordCountsAsync()
        //{
        //    var result = await _genericRepo.GetMasterRecordCountsAsync("");
                
        //    return result;
        //}
    }
}
