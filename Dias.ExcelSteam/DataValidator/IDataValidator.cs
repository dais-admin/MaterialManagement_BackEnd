using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.DataValidator
{
    public interface IDataValidator<in T>
    {
        Task<bool> ValidateAsync(Guid id, T data);
    }
}
