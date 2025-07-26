using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.LocationOfOperation
{
    public interface ILocationOperationRepository
    {
        Task<LocationOperation> GetAsync(LocationOfOperationDto locationOf, IDbTransaction? transaction);
        Task<LocationOperation> InsertAsync(LocationOfOperationDto locationOf, Guid? projectId, IDbTransaction? transaction);
    }
}
