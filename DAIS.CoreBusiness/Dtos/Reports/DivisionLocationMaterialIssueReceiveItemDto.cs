using System;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class DivisionLocationMaterialIssueReceiveItemDto
    {
        public Guid IssuingDivisionId { get; set; }
        public DivisionDto issuingDivision { get; set; }
        public Guid ReceiveLocationId { get; set; }
        public LocationOperationDto ReceiveLocation { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public int RecievedQuantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int BalanceQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
    }
}
