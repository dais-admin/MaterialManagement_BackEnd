
using AutoMapper;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace DAIS.CoreBusiness.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly MaterialServiceDependencies _materialServiceDependencies;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private Guid projectId = Guid.Empty;
        private string roleName = string.Empty;
        public MaterialService(MaterialServiceDependencies materialServiceDependencies, MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _materialServiceDependencies = materialServiceDependencies;
            _materialServiceInfrastructure = materialServiceInfrastructure;

            if (_materialServiceInfrastructure.HttpContextAccessor.HttpContext != null)
            {
                SetUserAndProject(_materialServiceInfrastructure.HttpContextAccessor.HttpContext.User);
            }

        }

        private void SetUserAndProject(ClaimsPrincipal user)
        {
            try
            {
                if (user != null)
                {
                    userName = user.Claims.FirstOrDefault(x => x.Type == Claims.NameClaim).Value;

                    if (user.Claims.FirstOrDefault(x => x.Type == "ProjectId").Value != "")
                    {
                        projectId = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "ProjectId").Value);
                    }

                    roleName = user.Claims.FirstOrDefault(x => x.Type == Claims.RoleClaim).Value;
                }
            }
            catch(Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<MaterialDto> AddMaterialAsync(MaterialDto materialDto)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:AddMaterialAsync:Method Start");
            try
            {
                var material = _materialServiceInfrastructure.Mapper.Map<Material>(materialDto);
                if (materialDto.YearOfInstallation != null)
                {
                    material.MaterialStatus = MaterialStatus.InUse;
                }
                
                var dbEntity = await _materialServiceInfrastructure.GenericRepository.Add(material).ConfigureAwait(false);
                materialDto.Id = dbEntity.Id;

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:AddMaterialAsync:Method End");
            return materialDto;
        }

        public async Task DeleteMaterialAsync(Guid id)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:DeleteMaterialAsync:Method Start");
            try
            {
                var materialToRemove = await _materialServiceInfrastructure.GenericRepository.GetById(id).ConfigureAwait(false); 
              await  _materialServiceInfrastructure.GenericRepository.Remove(materialToRemove).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;

            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:DeleteMaterialAsync:Method End");
        }
        public async Task DeleteBulkUploadMaterials(Guid bulkUploadDetailId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:DeleteBulkUploadMaterials:Method Start");
            try
            {
                var materialsToRemove = await _materialServiceInfrastructure.GenericRepository.
                    Query().Where(x=>x.BuilkUploadDetailId == bulkUploadDetailId).
                    ToListAsync().ConfigureAwait(false);
                foreach (var material in materialsToRemove)
                {
                    await _materialServiceInfrastructure.GenericRepository
                    .Remove(material).ConfigureAwait(false);
                }
                
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;

            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:DeleteBulkUploadMaterials:Method End");
        }
        public async Task<MaterialMasterListDto> GetAllMaterialMasters()
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialMaster:Method Start");
            MaterialMasterListDto materialMasterListDto = new MaterialMasterListDto();
            try
            {
                materialMasterListDto.MaterialTypeList = await _materialServiceDependencies.MaterialTypeService.GetAllMaterialTypes().ConfigureAwait(false);
                materialMasterListDto.CategoryList = await _materialServiceDependencies.CategoryService.GetAllCategory().ConfigureAwait(false);
                materialMasterListDto.RegionList = await _materialServiceDependencies.RegionService.GetAllRegions().ConfigureAwait(false);
                materialMasterListDto.ManufacturerList = await _materialServiceDependencies.ManufacturerService.GetAllManufacturer().ConfigureAwait(false);
                materialMasterListDto.SuppliersList = await _materialServiceDependencies.SupplierService.GetAllSupplier().ConfigureAwait(false);
                materialMasterListDto.DivisionList = await _materialServiceDependencies.DivisionService.GetAllDivision().ConfigureAwait(false);
                materialMasterListDto.LocationOperationList = await _materialServiceDependencies.LocationOperationService.GetAllLocationOperation().ConfigureAwait(false);
                materialMasterListDto.ProjectsList = await _materialServiceDependencies.ProjectService.GetAllProjects().ConfigureAwait(false);
                materialMasterListDto.MaterialMeasuremetList = await _materialServiceDependencies.MaterialMeasurementService.GetAllMaterialMeasurement().ConfigureAwait(false);
                materialMasterListDto.ContractorList = await _materialServiceDependencies.ContractorService.GetAllContractor().ConfigureAwait(false);
                materialMasterListDto.MaterialCode = GenerateMaterialCode();
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;

            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialMaster:Method End");
            return materialMasterListDto;
        }

        private string GenerateMaterialCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, 10).Select(x => chars[random.Next(chars.Length)]);
            return string.Join("", list);
        }

        public async Task<List<MaterialDto>> GetAllMaterialsAsync(Guid workPackageId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x=>x.WorkPackageId== workPackageId)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .ThenInclude(x=>x.SubDivision)
                    .ThenInclude(x=>x.Division)
                    .Include(x => x.Supplier)  
                    .Include(x=>x.WorkPackage)
                    .Include(x=>x.Measurement)
                    .Include(x=>x.Contractor)
                    .ToListAsync().ConfigureAwait(false);

                if (!IsAdminUser(roleName))
                {
                    materialList.Where(x => x.CreatedBy.Equals(userName));
                }

                materialList.ForEach(material =>
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);

                    materialDtoList.Add(materialDto);
                });

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(message: ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method End");

            return materialDtoList;
        }
        public async Task<List<MaterialDto>> GetAllMaterialsByLocationAsync(Guid locationId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.LocationId == locationId)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .Include(x => x.WorkPackage)
                    .Include(x => x.Measurement)
                    .Include(x => x.Contractor)
                    .ToListAsync().ConfigureAwait(false);
             

                materialList.ForEach(material =>
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);

                    materialDtoList.Add(materialDto);
                });

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(message: ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method End");

            return materialDtoList;
        }
        public async Task<List<MaterialDto>> GetAllMaterialsByDivisionAsync(Guid divisionId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsByDivisionAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.DivisionId == divisionId)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Measurement)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .Include(x => x.WorkPackage)
                    
                    .ToListAsync().ConfigureAwait(false);


                materialList.ForEach(material =>
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);

                    materialDtoList.Add(materialDto);
                });

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(message: ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsByDivisionAsync:Method End");

            return materialDtoList;
        }
        public async Task<List<MaterialDto>> GetAllMaterialsBySubDivisionAsync(Guid SubDivisionId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsBySubDivisionAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.SubDivisionId == SubDivisionId)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Measurement)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .Include(x => x.WorkPackage)

                    .ToListAsync().ConfigureAwait(false);


                materialList.ForEach(material =>
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);

                    materialDtoList.Add(materialDto);
                });

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(message: ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsBySubDivisionAsync:Method End");

            return materialDtoList;
        }

        public async Task<MaterialDto> GetMaterialByIdAsync(Guid id)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialByIdAsync:Method Start");
            MaterialDto materialDto = new MaterialDto();
            try
            {
                var material = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x=>x.Id==id)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .ThenInclude(x=>x.SubDivision)                    
                    .Include(x => x.Supplier)
                    
                    .Include(x=>x.WorkPackage)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                if (material.DivisionId.HasValue)
                {
                    materialDto.Division = await _materialServiceDependencies.DivisionService.GetDivision((Guid)material.DivisionId);
                }
                
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;

            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialByIdAsync:Method End");
            return materialDto;
        }
        public async Task<List<Guid>> GetAllMaterialIdsByBulkUploadIdAsync(Guid bulkUploadDetailId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsByBulkUploadIdAsync:Method Start");
            List<Guid> materialIdsList = new List<Guid>();
            try
            {

                materialIdsList = await _materialServiceInfrastructure.GenericRepository
                    .Query()
                    .Where(x => x.BuilkUploadDetailId == bulkUploadDetailId)
                    .Select(x=>x.Id)
                    .ToListAsync().ConfigureAwait(false);  

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsByBulkUploadIdAsync:Method End");
            return materialIdsList;
        }
        public async Task<MaterialDto> UpdateMaterialAsync(MaterialDto materialDto)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:UpdateMaterialAsync:Method Start");
            try
            {
                materialDto.UpdatedDate = DateTime.UtcNow;
                var material = _materialServiceInfrastructure.Mapper.Map<MaterialDto, Material>(materialDto);
                await _materialServiceInfrastructure.GenericRepository.Update(material);
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;

            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:UpdateMaterialAsync:Method End");
            return materialDto;
        }
        
        
        public async Task<List<MaterialAuditReportDto>> GetMaterialAuditReport()
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialAuditReport:Method Start");
            List<MaterialAuditReportDto> materialAuditReportListDto = new List<MaterialAuditReportDto>();
            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .ToListAsync().ConfigureAwait(false);

                if (!IsAdminUser(roleName))
                {
                    materialList.Where(x => x.CreatedBy.Equals(userName));
                }
                foreach (var material in materialList)
                {
                    var materialAuditReportDto = new MaterialAuditReportDto()
                    {
                        MaterialName = material.MaterialName,
                        MaterialCode = material.MaterialCode,
                        CreatedBy = material.CreatedBy,
                        CreatedOn = material.CreatedDate.ToString(),
                        UpdatedBy = material.UpdatedBy,
                        UpdatedOn = material.UpdatedDate.ToString()
                    };
                    materialAuditReportListDto.Add(materialAuditReportDto);
                }
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError("Error:" + ex.Message);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialAuditReport:Method End");
            return materialAuditReportListDto;
        }

        public async Task<MaterialDto> GetMaterialsByCodeAsync(string materialCode)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsByCodeAsync:Method Start");
            MaterialDto materialDto = new MaterialDto();
            try
            {
                var material = await _materialServiceInfrastructure.GenericRepository.Query().Where(x => x.MaterialCode == materialCode)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .ThenInclude(x => x.SubDivision)
                    .ThenInclude(x => x.Division)
                    .Include(x => x.Supplier)
                    .Include(x=>x.WorkPackage)
                    .Include(x=>x.Measurement)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

                materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsByCodeAsync:Method End");
            return materialDto;
        }
        public async Task<List<MaterialDto>> GetMaterialsAddedByCurrentUser()
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsAddedByUser:Method Start");
            List<MaterialDto> materialDto = new List<MaterialDto>();
            try
            {
                var materials = await _materialServiceInfrastructure.GenericRepository
                    .Query().Where(x => x.CreatedBy == userName)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .Include(x => x.WorkPackage)
                    .Include(x => x.Measurement)
                    .ToListAsync().ConfigureAwait(false);

                materialDto = _materialServiceInfrastructure.Mapper.Map<List<MaterialDto>>(materials);

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsAddedByUser:Method End");
            return materialDto;
        }
        public async Task<List<MaterialDto>> GetMaterialsBySystemAsync(string systemName, bool isRehabilitation)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsBySystemAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                List<Material> filteredMaterialList = [];
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.System == systemName && x.IsRehabilitation == isRehabilitation)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)
                    .ToListAsync().ConfigureAwait(false);

                if (!IsAdminUser(roleName))
                {
                    filteredMaterialList=materialList.Where(x => x.CreatedBy==userName).ToList();
                }
                else
                {
                    filteredMaterialList = materialList;
                }
                foreach (var material in filteredMaterialList)
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);
                   
                    materialDtoList.Add(materialDto);
                }
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsBySystemAsync:Method End");
            return materialDtoList;
        }

        public async Task<List<MaterialDto>> GetMaterialsWithFilterAsync(MaterialFilterDto materialFilterDto)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsWithFilterAsync:Method Start");
            List<MaterialDto> materialDtoList = new List<MaterialDto>();
            try
            {
                List<Material> filteredMaterialList = [];
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.WorkPackageId == materialFilterDto.WorkPackageId 
                    && x.System==materialFilterDto.System 
                    && x.IsRehabilitation==(materialFilterDto.KindOfMaterial=="Material"?false:true))
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .ThenInclude(x=>x.MaterialType)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .ThenInclude(x => x.SubDivision)
                    .ThenInclude(x => x.Division)
                    .Include(x => x.Supplier)
                    
                    .Include(x=>x.WorkPackage)
                    .ToListAsync().ConfigureAwait(false);

                if (!IsAdminUser(roleName))
                {
                    filteredMaterialList = materialList.Where(x => x.CreatedBy == userName).ToList();
                }
                else
                {
                    filteredMaterialList = materialList;
                }
                foreach (var material in filteredMaterialList)
                {
                    var materialDto = _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                    materialDto.MaterialStatus = Enum.GetName(typeof(MaterialStatus), material.MaterialStatus);
                    if (material.DivisionId != null)
                    {
                        materialDto.Division = await _materialServiceDependencies
                            .DivisionService.GetDivision((Guid)material.DivisionId);
                    }
                    if(material.SubDivisionId != null)
                    {
                        materialDto.SubDivision = await _materialServiceDependencies
                           .SubDivisionService.GetSubDivision((Guid)material.SubDivisionId);
                    }
                    materialDtoList.Add(materialDto);
                }
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetMaterialsWithFilterAsync:Method End");
            return materialDtoList;
        }

        private static bool IsAdminUser(string roleName)
        {
            UserTypes currentRole = (UserTypes)Enum.Parse(typeof(UserTypes), roleName);

            if (UserTypes.Admin == currentRole)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
