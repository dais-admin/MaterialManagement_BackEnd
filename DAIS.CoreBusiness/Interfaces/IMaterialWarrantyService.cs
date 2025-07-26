using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialWarrantyService
    {
        Task<MaterialWarrantyDto> AddWarrantyAsync(MaterialWarrantyDto materialWarrantyDto);
        Task<MaterialWarrantyDto> UpdateWarrantyAsync(MaterialWarrantyDto materialWarrantyDto);

        Task DeleteWarrantyAsync(Guid id);
        Task<MaterialWarrantyDto> GetWarrantyByIdAsync(Guid id);
        Task<List<MaterialWarrantyDto>> GetAllMaterialWarranty();
        Task<MaterialWarrantyDto> GetWarrantyByMaterialIdAsync(Guid materialId);
        Task<List<MaterialWarrantyDto>> GetMaterialWarrantyWithFilter(MaterialFilterDto materialFilterDto);


    }
}
