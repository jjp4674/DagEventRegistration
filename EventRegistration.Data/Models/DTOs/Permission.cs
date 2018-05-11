using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistration.Data.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual PermissionGroup PermissionGroup { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
