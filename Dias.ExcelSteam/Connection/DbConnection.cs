using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Connection
{
    public class DbConnection(string connectionString) : ISqlDbConnection
    {
        public string DbConnetionString { get => connectionString; }

    }
}
