using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialAuditService
    {
        Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByUserAsync(string userId);
        Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByMaterialCodeAsync(string materialCode);
        Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByFilterAsync(MaterialFilterDto auditFilterDto);
    }
}
