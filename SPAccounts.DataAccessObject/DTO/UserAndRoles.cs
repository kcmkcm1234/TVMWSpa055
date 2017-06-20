using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class User
    {
        
       
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string RoleList { get; set; }
        public string[] Roles { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public Role role { get; set; }
        
    }

    public class Role
    {
        public Guid ID { get; set; }
        public string RoleName { get; set; }
        
    }
    public class UserProfile
    {

        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
      
    }
}