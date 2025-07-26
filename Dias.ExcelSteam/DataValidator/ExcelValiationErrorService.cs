using Dapper;
using Dias.ExcelSteam.Connection;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.DataValidator
{
    public class ExcelValiationErrorService(ISqlDbConnection dbConnection) : IExcelValiationErrorService
    {
        public async Task LogValidationError(Guid id, string error)
        {

            var sql = "UPDATE ExcelReaderMetadata SET IsRead = 0, IsDeleted=1, FileException = @Error WHERE Id = @Id";

            using (var connection = new SqlConnection(dbConnection.DbConnetionString))
            {
                await connection.OpenAsync();
                var parameters = new { Id = id, Error = error };
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
