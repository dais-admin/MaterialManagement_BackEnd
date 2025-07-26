using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public  interface IMaterialHardwareService
    {
        Task<MaterialHardwareDto>AddMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto);
        Task<MaterialHardwareDto> UpdateMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto);
        Task DeleteMaterialHardwareAsync(Guid Id);
        Task<MaterialHardwareDto> GetMaterialHardwareByIdAsync(Guid id);
        Task<List<MaterialHardwareDto>>GetAllMaterialHardware();
        Task<List<MaterialHardwareDto>> GetAllMaterialHardwaresByMaterialId(Guid materialId);

    }
}
