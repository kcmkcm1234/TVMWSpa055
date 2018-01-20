using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class ChartOfAccounts
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string TypeDesc { get; set; }
        public string OpeningPaymentMode { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime OpeningAsOfDate { get; set; }
        public bool ISEmploy {get;set;}
        public bool IsReverse { get; set; }
        public bool IsPurchase { get; set; }
        public bool IsAvailLEReport { get; set; }
        public decimal Amount { get; set; }
        //public string account { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public int days { get; set; }
        public string isUpdate { get; set; }
        public Common commonObj { get; set; }
        public string AssignRowValues { get; set; }
        public List<ChartOfAccounts> CheckedRows { get; set; }
    }
}