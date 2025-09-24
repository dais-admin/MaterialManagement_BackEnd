using DAIS.DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class MaterialIssueRecieveVoucher : BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }
        public int OnBoardedQuantity { get; set; }
        public Guid IssueLocationId { get; set; }
        public virtual LocationOperation IssueLocation { get; set; }
        public Guid RecieveLocationId { get; set; }
        public virtual LocationOperation RecieveLocation { get; set; }
        public Guid? OnBoardedLocationId { get; set; }
        public virtual LocationOperation OnBoardedLocation { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int Stock { get; set; }

        public virtual ICollection<MaterialVoucherTransaction> MaterialVoucherTransactions { get; set; }
    }

    public class MaterialVoucherTransaction : BaseEntity
    {

        public Guid LocationId { get; set; }
        public virtual LocationOperation Location { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public VoucherType VoucherType { get; set; }
        public int Quantity { get; set; }

        public Guid MaterialIssueRecieveVoucherId { get; set; }
        public virtual MaterialIssueRecieveVoucher MaterialIssueRecieveVoucher { get; set; }
        public int IssuedQuantity { get; set; }
        public int RecievedQuantity { get; set; }

    }

    public class MaterialVoucherTransactionApproval: BaseEntity
    {
        public Guid MaterialIssueRecieveVoucherId {  get; set; }
        public MaterialIssueRecieveVoucher MaterialIssueRecieveVoucher {  get; set; }     
        public string IssuerId { get; set; }
        public virtual User Issuer { get; set; }
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
    
