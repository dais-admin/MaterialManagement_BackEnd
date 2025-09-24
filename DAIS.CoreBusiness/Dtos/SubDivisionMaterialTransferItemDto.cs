using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class SubDivisionMaterialTransferItemDto
    {
        public Guid IssuingSubDivisionId { get; set; }
        public SubDivisionDto? IssuingSubDivision { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid ReceivingSubDivisionId { get; set; }
        public SubDivisionDto? ReceivingSubDivision { get; set; }
        public Guid? OnBoardedSubDivisionId { get; set; }
        public SubDivisionDto? OnBoardedSubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }
}
