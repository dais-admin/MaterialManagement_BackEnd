using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialIssueReceiveItemDto
    {
        public Guid IssuingLocation { get; set; }
        public LocationOperationDto? IssuingLocationOperation { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid ReceivingLocation { get; set; }
        public LocationOperationDto? ReceivingLocationOperation { get; set; }
        public Guid? OnBoardedLocationId { get; set; }
        public LocationOperationDto? OnBoardedLocationOperation { get; set; }
        public Guid MaterialId {  get; set; }
        public MaterialDto? Material { get; set; }
    }  
}
