using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.SupplierRepo
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetAsync(SupplierDto supplier, IDbTransaction? transaction);      
    }
}
