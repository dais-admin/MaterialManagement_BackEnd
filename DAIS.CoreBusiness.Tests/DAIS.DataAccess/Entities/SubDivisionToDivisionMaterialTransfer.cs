using DAIS.DataAccess.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class SubDivisionToDivisionMaterialTransfer:BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }

        [ForeignKey("IssueSubDivisionId")]
        public Guid IssueSubDivisionId { get; set; }
        public virtual SubDivision IssueSubDivision { get; set; }

        [ForeignKey("RecieveDivisionId")]
        public Guid RecieveDivisionId { get; set; }
        public virtual Division RecieveDivision { get; set; }

        [ForeignKey("OnBoardedSubDivisionId")]
        public Guid? OnBoardedSubDivisionId { get; set; }
        public virtual SubDivision OnBoardedSubDivision { get; set; }

        [ForeignKey("MaterialId")]
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }
        public virtual ICollection<SubDivisionToDivisionMaterialTransferTrancation> SubDivisionToDivisionMaterialTransferTrancations { get; set; }
    }
    public class SubDivisionToDivisionMaterialTransferTrancation : BaseEntity
    {
        [ForeignKey("SubDivisionId")]
        public Guid? SubDivisionId { get; set; }
        public virtual SubDivision SubDivision { get; set; }

        [ForeignKey("DivisionId")]
        public Guid? DivisionId { get; set; }
        public virtual Division Division { get; set; }

        [ForeignKey("MaterialId")]
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("SubDivToDivMaterialTransferId")]
        public Guid SubDivToDivMaterialTransferId { get; set; }
        public virtual SubDivisionToDivisionMaterialTransfer SubDivisionToDivisionMaterialTransfer { get; set; }


        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }

    public class SubDivisionToDivisionMaterialTransferApproval : BaseEntity
    {
        [ForeignKey("SubDivToDivMaterialTransferApprovalId")]
        public Guid SubDivToDivMaterialTransferApprovalId { get; set; }
        public SubDivisionToDivisionMaterialTransfer SubDivisionToDivisionMaterialTransfer { get; set; }
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
