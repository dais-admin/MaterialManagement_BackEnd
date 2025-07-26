
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class MaterialLocatoinIssueReceiveItemDto
    {
        public Guid IssueRecieveLocation { get; set; }
        public LocationOperationDto? IssueRecieveLocationOperation { get; set; }
        public VoucherType VoucherType { get; set; }
        public string VoucherNo {  get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public int BalanceQuantity { get; set; }      
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
    }

    public class LocationReport
    {
        public Guid LocationId { get; set; }
        public LocationOperationDto? IssueRecieveLocationOperation { get; set; }
        public DateTime CreatedDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public int BalanceQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public MaterialDto? Material { get; set; }
    }
}
