using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Department
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Common commonObj { get; set; }
    }
}