
namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialMaintenaceDto
    {
        public Guid? Id { get; set; }
        public DateTime MaintenanceStartDate { get; set; }
        public DateTime MaintenanceEndDate { get; set; }

        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }

    }
}
