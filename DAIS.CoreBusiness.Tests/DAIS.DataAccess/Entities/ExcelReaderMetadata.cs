using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class ExcelReaderMetadata : BaseEntity
    {        

        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string FileException { get; set; }

    }
}
