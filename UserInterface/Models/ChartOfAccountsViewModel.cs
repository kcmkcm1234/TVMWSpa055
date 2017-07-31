using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class ChartOfAccountsViewModel
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string TypeDesc { get; set; }
        public string OpeningPaymentMode { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime OpeningAsOfDate { get; set; }
        public bool ISEmploy { get; set; }
        public decimal Amount { get; set; }
        public string account { get; set; }
    }
}