
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialTypeDto
    {
        private string typeName;
        private string typeCode;

        public Guid Id { get; set; }

        public string TypeName
        {
            get => typeName;
            set => typeName = value?.ToUpper();
        }

        public string TypeCode
        {
            get => typeCode;
            set => typeCode = value?.ToUpper();
        }

        public string? Remarks { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectDto? Project { get; set; }
    }

}
