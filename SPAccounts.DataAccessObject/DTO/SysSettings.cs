using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SysSettings
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; } 
        public Common CommonObj { get; set; }
    }
}