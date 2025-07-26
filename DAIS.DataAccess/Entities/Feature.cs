using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.DataAccess.Entities
{
    public class Feature: BaseEntity
    {
        public string Name { get; set; }
    }

    public class Permission :BaseEntity
    {
        public string Name { get; set; }
    }
    public class RoleFeature:BaseEntity
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }

        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; }

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
