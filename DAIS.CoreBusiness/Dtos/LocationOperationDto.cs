using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class LocationOperationDto
    {
        public Guid Id { get; set; }
        public string LocationOperationName { get; set; }
        public string LocationOperationCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string System { get; set; }
        public string? Remarks {  get; set; }
        public Guid WorkPackageId { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }


    }
}
