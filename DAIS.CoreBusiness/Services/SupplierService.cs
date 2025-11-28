using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class SupplierService : ISupplierService
    {
        private IGenericRepository<Supplier> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierService> _logger;
        private readonly IFileManagerService _fileManager;
        public SupplierService(IGenericRepository<Supplier> genericRepo, IMapper mapper, ILogger<SupplierService> logger, IFileManagerService fileManager)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        }
        public async Task<SupplierDto> AddSupplier(SupplierDto supplierDto)
        {
            _logger.LogInformation("SupplierService:AddSupplier:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(supplierDto.SupplierName))
                {
                    supplierDto.SupplierName = supplierDto.SupplierName.ToUpper();
                }
                var supplier =_mapper.Map<Supplier>(supplierDto);
                await _genericRepo.Add(supplier);
               
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SupplierService:AddSupplier:Method End");
            return supplierDto;
        }

        public async Task DeleteSupplier(Guid id)
        {
            _logger.LogInformation("SupplierService:DeleteSupplier:Method Start");

            try
            {
                var supplier = await _genericRepo.GetById(id);

                if (supplier == null)
                {
                    _logger.LogWarning($"Supplier with id {id} not found.");
                    return;
                }

                // Delete associated files/folders if any
                if (!string.IsNullOrWhiteSpace(supplier.SupplierDocument))
                {
                    var files = supplier.SupplierDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);  // Use your FileManager service
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Remove DB entry
                await _genericRepo.Remove(supplier);

                _logger.LogInformation("SupplierService:DeleteSupplier:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteSupplier");
                throw;
            }
        }


        public async Task<List<SupplierDto>> GetAllSupplier()
        {
            _logger.LogInformation("SupplierService:GetAllSupplier:Method Start");
            List<SupplierDto> supplierDtoList = new List<SupplierDto>();
            try
            {
                var supplierList = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).ToListAsync().ConfigureAwait(false);

                supplierDtoList.AddRange(_mapper.Map<List<SupplierDto>>(supplierList));        
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SupplierService:GetAllSupplier:Method End");
            return supplierDtoList;
            
        }

        public async Task<SupplierDto> GetSupplier(Guid id)
        {
            _logger.LogInformation("SupplierService:GetSupplier:Method Start");
            SupplierDto supplierDto = new SupplierDto();
            try
            {
                var supplier = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                supplierDto = _mapper.Map<SupplierDto>(supplier);
                
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SupplierService:GetSupplier:Method End");
            return supplierDto;
        }

        public async Task<SupplierDto> UpdateSupplier(SupplierDto supplierDto)
        {
            _logger.LogInformation("SupplierService:UpdateSupplier:Method Start");
            try
            {
                var existingSupplier = await _genericRepo.GetById(supplierDto.Id);
                if (existingSupplier != null)
                {
                    if (supplierDto.SupplierDocument != null)
                    {
                        existingSupplier.SupplierDocument = supplierDto.SupplierDocument;
                    }
                    existingSupplier.UpdatedDate = DateTime.Now;
                    existingSupplier.SupplierName = supplierDto.SupplierName;
                    existingSupplier.SupplierAddress = supplierDto.SupplierAddress;
                    existingSupplier.ProductsDetails = supplierDto.ProductsDetails;
                    existingSupplier.Remarks = supplierDto.Remarks;
                    existingSupplier.ContactNo = supplierDto.ContactNo;
                    existingSupplier.ContactEmail = supplierDto.ContactEmail;
                }

                await _genericRepo.Update(existingSupplier);
            }
               
           
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SupplierService:UpdateSupplier:Method End");
            return supplierDto;
        }

        public SupplierDto GetSupplierIdByName(string name)
        {
            _logger.LogInformation("SupplierService:GetSupplierIdByName:Method Start");
            SupplierDto supplierDto = new SupplierDto();
            try
            {
                var supplier =  _genericRepo.Query()
                 .FirstOrDefault(x => x.SupplierName==name);
                supplierDto = _mapper.Map<SupplierDto>(supplier);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SupplierService:GetSupplierIdByName:Method End");
            return supplierDto;
        }
    }
}
