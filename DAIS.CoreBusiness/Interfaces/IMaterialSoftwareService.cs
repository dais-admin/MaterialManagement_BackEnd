using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialSoftwareService 
    {
        Task<MaterialSoftwareDto> AddMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto);
        Task<MaterialSoftwareDto> UpdateMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto);
        Task DeleteMaterialSoftwareAsync(Guid Id);
        Task<MaterialSoftwareDto> GetMaterialSoftwareByIdAsync(Guid id);
        Task<List<MaterialSoftwareDto>> GetAllMaterialSoftware();
        Task<List<MaterialSoftwareDto>> GetSoftwareListByMaterialIdAsync(Guid materialId);
    }
}
