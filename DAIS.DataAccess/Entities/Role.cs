using Microsoft.AspNetCore.Identity;

namespace DAIS.DataAccess.Entities
{
    public class Role : IdentityRole
    {
        public virtual ICollection<RoleFeature> RoleFeatures { get; set; }
    }


}
