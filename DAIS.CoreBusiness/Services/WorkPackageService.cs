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
    public class WorkPackageService : IWorkPackageService
    {
        private IGenericRepository<WorkPackage> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkPackageService> _logger;
        public WorkPackageService(IGenericRepository<WorkPackage> genericRepo, IMapper mapper,
            ILogger<WorkPackageService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<WorkPackageDto> AddWorkPackage(WorkPackageDto workPackageDto)
        {
            _logger.LogInformation("WorkPackageService:AddWorkPackage:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(workPackageDto.WorkPackageName))
                {
                    workPackageDto.WorkPackageName = workPackageDto.WorkPackageName.ToUpper();
                }
                var workPackage = _mapper.Map<WorkPackage>(workPackageDto);
                await _genericRepo.Add(workPackage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:AddWorkPackage:Method End");
            return workPackageDto;
        }

        public async Task DeleteWorkPakage(Guid id)
        {
            _logger.LogInformation("WorkPackageService:DeleteWorkPakage:Method Start");
            try
            {
                var workPackage = await _genericRepo.GetById(id);
                await _genericRepo.Remove(workPackage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:DeleteWorkPakage:Method End");
            
        }

        public async Task<List<WorkPackageDto>> GetAllWorkPackages()
        {
            _logger.LogInformation("WorkPackageService:GetAllWorkPackages:Method Start");
            List<WorkPackageDto> workPackageDtoList = new List<WorkPackageDto>();
            try
            {
                var workPackages = await _genericRepo.Query()
                    .Include(x => x.Project)               
                    .ToListAsync().ConfigureAwait(false);
                workPackageDtoList.AddRange(_mapper.Map<List<WorkPackageDto>>(workPackages));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:GetAllWorkPackages:Method End");
            return workPackageDtoList;
        }
        public async Task<List<WorkPackageDto>> GetWorkPackagesByProjectId(Guid projectId)
        {
            _logger.LogInformation("WorkPackageService:GetWorkPackagesByProjectId:Method Start");
            List<WorkPackageDto> workPackageDtoList = new List<WorkPackageDto>();
            try
            {
                var workPackages = await _genericRepo.Query()
                    .Where(x => x.ProjectId == projectId)
                    .Include(x => x.Project)
                    .ToListAsync().ConfigureAwait(false);
                workPackageDtoList.AddRange(_mapper.Map<List<WorkPackageDto>>(workPackages));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:GetWorkPackagesByProjectId:Method End");
            return workPackageDtoList;
        }
        public async Task<List<WorkPackageDto>> GetWorkPackagesByProjectIdAndSystem(Guid projectId,string system)
        {
            _logger.LogInformation("WorkPackageService:GetWorkPackagesByProjectIdAndSystem:Method Start");
            List<WorkPackageDto> workPackageDtoList = new List<WorkPackageDto>();
            try
            {
                var workPackages = await _genericRepo.Query()
                    .Where(x => x.ProjectId == projectId && x.System==system)
                    .Include(x => x.Project)
                    .ToListAsync().ConfigureAwait(false);
                workPackageDtoList.AddRange(_mapper.Map<List<WorkPackageDto>>(workPackages));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:GetWorkPackagesByProjectIdAndSystem:Method End");
            return workPackageDtoList;
        }
        public async Task<WorkPackageDto> GetWorkPackage(Guid id)
        {
            _logger.LogInformation("WorkPackageService:GetWorkPackage:Method Start");
            WorkPackageDto workPackageDto = new WorkPackageDto();
            try
            {
                var workPackage = await _genericRepo.Query()
                    .Where(x => x.Id == id)
                    .Include(x => x.Project)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                workPackageDto = _mapper.Map<WorkPackageDto>(workPackage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:GetWorkPackage:Method End");
            return workPackageDto;
        }

        public async Task<WorkPackageDto> UpdateWorkPackage(WorkPackageDto workPackageDto)
        {
            _logger.LogInformation("WorkPackageService:UpdateWorkPackage:Method Start");
            
            try
            {              
                var workPackage = _mapper.Map<WorkPackage>(workPackageDto);
                await _genericRepo.Update(workPackage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:UpdateWorkPackage:Method End");
            return workPackageDto;
        }

        public WorkPackageDto GetWorkPackageIdByName(string name)
        {
            _logger.LogInformation("WorkPackageService:GetWorkPackageIdByName:Method Start");
            WorkPackageDto workPackageDto = new WorkPackageDto();
            try
            {
                var workPackage = _genericRepo.Query()                  
                    .FirstOrDefault(x=>x.WorkPackageName.ToLower()==name.ToLower());
                workPackageDto = _mapper.Map<WorkPackageDto>(workPackage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("WorkPackageService:GetWorkPackageIdByName:Method End");
            return workPackageDto;
        }
    }
}
