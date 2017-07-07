using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class HomeViewModel
    {
        public Guid ID { get; set; }
        public string LinkName { get; set; }
        public string LinkDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
      
    }
}