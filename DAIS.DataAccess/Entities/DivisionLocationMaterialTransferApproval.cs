using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class DivisionLocationMaterialTransferApproval : BaseEntity
    {
        public Guid DivisionLocationMaterialTransferId { get; set; }
        public DivisionLocationMaterialTransfer DivisionLocationMaterialTransfer { get; set; }
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
