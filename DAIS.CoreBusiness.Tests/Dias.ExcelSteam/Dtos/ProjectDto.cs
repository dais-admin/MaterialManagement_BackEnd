using Aspose.Cells.Drawing;

namespace Dias.ExcelSteam.Dtos
{
    public class RootDto
    {
        public MaterialDto Asset { get; set; }
    }
    public class MaterialDto
    {
        public ProjectDto Project { get; set; }
        public MaterialTypeDto MaterialType { get; set; }
        public CategoryDto Category { get; set; }
        public WorkPackageDto WorkPackage { get; set; }
        public RegionDto Region { get; set; }
        public LocationOfOperationDto LocationOfOperation { get; set; }
        public DivisionDto Devision { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
        public SupplierDto Supplier { get; set; }
        public string System { get; set; }
        public string MaterialCode
        {
            get
            {
                return "Bulk " + MaterialName;
            }
        }

        public string MaterialName { get; set; }
        public string TagNumber { get; set; }
        public string LocationRFId { get; set; }
        public string ModelNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? YearOfInstallation { get; set; }
        public DateTime? CommissioningDate { get; set; }
        public DateTime? DesignLifeDate { get; set; }
        public DateTime? EndPeriodLifeDate { get; set; }

        public string MaterialStatus { get; set; }
        public string MaterialQty { get; set; }
        



    }
    public class ProjectDto
    {
        public string ProjectName { get; set; }

    }
    public class MaterialTypeDto
    {
        public string Name { get; set; }

    }
    public class CategoryDto
    {
        public string Name { get; set; }

    }

    public class RegionDto
    {
        public string Name { get; set; }

    }
    public class LocationOfOperationDto
    {
        public string Name { get; set; }

    }
    public class DivisionDto
    {
        public string Name { get; set; }

    }
    public class ManufacturerDto
    {
        public string Name { get; set; }
    }
    public class SupplierDto
    {
        public string Name { get; set; }
    }
    public class WorkPackageDto
    {
        public string WorkPackageName { get; set; }
    }




}
