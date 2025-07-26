using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialMaintenanceService
    {
        
        Task<MaterialMaintenaceDto> AddMaterialMaintenanceAsync(MaterialMaintenaceDto materialMaintenaceDto);
        Task<MaterialMaintenaceDto> UpdateMaterialMaintenaceAsync(MaterialMaintenaceDto materialMaintenaceDto);
        Task DeleteMaterialMaintenaceAsync(Guid id);
        Task<MaterialMaintenaceDto> GetMaterialMaintenaceByIdAsync(Guid id);
        Task<List<MaterialMaintenaceDto>> GetAllMaterialMaintenacesAsync();
        Task<MaterialMaintenaceDto> GetMaintenanceByMaterialIdAsync(Guid materialId);
    }

}
