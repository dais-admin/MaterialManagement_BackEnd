using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class SubDivisionToDivisionMaterialIssueReceiveItemDto
    {
        public Guid IssueReceiveSubDivisionId { get; set; }
        public SubDivisionDto IssueReceiveSubDivision { get; set; }
        public Guid ReceiveDivisionId { get; set; }
        public DivisionDto ReceiveDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int BalanceQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
    }
}
