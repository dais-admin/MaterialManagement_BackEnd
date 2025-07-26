using System.ComponentModel.DataAnnotations;

namespace DAIS.DataAccess.Helpers
{
    public enum MaterialStatus
    {
        Purchased,
        [Display(Name = "In Use")]
        InUse,
        UnderMaintanance,
        Defective,
        //Return,
        Expired,
        [Display(Name = "Required License")]
        RequiredLicense,
        //Update,
        //Miscellaneous
    }
}
