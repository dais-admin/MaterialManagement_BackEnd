namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class DivisionToSubDivisionMaterialIssueReceiveItemDto
    {
        public Guid IssuingDivisionId { get; set; }
        public DivisionDto IssuingDivision { get; set; }
        public Guid ReceiveSubDivisionId { get; set; }
        public SubDivisionDto ReceiveSubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public int RecievedQuantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int BalanceQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
    }
}
