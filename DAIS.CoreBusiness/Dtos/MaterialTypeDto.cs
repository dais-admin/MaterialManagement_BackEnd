
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialTypeDto
    {
        public Guid Id {  get; set; }
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
        public string? Remarks {  get; set; }
        public Guid ProjectId { get; set; }
        public ProjectDto? Project { get; set; }
    }
}
