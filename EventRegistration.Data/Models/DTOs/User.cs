using System;
using System.Collections.Generic;

namespace EventRegistration.Data.Models
{
    public class User
    {
        public Int64 UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordToken { get; set; }
        public string Email { get; set; }
        public string EmailToken { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Locked { get; set; }
        public DateTime? LockedDate { get; set; }
        public int FailedLogins { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}