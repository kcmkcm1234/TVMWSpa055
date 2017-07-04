using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class PrivilegesViewModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please select Role Name")]
        [Display(Name = "Role Name")]
        public Guid RoleID { get; set; }

        [Required(ErrorMessage = "Please select Application Name")]
        [Display(Name = "Application Name")]
        public Guid AppID { get; set; }

        [Required(ErrorMessage = "Please select Module Name")]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        
        [Display(Name = "Description")]
        public string AccessDescription { get; set; }

        public CommonViewModel commonDetails { get; set; }

        public string ApplicationName { get; set; }
        public string RoleName { get; set; }

        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> ApplicationList { get; set; }
    }
}