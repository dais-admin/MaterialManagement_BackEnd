
using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.DivisionRepo
{
    public interface IDivisionRepository
    {
        Task<Division> GetAsync(DivisionDto divisionDto, IDbTransaction? transaction);
    }
}
