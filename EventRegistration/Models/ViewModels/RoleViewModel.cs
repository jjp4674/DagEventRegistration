using EventRegistration.Data.Context;
using EventRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace EventRegistration.Models
{
    public class RoleViewModel
    {
        private EventContext db = new EventContext();

        [Key]
        [HiddenInput(DisplayValue = false)]
        public Int64 RoleId { get; set; }

        public bool IsSelected { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public List<PermissionGroupViewModel> Permissions { get; set; }

        public RoleViewModel()
        {

        }

        public RoleViewModel(bool populatePermissions = true)
        {
            if (populatePermissions)
            {
                Permissions = GeneratePermissions(null);
            }
        }

        public RoleViewModel(Int64 roleId)
        {
            Role role = db.Roles.FirstOrDefault(r => r.RoleId == roleId);
            if (role != null)
            {
                RoleId = role.RoleId;
                Name = role.Name;
                Description = role.Description;
                Permissions = GeneratePermissions(role);
            }
            else
            {
                Permissions = new List<PermissionGroupViewModel>();
            }
        }

        private List<PermissionGroupViewModel> GeneratePermissions(Role role)
        {
            // Start with the top level permission groups (permission groups with no parent group id)
            return GetPermissionGroups(null, db.PermissionGroups.ToList(), db.Permissions.ToList(), role);
        }

        private List<PermissionGroupViewModel> GetPermissionGroups(int? parentId, List<PermissionGroup> permissionGroups, List<Permission> permissions, Role role)
        {
            List<PermissionGroupViewModel> permissionGroupVMs = new List<PermissionGroupViewModel>();

            // Get all the permission groups for the parent
            foreach (PermissionGroup pg in permissionGroups.Where(pg => pg.ParentId == parentId).ToList())
            {
                // Initialize the view model
                PermissionGroupViewModel newPG = new PermissionGroupViewModel
                {
                    GroupId = pg.GroupId,
                    Name = pg.Name,
                    Description = pg.Description,
                    ParentId = parentId
                };

                // Get the permissions for the group
                newPG.Permissions = GetPermissions(pg.GroupId, permissions, role);

                // Get the child groups
                newPG.PermissionGroups = GetPermissionGroups(pg.GroupId, permissionGroups, permissions, role);

                permissionGroupVMs.Add(newPG);
            }

            return permissionGroupVMs;
        }

        private List<PermissionViewModel> GetPermissions(int groupId, List<Permission> permissions, Role role)
        {
            List<PermissionViewModel> permissionVMs = new List<PermissionViewModel>();

            // Get all the permissions for the group
            foreach (Permission permission in permissions.Where(p => p.GroupId == groupId).ToList())
            {
                bool selected = false;

                // If the role isn't null and the permission is in the role, mark the permission view model as selected
                if (role != null &&
                    role.RolePermissions.Any(rp => rp.PermissionId == permission.PermissionId))
                {
                    selected = true;
                }

                // Initialize the view model
                PermissionViewModel pvm = new PermissionViewModel
                {
                    PermissionId = permission.PermissionId,
                    GroupId = groupId,
                    Name = permission.Name,
                    Description = permission.Description,
                    IsSelected = selected
                };

                // Add the view model to the list
                permissionVMs.Add(pvm);
            }

            return permissionVMs;
        }
    }
}