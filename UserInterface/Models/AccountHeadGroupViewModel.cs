using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class AccountHeadGroupViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        public string AccountHead { get; set; }
        [Display(Name = "Account Head")]
        public string AccountHeads { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }
        public CommonViewModel commonObj { get; set; }
    }

    public class AccountHeadGroupDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid GroupID { get; set; }
        public string AccountHeadCode { get; set; }
    }

}