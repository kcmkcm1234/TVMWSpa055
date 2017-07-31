using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Dashboard
    {
    }

    public class MonthlyRecap
    {
        public string CompanyName { get; set; }
        public List<MonthlyRecapItem> MonthlyRecapItemList { get; set; }

    }
    public class MonthlyRecapItem
    {
        public string Period { get; set; }
        public decimal INAmount { get; set; }
        public decimal ExAmount { get; set; }

    }


}