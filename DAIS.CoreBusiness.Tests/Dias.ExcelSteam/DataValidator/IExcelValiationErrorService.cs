using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.DataValidator
{
    public interface IExcelValiationErrorService
    {
       Task LogValidationError(Guid id, string error);
    }
}
