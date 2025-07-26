using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDivisionService
    {
        Task<DivisionDto> GetDivision(Guid id);
        Task<DivisionDto>AddDivision(DivisionDto divisiondto);
        Task<DivisionDto>UpdateDivision(DivisionDto divisiondto);
        Task DeleteDivision(Guid id);
        Task<List<DivisionDto>> GetAllDivision();
        Task<List<DivisionDto>> GetDivisionsByLocation(Guid locationId);
        DivisionDto GetDivisionIdByName(string name);
    }
}
