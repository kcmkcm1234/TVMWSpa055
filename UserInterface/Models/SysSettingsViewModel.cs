using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class SysSettingsViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public CommonViewModel CommonObj { get; set; }
    }
}