using System;

namespace DAIS.CoreBusiness.Dtos
{
    public class UpdateAlertStatusDto
    {
        public Guid MaintenanceId { get; set; }
        public int? AlertStatus { get; set; }
        public int? AlertPostponedDays { get; set; }
    }
}
