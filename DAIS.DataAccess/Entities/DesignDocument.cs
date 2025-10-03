using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class DesignDocument:BaseEntity
    {
      
        public Guid ProjectId { get; set; }
        public Guid WorkPackageId { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }
        public string DesignDocumentName { get; set; }
        public string DocumentFileName { get; set; }



    }
}
