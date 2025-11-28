using AutoMapper;
using DAIS.CoreBusiness.Dtos;
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
    public class ContractorService : IContractorService
    {
        private IGenericRepository<Contractor> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ContractorService> _logger;
        private readonly IFileManagerService _fileManager;
        public ContractorService(IGenericRepository<Contractor> genericRepo, IMapper mapper, ILogger<ContractorService> logger, IFileManagerService fileManager)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        }
        public async Task<ContractorDto> AddContractor(ContractorDto contractorDto)
        {
            _logger.LogInformation("ContractorService:AddContractor:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(contractorDto.ContractorName))
                {
                    contractorDto.ContractorName = contractorDto.ContractorName.ToUpper();
                }
                var contractor = _mapper.Map<Contractor>(contractorDto);
                await _genericRepo.Add(contractor);
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ContractorService:AddContractor:Method End");
            return contractorDto;
        }

        public async Task DeleteContractor(Guid id)
        {
            _logger.LogInformation("ContractorService:DeleteContractor:Method Start");

            try
            {
                var contractor = await _genericRepo.GetById(id);

                if (contractor == null)
                {
                    _logger.LogWarning($"Contractor with id {id} not found.");
                    return;
                }

                // Delete associated files/directories
                if (!string.IsNullOrWhiteSpace(contractor.ContractorDocument))
                {
                    var files = contractor.ContractorDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);   // Delegates file/folder delete
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Remove contractor from DB
                await _genericRepo.Remove(contractor);

                _logger.LogInformation("ContractorService:DeleteContractor:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteContractor");
                throw;
            }
        }


        public async Task<List<ContractorDto>> GetAllContractor()
        {
            _logger.LogInformation("ContractorService:GetAllContractor:Method Start");
            List<ContractorDto> contractorDtoList = new List<ContractorDto>();
            try
            {
                var contractorList = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).ToListAsync().ConfigureAwait(false);

                contractorDtoList.AddRange(_mapper.Map<List<ContractorDto>>(contractorList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ContractorService:GetAllContractor:Method End");
            return contractorDtoList;
        }

        public async Task<ContractorDto> GetContractor(Guid id)
        {
            _logger.LogInformation("ContractorService:GetContractor:Method Start");
            ContractorDto contractorDto = new ContractorDto();
            try
            {
                var contractor = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                contractorDto = _mapper.Map<ContractorDto>(contractor);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ContractorService:GetContractor:Method End");
            return contractorDto;
        }

        public ContractorDto GetContractorIdByName(string name)
        {
            _logger.LogInformation("ContractorService:GetContractorIdByName:Method Start");
            ContractorDto contractorDto = new ContractorDto();
            try
            {
                var contractor = _genericRepo.Query()
                 .FirstOrDefault(x => x.ContractorName == name);
                contractorDto = _mapper.Map<ContractorDto>(contractor);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ContractorService:GetContractorIdByName:Method End");
            return contractorDto;
        }

        public async Task<ContractorDto> UpdateContractor(ContractorDto contractorDto)
        {
            _logger.LogInformation("ContractorService:UpdateContractor:Method Start");
            try
            {
                var existingContractorDocument = await _genericRepo.GetById(contractorDto.Id);
                if(existingContractorDocument != null)
                {
                    if(!string.IsNullOrWhiteSpace(contractorDto.ContractorDocument))
                    {
                        existingContractorDocument.ContractorDocument =contractorDto.ContractorDocument;
                    }
                    existingContractorDocument.UpdatedDate =DateTime.Now;
                    existingContractorDocument.ContractorName = contractorDto.ContractorName;
                    existingContractorDocument.ContractorAddress = contractorDto.ContractorAddress;
                    existingContractorDocument.ContactNo = contractorDto.ContactNo;
                    existingContractorDocument.ProductsDetails = contractorDto.ProductsDetails;


                }
             
                await _genericRepo.Update(existingContractorDocument);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ContractorService:UpdateContractor:Method End");
            return contractorDto;
        }
    }
}
