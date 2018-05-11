using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistration.Data.Models
{
    public class RolePermission
    {
        public Int64 RolePermissionId { get; set; }
        public Int64 RoleId { get; set; }
        public int PermissionId { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
