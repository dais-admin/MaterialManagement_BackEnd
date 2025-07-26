
using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IChartService
    {
        Task<List<MaterialChartByCategory>> GetChartData(Guid workPackageId);
    }
}
