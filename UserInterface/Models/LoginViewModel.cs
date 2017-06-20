using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter login name")]
        //[Display(Name = "Login Name")]
        [StringLength(250)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        //[Display(Name = "Password")]
        [StringLength(250)]
        public string Password { get; set; }
        
        public bool IsFailure { get; set; }
    }
}