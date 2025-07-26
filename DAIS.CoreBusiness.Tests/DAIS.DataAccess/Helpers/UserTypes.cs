
using System.ComponentModel.DataAnnotations;

namespace DAIS.DataAccess.Helpers
{
    public enum UserTypes
    {
        Admin,
        Submitter,
        Reviewer,
        Approver,
        MaterialIssuer,
        MaterialReciever,
        Viewer,
        Supervisor
        
    }
}
