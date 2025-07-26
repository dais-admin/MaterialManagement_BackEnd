using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Dtos
{
    public class LocationOperationDto
    {
        private string locationOperationName;
        private string locationOperationCode;

        public Guid Id { get; set; }

        public string LocationOperationName
        {
            get => locationOperationName;
            set => locationOperationName = value?.ToUpper();
        }

        public string LocationOperationCode
        {
            get => locationOperationCode;
            set => locationOperationCode = value?.ToUpper();
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string System { get; set; }
        public string? Remarks { get; set; }
        public Guid WorkPackageId { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }
        public Guid SubDivisionId { get; set; }
        public SubDivisionDto? SubDivision { get; set; }
    }

}
