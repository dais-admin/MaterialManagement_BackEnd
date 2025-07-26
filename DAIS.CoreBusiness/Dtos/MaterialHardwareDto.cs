using DAIS.CoreBusiness.Dtos.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialHardwareDto
    {
        public Guid Id { get; set; }
        public string HarwareName { get; set; }
        public int SerialNo { get; set; }
        public string? Chipset { get; set; }
        public DateTime? DateOfManufacturer { get; set; }
        public string? NetworkDetails { get; set; }
        public string? DiskDetails { get; set; }
        public string BiosDetails { get; set; }
        public int Quantity { get; set; }
        public string? Remarks { get; set; }
        public string? HardwareDocument { get; set; }
        public Guid MaterialId { get; set; }
        public Guid? SupplierId { get; set; }
        public string? SupplierName {  get; set; }
        public Guid ManufacturerId { get; set; }
        
    }
}
