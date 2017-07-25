using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class ChartOfAccount
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string OpeningPaymentMode { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal OpeningAsOfDate { get; set; }
        public Common commonObj { get; set; }
    }
}