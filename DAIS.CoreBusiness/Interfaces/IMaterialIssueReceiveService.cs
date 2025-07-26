using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialIssueReceiveService
    {
        Task<MaterialIssueReceiveDto> GetMaterialIssueReceive(Guid id);
        Task<MaterialIssueReceiveDto> GetMaterialIssueReceiveByVoucherNo(string voucherNo);
        Task<MaterialIssueReceiveResponseDto> AddMaterialIssueReceive(MaterialIssueReceiveDto materialIssueReceiveDto);
        Task<MaterialIssueReceiveDto> UpdateMaterialIssueReceive(MaterialIssueReceiveDto materialIssueReceiveDto);
        Task DeleteMaterialIssueReceive(Guid id);
        Task<List<MaterialIssueReceiveDto>> GetAllMaterialIssueReceive();
        Task<List<MaterialIssueReceiveDto>> GetMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate);
        Task<List<MaterialLocatoinIssueReceiveItemDto>> GetMaterialLocationIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
    }
}
