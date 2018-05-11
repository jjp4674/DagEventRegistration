using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EventRegistration.Models
{
    public class PermissionGroupViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int GroupId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public List<PermissionGroupViewModel> PermissionGroups { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}