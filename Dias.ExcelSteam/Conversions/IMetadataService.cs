using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Conversions
{
    public interface IMetadataService
    {
        Task<IEnumerable<ExcelReaderMetadata>> GetExcelReaderMetadataAsync();
        Task<int> UpdateExcelReaderMetadataAsync(Guid id, IDbTransaction? transaction);
        Task<int> AddAsync(Material material, IDbTransaction? transaction);
    }
}
