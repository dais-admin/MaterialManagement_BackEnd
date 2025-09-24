using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class DivisionLocationMaterialTransfer:BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid IssueDivisionId { get; set; }
        public virtual Division IssueDivision { get; set; }
        public Guid RecieveLocationId { get; set; }
        public virtual LocationOperation RecieveLocation { get; set; }
        public Guid? OnBoardedDivisionId { get; set; }
        public virtual Division OnBoardedDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }
        public virtual ICollection<DivisionLocationMaterialTransferTrancation> DivisionLocationMaterialTransferTrancations { get; set; }

    }
    public class DivisionLocationMaterialTransferTrancation : BaseEntity
    {
        public Guid LocationId { get; set; }
        public virtual LocationOperation Location { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }

        public Guid DivisionMaterialTransferId { get; set; }
        public virtual DivisionLocationMaterialTransfer DivisionLocationMaterialTransfer { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }
}
