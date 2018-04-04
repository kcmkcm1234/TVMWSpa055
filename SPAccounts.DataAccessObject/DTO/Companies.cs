using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Companies
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public Common commonObj { get; set; }
        public Guid ApproverID { get; set; }
        public string LogoURL { get; set; }
      }
}