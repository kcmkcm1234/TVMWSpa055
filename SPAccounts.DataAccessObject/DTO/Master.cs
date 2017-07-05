using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class PaymentTerms
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public Common commonObj { get; set; }
    }
    public class Companies
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public Common commonObj { get; set; }
    }

    public class TransactionTypes
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}