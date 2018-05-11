using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistration.Data.Models
{
    public class PermissionGroup
    {
        [Key]
        public int GroupId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
