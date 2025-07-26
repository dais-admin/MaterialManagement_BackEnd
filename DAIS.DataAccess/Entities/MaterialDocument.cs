using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class MaterialDocument:BaseEntity
    {
        public string DocumentFileName {  get; set; }      
        public string DocumentFilePath { get; set; }

        public Guid DocumentTypeId { get; set; }
        public virtual DocumentType? DocumentType { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material? Material { get; set; }
      
    }
}
