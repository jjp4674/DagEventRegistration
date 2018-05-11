using System.Data.Entity;
using EventRegistration.Data.Models;

namespace EventRegistration.Data.Context
{
    public class EventContext : DbContext
    {
        public EventContext()
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
    }
}
