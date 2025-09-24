using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.MaterialTransferDto
{
    public class UserMaterialTransferDto
    {
        public List<DivisionMaterialTransferApprovalDto> DivisionMaterialTransfers { get; set; } = new List<DivisionMaterialTransferApprovalDto>();
        public List<LocationMaterialTransferDto> LocationMaterialTransfers { get; set; } = new List<LocationMaterialTransferDto>();
        public List<SubDivisionMaterialTransferApprovalDto> SubDivisionMaterialTransfers { get; set; } = new List<SubDivisionMaterialTransferApprovalDto>();
    }
}
