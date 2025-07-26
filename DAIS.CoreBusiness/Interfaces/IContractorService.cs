using DAIS.CoreBusiness.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IContractorService
    {
        Task<ContractorDto> GetContractor(Guid id);
        Task<ContractorDto> AddContractor(ContractorDto contractorDto);
        Task<ContractorDto> UpdateContractor(ContractorDto contractorDto);
        Task DeleteContractor(Guid id);
        Task<List<ContractorDto>> GetAllContractor();
        ContractorDto GetContractorIdByName(string name);
    }
}
