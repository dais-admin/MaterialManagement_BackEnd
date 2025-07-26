using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.RegionRepo
{
    public interface IRegionRepository
    {
        Task<Region> GetAsync(RegionDto region, IDbTransaction? transaction);
        Task<Region> InsertAsync(RegionDto Region, Guid? projectId, IDbTransaction? transaction);
    }
}
