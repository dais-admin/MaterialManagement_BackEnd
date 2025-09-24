using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class SubDivisionMaterialTransferApproval : BaseEntity
    {
        public Guid SubDivisionMaterialTransferId { get; set; }
        public SubDivisionMaterialTransfer SubDivisionMaterialTransfer { get; set; }
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
