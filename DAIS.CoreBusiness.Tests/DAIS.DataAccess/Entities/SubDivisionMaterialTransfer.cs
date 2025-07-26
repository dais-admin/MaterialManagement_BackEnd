using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    
    public class SubDivisionMaterialTransfer : BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid IssueSubDivisionId { get; set; }
        public virtual SubDivision IssueSubDivision { get; set; }
        public Guid RecieveSubDivisionId { get; set; }
        public virtual SubDivision RecieveSubDivision { get; set; }
        public Guid? OnBoardedSubDivisionId { get; set; }
        public virtual SubDivision OnBoardedSubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }
        public virtual ICollection<SubDivisionMaterialTransferTransaction> SubDivisionMaterialTransferTrancations { get; set; }
    }

    public class SubDivisionMaterialTransferTransaction : BaseEntity
    {

        public Guid SubDivisionId { get; set; }
        public virtual SubDivision SubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }

        public Guid SubDivisionMaterialTransferId { get; set; }
        public virtual SubDivisionMaterialTransfer SubDivisionMaterialTransfer { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }
}
