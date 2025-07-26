
using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.WorkPackageRepo
{
    public class WorkPackageRepository(IDbConnection connection) : IWorkPackageRepository
    {
        public async Task<WorkPackage> GetAsync(WorkPackageDto workPackage, IDbTransaction? transaction)
        {
            var package = await connection.QueryFirstOrDefaultAsync<WorkPackage>(
            "SELECT * FROM WorkPackages WHERE WorkPackageName = @WorkPackageName",
            new { workPackage.WorkPackageName },
            transaction);
            return package!;
        }


    }
}
