
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.MaterialTransferDto
{
    public class SubDivisionToDivisionMaterialTransferDto
    {
        public Guid? Id { get; set; }
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public List<SubDivisionToDivisionMaterialTransferItemDto> SubDivisionToDivisionMaterialTransferItems { get; set; }
    }
    public class SubDivisionToDivisionMaterialTransferItemDto
    {
        public Guid IssueSubDivisionId { get; set; }
        public SubDivisionDto? IssueSubDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid RecieveDivisionId { get; set; }
        public DivisionDto? RecieveDivision { get; set; }
        public Guid? OnBoardedSubDivisionId { get; set; }
        public SubDivisionDto? OnBoardedSubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }
    public class SubDivisionToDivisionMaterialTransferApprovalDto
    {
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public SubDivisionDto? IssuingSubDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public DivisionDto? ReceivingDivision { get; set; }
        public SubDivisionDto? OnBoardedSubDivision { get; set; }
        public MaterialDto? Material { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
