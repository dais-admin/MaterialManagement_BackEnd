using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.WorkPackageRepo
{
    public interface IWorkPackageRepository
    {
        Task<WorkPackage> GetAsync(WorkPackageDto workPackage, IDbTransaction? transaction);
    }
}
