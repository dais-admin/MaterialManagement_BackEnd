using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.MaterialTransferDto
{
    public class DivisionMaterialTransferApprovalDto
    {
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public DivisionDto? IssuingDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public DivisionDto? ReceivingDivision { get; set; }
        public DivisionDto? OnBoardedDivision { get; set; }
        public MaterialDto? Material { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
