using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class DocumentType:BaseEntity
    {
        public string DocumentName { get; set; }
        public string? Remarks { get; set; }
        public virtual ICollection<MaterialDocument> MaterialDocuments { get; set; }
    }
}
