using DAIS.CoreBusiness.Dtos;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface ISubDivisionService
    {
        Task<SubDivisionDto> GetSubDivision(Guid id);
        Task<SubDivisionDto> AddSubDivision(SubDivisionDto subDivisionDto);
        Task<SubDivisionDto> UpdateSubDivision(SubDivisionDto subDivisionDto);
        Task DeleteSubDivision(Guid id);
        Task<List<SubDivisionDto>> GetAllSubDivision();
        SubDivisionDto GetSubDivisionIdByName(string name);
        Task<List<SubDivisionDto>> GetAllSubDivisionsByDivision(Guid divisionId);
    }
}
