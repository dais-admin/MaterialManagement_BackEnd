using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class DesignDocument:BaseEntity
    {
        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [ForeignKey("WorkPackageId")]
        public Guid WorkPackageId { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }
        public string DesignDocumentName { get; set; }
        public string DocumentFileName { get; set; }



    }
}
