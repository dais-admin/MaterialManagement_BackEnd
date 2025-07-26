using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class Contractor:BaseEntity
    {
        public string ContractorName { get; set; }
        public string? ContractorAddress { get; set; }
        public string? ProductsDetails { get; set; }
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? Remarks { get; set; }
        public string? ContractorDocument { get; set; }
        public Guid? MaterialTypeId { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Material> Materials { get; set; }

    }
}
