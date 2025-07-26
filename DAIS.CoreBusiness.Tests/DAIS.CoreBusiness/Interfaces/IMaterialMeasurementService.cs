using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialMeasurementService
    {
        Task<MaterialMeasuremetDto> GetMaterialMeasurement(Guid id);
        Task<MaterialMeasuremetDto> AddMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto);
        Task<MaterialMeasuremetDto> UpdateMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto);
        Task DeleteMeasurement(Guid id);
        Task<List<MaterialMeasuremetDto>> GetAllMaterialMeasurement();
        MaterialMeasuremetDto GetMeasuremetByName(string name);
    }
}
