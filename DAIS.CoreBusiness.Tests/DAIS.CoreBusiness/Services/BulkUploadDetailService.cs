using Aspose.Cells.Charts;
using AutoMapper;
using Castle.Core.Logging;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class BulkUploadDetailService : IBulkUploadDetailService
    {
        public IGenericRepository<BulkUploadDetail> _genericRepo;
        public IBulkUploadRepository<BulkUploadDetail> _bulkUploadRepository;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;
        public IMapper _mapper;
        public ILogger<BulkUploadDetailService> _logger;
        private string userName = string.Empty;
        private Guid projectId = Guid.Empty;
        private string roleName = string.Empty;
        public BulkUploadDetailService(IGenericRepository<BulkUploadDetail> genericRepo,
            IBulkUploadRepository<BulkUploadDetail> bulkUploadRepository,
             IMapper mapper, ILogger<BulkUploadDetailService> logger,
             MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _genericRepo= genericRepo;
            _bulkUploadRepository= bulkUploadRepository;
            _mapper = mapper;
            _logger = logger;
            _materialServiceInfrastructure = materialServiceInfrastructure;
            if (_materialServiceInfrastructure.HttpContextAccessor.HttpContext != null)
            {
                SetUserAndProject(_materialServiceInfrastructure.HttpContextAccessor.HttpContext.User);
            }
        }
        private void SetUserAndProject(ClaimsPrincipal user)
        {
            if (user != null)
            {
                userName = user.Claims.FirstOrDefault(x => x.Type == Claims.NameClaim).Value;
                roleName = user.Claims.FirstOrDefault(x => x.Type == Claims.RoleClaim).Value;
            }
        }
        public BulkUploadDetailsDto AddBulkUploadDetail(BulkUploadDetailsDto bulkUploadDetailsDto)
        {
            _logger.LogInformation("BulkUploadDetailService:AddBulkUploadDetail:Method Start");
            try
            {
                var bulkUploadDetail = _mapper.Map<BulkUploadDetail>(bulkUploadDetailsDto);
                var dbEntity= _bulkUploadRepository.Add(bulkUploadDetail);
                bulkUploadDetailsDto.Id = dbEntity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:AddBulkUploadDetail:Method End");
            return bulkUploadDetailsDto;
        }

        public async Task DeleteBulkUploadDetail(Guid id)
        {
            _logger.LogInformation("BulkUploadDetailService:DeleteBulkUploadDetail:Method Start");
            try
            {
                var bulkUploadDetail = await _genericRepo.GetById(id);
                await _genericRepo.Remove(bulkUploadDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:DeleteBulkUploadDetail:Method End");
            
        }

        public async Task<List<BulkUploadDetailsDto>> GetAllBulkUploadDetails()
        {
            _logger.LogInformation("BulkUploadDetailService:GetAllBulkUploadDetails:Method Start");
            List<BulkUploadDetailsDto> bulkUploadDetailDtoList=new List<BulkUploadDetailsDto>();
            try
            {
                var bulkUploadDetailList = await _genericRepo.Query()
                .ToListAsync().ConfigureAwait(false); ;
                bulkUploadDetailDtoList.AddRange(_mapper.Map<List<BulkUploadDetailsDto>>(bulkUploadDetailList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:GetAllBulkUploadDetails:Method End");
            return bulkUploadDetailDtoList;
        }

        public async Task<BulkUploadDetailsDto> GetBulkUploadDetailById(Guid id)
        {
            _logger.LogInformation("BulkUploadDetailService:GetBulkUploadDetailById:Method Start");
            BulkUploadDetailsDto bulkUploadDetailDto = new BulkUploadDetailsDto();
            try
            {
                var bulkUploadDetail = await _genericRepo.Query()
                .FirstOrDefaultAsync(x=>x.Id==id).ConfigureAwait(false); ;
                bulkUploadDetailDto=_mapper.Map<BulkUploadDetailsDto>(bulkUploadDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:GetBulkUploadDetailById:Method End");
            return bulkUploadDetailDto;
        }

        public async Task<BulkUploadDetailsDto> UpdateBulkUpload(BulkUploadDetailsDto bulkUploadDetailsDto)
        {
            _logger.LogInformation("BulkUploadDetailService:UpdateBulkUpload:Method Start");
            BulkUploadDetailsDto bulkUploadDetailDto = new BulkUploadDetailsDto();
            try
            {
                var bulkUploadDetail = await _genericRepo.Query()
                .FirstOrDefaultAsync(x => x.Id == bulkUploadDetailsDto.Id).ConfigureAwait(false);
                bulkUploadDetail.ApprovalStatus = bulkUploadDetailsDto.ApprovalStatus;
                bulkUploadDetail.Comment= bulkUploadDetailsDto.Comment;

                 await _genericRepo.Update(bulkUploadDetail);
                bulkUploadDetailDto =_mapper.Map<BulkUploadDetailsDto>(bulkUploadDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:UpdateBulkUpload:Method End");
            return bulkUploadDetailDto;
        }

        public List<BulkUploadDetailsDto> GetAllBulkUploadDetailsByUser(string userName)
        {
            _logger.LogInformation("BulkUploadDetailService:GetAllBulkUploadDetailsByUser:Method Start");
            List<BulkUploadDetailsDto> bulkUploadDetailDtoList = new List<BulkUploadDetailsDto>();
            try
            {
                var bulkUploadDetails = _bulkUploadRepository.Query()
                .Where(x => x.CreatedBy == userName)
                .ToList();
                bulkUploadDetailDtoList.AddRange( _mapper.Map<List<BulkUploadDetailsDto>>(bulkUploadDetails));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("BulkUploadDetailService:GetAllBulkUploadDetailsByUser:Method End");
            return bulkUploadDetailDtoList;
        }
    }
}
