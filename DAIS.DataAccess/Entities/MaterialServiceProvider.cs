
using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class MaterialServiceProvider:BaseEntity
    {
        public string ServiceProviderName {  get; set; }
        public string? Address { get; set; }
        public string? ContactNo {  get; set; }
        public string? ContactEmail { get; set; }
        public string? Remarks { get; set; }
        public string? ServiceProviderDocument {  get; set; }

        [ForeignKey("ManufacturerId")]
        public Guid? ManufacturerId {  get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }

        [ForeignKey("ContractorId")]
        public Guid? ContractorId { get; set; }
        public virtual Contractor? Contractor { get; set; }

        public virtual ICollection<MaterialMaintenance> MaterialMaintenance { get; set; }
    }
}
