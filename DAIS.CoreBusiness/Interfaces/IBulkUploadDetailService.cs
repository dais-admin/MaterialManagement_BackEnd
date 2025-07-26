using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IBulkUploadDetailService
    {
        Task<BulkUploadDetailsDto> GetBulkUploadDetailById(Guid id);
        BulkUploadDetailsDto AddBulkUploadDetail(BulkUploadDetailsDto bulkUploadDetailsDto);
        Task<BulkUploadDetailsDto> UpdateBulkUpload(BulkUploadDetailsDto bulkUploadDetailsDto);
        Task DeleteBulkUploadDetail(Guid id);
        Task<List<BulkUploadDetailsDto>> GetAllBulkUploadDetails();
        List<BulkUploadDetailsDto> GetAllBulkUploadDetailsByUser(string userName);
    }
}
