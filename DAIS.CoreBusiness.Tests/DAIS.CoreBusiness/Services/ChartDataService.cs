using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class ChartDataService: IChartService
    {
        private readonly MaterialServiceDependencies _materialServiceDependencies;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        public ChartDataService(MaterialServiceDependencies materialServiceDependencies, MaterialServiceInfrastructure materialServiceInfrastructure) 
        {
            _materialServiceDependencies = materialServiceDependencies;
            _materialServiceInfrastructure = materialServiceInfrastructure;
        }

        public async Task<List<MaterialChartByCategory>> GetChartData(Guid workPackageId)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method Start");
            ChartDataDto materialChartData = new ChartDataDto();
            List<MaterialInMaintenanceChart> materialInMaintenanceList = new List<MaterialInMaintenanceChart>();
            List<MaterialChartByCategory> materialChartByCategoryList = new List<MaterialChartByCategory>();

            try
            {
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.WorkPackageId == workPackageId)
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)                   
                    .Include(x => x.WorkPackage)
                    .ToListAsync().ConfigureAwait(false);

                
                var materialInMaintenance = from material in materialList
                                            where material.MaterialStatus == MaterialStatus.UnderMaintanance
                                            group material by material.MaterialStatus;
                var materialByCategories = from material in materialList
                                         group material by material.Category.CategoryName;

                foreach ( var material in materialInMaintenance )
                {
                    materialInMaintenanceList.Add(
                        new MaterialInMaintenanceChart()
                        {
                            CategoryName= material.Key.ToString(),
                            InMaintenanceCount=material.Count()
                        });
                }
                foreach (var material in materialByCategories)
                {
                    materialChartByCategoryList.Add(
                        new MaterialChartByCategory()
                        {
                            CategoryName=material.Key.ToString(),
                            CountByCategory=material.Count()
                        });
                }




                materialChartData.materialInMaintenanceCharts = materialInMaintenanceList;
                materialChartData.materialChartByStatuses = new List<MaterialChartByStatus>();
                materialChartData.materialChartByCategories = materialChartByCategoryList;

            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(message: ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialService:GetAllMaterialsAsync:Method End");

            return materialChartByCategoryList;
        }
    }
}
