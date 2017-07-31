using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Bank
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CompanyCode { get; set; }
        public Common commonObj { get; set; }
        public Companies Company { get; set; }
        public string isUpdate { get; set; }        
    }
}