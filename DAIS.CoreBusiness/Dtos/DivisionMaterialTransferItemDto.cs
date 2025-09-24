using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class DivisionMaterialTransferItemDto
    {
        public Guid IssuingDivisionId { get; set; }
        public DivisionDto? IssuingDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid ReceivingDivisionId { get; set; }
        public DivisionDto? ReceivingDivision { get; set; }
        public Guid? OnBoardedDivisionId { get; set; }
        public DivisionDto? OnBoardedDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }
}
