using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.ManufacturersRepo
{
    public interface IManufacturersRepository
    {
        Task<Manufacturer> GetAsync(ManufacturerDto manufacturer, IDbTransaction? transaction);
    }
}

