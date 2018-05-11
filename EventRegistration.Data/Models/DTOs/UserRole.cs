using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistration.Data.Models
{
    public class UserRole
    {
        public Int64 UserRoleId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 RoleId { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
