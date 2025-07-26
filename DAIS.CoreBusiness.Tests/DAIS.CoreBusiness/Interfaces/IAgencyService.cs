using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IAgencyService
    {
        Task<AgencyDto> GetAgency(Guid id);
        Task<AgencyDto> AddAgency(AgencyDto agencyDto);
        Task<AgencyDto> UpdateAgency(AgencyDto agencyDto);
        Task DeleteAgency(Guid id);
        Task<List<AgencyDto>> GetAllAgency();
        AgencyDto GetAgencyIdByName(string name);
    }
}
