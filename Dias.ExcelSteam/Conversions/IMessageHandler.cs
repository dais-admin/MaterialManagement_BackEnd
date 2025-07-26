using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Conversions
{
    public interface IMessageHandler
    {
        Task<MaterialDto> DeserializeMessage(string message);
    }
}
