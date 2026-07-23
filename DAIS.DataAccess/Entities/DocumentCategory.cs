using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class DocumentCategory : BaseEntity
    {
        public string CategoryName { get; set; }
        public string? Remarks { get; set; }
    }
}
