using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.MaterialTransferDto
{
    public class SubDivisionMaterialTransferApprovalDto
    {
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public SubDivisionDto? IssuingSubDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public SubDivisionDto? ReceivingSubDivision { get; set; }
        public SubDivisionDto? OnBoardedSubDivision { get; set; }
        public MaterialDto? Material { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
