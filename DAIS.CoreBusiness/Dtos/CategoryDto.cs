using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Dtos
{
    public class CategoryDto
    {
        private string categoryName;
        public Guid Id { get; set; }

        public string CategoryName
        {
            get => categoryName;
            set => categoryName = value?.ToUpper();
        }
        public string CategoryCode { get; set; }
        public string? Remarks { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectDto? Project { get; set; }

        public string? MaterialTypeName { get; set; }
        public Guid? MaterialTypeId { get;  set; }
        public MaterialTypeDto? MaterialType { get; set; }
    }
}
