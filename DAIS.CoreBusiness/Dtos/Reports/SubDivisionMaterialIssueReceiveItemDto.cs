using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class SubDivisionMaterialIssueReceiveItemDto
    {
        public Guid IssueReceiveSubDivisionId { get; set; }
        public SubDivisionDto? IssueReceiveSubDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public string VoucherNo { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public int BalanceQuantity { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }
}
