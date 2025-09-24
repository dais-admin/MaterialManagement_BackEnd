﻿﻿﻿using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class DivisionToSubDivisionMaterialTransfer:BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid IssueDivisionId { get; set; }
        public virtual Division IssueDivision { get; set; }
        public Guid TargetSubDivisionId { get; set; }
        public virtual SubDivision TargetSubDivision { get; set; }
        public Guid? OnBoardedDivisionId { get; set; }
        public virtual Division OnBoardedDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }
        public virtual ICollection<DivisionToSubDivisionMaterialTransferTrancation> DivisionToSubDivisionMaterialTransferTrancation { get; set; }
    }
    public class DivisionToSubDivisionMaterialTransferTrancation : BaseEntity
    {

        public Guid? DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public Guid? SubDivisionId { get; set; }
        public virtual SubDivision SubDivision { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }

        public Guid DivisionToSubDivisionMaterialTransferId { get; set; }
        public virtual DivisionToSubDivisionMaterialTransfer DivisionToSubDivisionMaterialTransfer { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }

    public class DivisionToSubDivisionMaterialTransferApproval : BaseEntity
    {
        public Guid DivisionToSubDivisionMaterialTransferId { get; set; }
        public DivisionToSubDivisionMaterialTransfer DivisionToSubDivisionMaterialTransfer { get; set; }
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
