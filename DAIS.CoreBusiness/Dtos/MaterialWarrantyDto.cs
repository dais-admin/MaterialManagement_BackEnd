using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialWarrantyDto
    {
        public Guid Id { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }

        public DateTime? DLPStartDate { get; set; }
        public DateTime? DLPEndDate { get; set; }
        public DateTime? LastRenewalDate { get; set; }
        public bool? IsExtended { get; set; }
        public int? NoOfMonths { get; set; }
        public Guid? ManufacturerId { get; set; }    
        public Guid MaterialId { get; set; }     
        public string? WarrantyDocument { get; set; }

        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public MaterialDto? Material { get; set; }
        
    }
}
