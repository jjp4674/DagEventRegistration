using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EventRegistration.Models
{
    public class PermissionViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int PermissionId { get; set; }

        public bool IsSelected { get; set; }

        public int GroupId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}