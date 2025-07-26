using DAIS.CoreBusiness.Dtos;
using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenarateToken(User user);
    }
}
