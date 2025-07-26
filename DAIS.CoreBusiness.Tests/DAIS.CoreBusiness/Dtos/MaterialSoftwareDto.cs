using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
   public class MaterialSoftwareDto
   {
        public Guid Id { get; set; }
        public string SoftwareName { get; set; }   
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SoftwareDocument { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid MaterialId { get; set; }
        public string? Remarks { get; set; }


    }
}
