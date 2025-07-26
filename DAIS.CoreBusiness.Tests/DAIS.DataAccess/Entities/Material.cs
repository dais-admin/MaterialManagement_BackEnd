
using DAIS.DataAccess.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class Material : BaseEntity
    {
        public string System { get; set; }
        public string TagNumber { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialQty { get; set; }
        public string? LocationRFId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? DateOfSupply { get; set; }
        public DateTime? CommissioningDate { get; set; }
        public DateTime? YearOfInstallation { get; set; }
        public DateTime? DesignLifeDate { get; set; }
        public DateTime? EndPeriodLifeDate { get; set; }
        public string ModelNumber { get; set; }
        public MaterialStatus MaterialStatus { get; set; }

        [ForeignKey("TypeId")]
        public Guid? TypeId { get; set; }
        public virtual MaterialType MaterialType { get; set; }

        [ForeignKey("CategoryId")]
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("RegionId")]
        public Guid? RegionId { get; set; }
        public virtual Region Region { get; set; }

        [ForeignKey("LocationId")]
        public Guid? LocationId { get; set; }
        public virtual LocationOperation Location { get; set; }

        [ForeignKey("ManufacturerId")]
        public Guid? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        [ForeignKey("SupplierId")]
        public Guid? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("ContractorId")]
        public Guid? ContractorId { get; set; }
        public virtual Contractor Contractor { get; set; }

        [ForeignKey("MeasurementId")]
        public Guid? MeasurementId { get; set; }
        public virtual MaterialMeasurement Measurement { get; set; }

        [ForeignKey("WorkPackageId")]
        public Guid WorkPackageId { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }

        [ForeignKey("BuilkUploadDetailId")]
        public Guid? BuilkUploadDetailId { get; set; }
        public virtual BulkUploadDetail BulkUploadDetail { get; set; }

        public Guid? DivisionId { get; set; }
        public Guid? SubDivisionId { get; set; }
        public string? Remarks {  get; set; }
        public string? MaterialImage { get; set; }
        public bool? IsRehabilitation { get; set; }
        public string? RehabilitationMaterialCode { get; set; }
       
        public virtual ICollection<MaterialApproval> MaterialApprovals {  get; set; }
        public virtual ICollection<MaterialDocument> MaterialDocuments { get; set; }
    }
}
