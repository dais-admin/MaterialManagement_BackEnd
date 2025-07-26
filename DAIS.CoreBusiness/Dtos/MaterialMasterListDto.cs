using DAIS.CoreBusiness.Dtos.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialMasterListDto
    {
        public List<MaterialTypeDto> MaterialTypeList { get; set; }
        public List<CategoryDto> CategoryList { get; set; }
        public List<LocationOperationDto> LocationOperationList { get; set; }
        public List<RegionDto> RegionList { get; set; }
        public List<DivisionDto> DivisionList { get; set; }
        public List<ManufacturerDto> ManufacturerList { get; set; }
        public List<SupplierDto> SuppliersList { get; set; }
        public List<ProjectDto> ProjectsList { get; set; }
        public List<WorkPackageDto> WorkPackageList { get; set; }
        public List<ContractorDto> ContractorList { get; set; }
        public List<MaterialMeasuremetDto> MaterialMeasuremetList { get; set;}
        public string MaterialCode {  get; set; }
    }
}
