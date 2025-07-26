namespace DAIS.CoreBusiness.Dtos
{
    public class ChartDataDto
    {
        public List<MaterialChartByStatus> materialChartByStatuses = new List<MaterialChartByStatus>();
        public List<MaterialChartByCategory> materialChartByCategories = new List<MaterialChartByCategory>();
        public List<MaterialInMaintenanceChart> materialInMaintenanceCharts = new List<MaterialInMaintenanceChart>();
    }
    public class MaterialChartByStatus
    {
        public string Status { get; set; }
        public int CountByStatus { get; set; }
    }
    public class MaterialChartByCategory
    {
        public string CategoryName { get; set; }
        public int CountByCategory { get; set; }
    }
    public class MaterialInMaintenanceChart
    {
        public string CategoryName { get; set; }
        public int InMaintenanceCount { get; set;}
    }
}
