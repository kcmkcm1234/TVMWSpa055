using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class MenuViewModel
    {
        public Int16 ID { get; set; }
        public Int16 ParentID { get; set; }
        public string MenuText { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
        public string IconURL { get; set; }
        public string Parameter { get; set; }
    }
}