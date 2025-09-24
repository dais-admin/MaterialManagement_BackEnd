
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialDto
    {
        public Guid Id { get; set; }
        public string System { get; set; }
        public string? TagNumber { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialQty { get; set; }
        public string? LocationRFId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? CommissioningDate { get; set; }
        public DateTime? YearOfInstallation { get; set; }
        public DateTime? DesignLifeDate { get; set; }
        public DateTime? EndPeriodLifeDate { get; set; }
        public DateTime? DateOfSupply { get; set; }
        public string ModelNumber { get; set; }
        public string? MaterialStatus { get; set; }
        public string? Remarks { get; set; }
        public string? RehabilitationMaterialCode {  get; set; }
        public Guid? BuilkUploadDetailId { get; set; }

        public Guid? TypeId { get; set; }
        public  MaterialTypeDto? MaterialType { get; set; }

        public Guid? CategoryId { get; set; }
        public CategoryDto? Category { get; set; }

        public Guid? RegionId { get; set; }
        public RegionDto? Region { get; set; }
        public Guid? DivisionId { get; set; }
        public DivisionDto? Division { get; set; }
        public Guid? SubDivisionId { get; set; }
        public SubDivisionDto? SubDivision { get; set; }
        public Guid? LocationId { get; set; }      
        public LocationOperationDto? Location { get; set; }

        

        public Guid? ManufacturerId { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
        
        public Guid? SupplierId { get; set; }
        public SupplierDto? Supplier { get; set; }

        public string? MaterialImage {  get; set; }

        public bool? IsRehabilitation { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Guid WorkPackageId { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }

        public Guid? MeasurementId { get; set; }
        public MaterialMeasuremetDto? Measurement { get; set; }

        public Guid? ContractorId { get; set; }
        public  ContractorDto? ContractorName { get;  set; }
    }
}
