using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class DivisionMaterialTransfer:BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid IssueDivisionId { get; set; }
        public virtual Division IssueDivision { get; set; }
        public Guid RecieveDivisionId { get; set; }
        public virtual Division RecieveDivision { get; set; }
        public Guid? OnBoardedDivisionId { get; set; }
        public virtual Division OnBoardedDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }
        public virtual ICollection<DivisionMaterialTransferTrancation> DivisionMaterialTransferTrancations { get; set; }
    }

    public class DivisionMaterialTransferTrancation : BaseEntity
    {

        public Guid DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }

        public Guid DivisionMaterialTransferId { get; set; }
        public virtual DivisionMaterialTransfer DivisionMaterialTransfer { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }

    public class DivisionMaterialTransferApproval : BaseEntity
    {
        public Guid DivisionMaterialTransferId { get; set; }
        public DivisionMaterialTransfer DivisionMaterialTransfer { get; set; }
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
