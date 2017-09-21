using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class ApprovalStatusViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> ApprovalStatusList { get; set; }
    }
}