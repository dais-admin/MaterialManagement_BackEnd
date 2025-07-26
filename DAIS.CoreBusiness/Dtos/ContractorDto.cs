using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class ContractorDto
    {
        public Guid Id { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public string ProductsDetails { get; set; }
        public string Remarks { get; set; }
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContractorDocument { get; set; }
        public Guid? MaterialTypeId { get; set; }
        public Guid? CategoryId { get; set; }
        public MaterialTypeDto? MaterialType { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
