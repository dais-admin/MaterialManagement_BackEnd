

using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.MaterialTransferDto
{
    public class DivisionToSubDivisionMaterialTransferDto
    {
        public Guid? Id { get; set; }
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public List<DivisionToSubDivisionMaterialTransferItemDto> DivisionToSubDivisionMaterialTransferItems { get; set; }

    }
    public class DivisionToSubDivisionMaterialTransferItemDto
    {
        public Guid IssuingDivisionId { get; set; }
        public DivisionDto? IssuingDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid ReceivingSubDivisionId { get; set; }
        public SubDivisionDto? ReceivingSubDivision { get; set; }
        public Guid? OnBoardedDivisionId { get; set; }
        public DivisionDto? OnBoardedDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }
    public class DivisionToSubDivisionMaterialTransferApprovalDto
    {
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public DivisionDto? IssuingDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public SubDivisionDto? ReceivingSubDivision { get; set; }
        public DivisionDto? OnBoardedDivision { get; set; }
        public MaterialDto? Material { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
