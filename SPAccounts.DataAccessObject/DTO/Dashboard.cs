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

        public string Caption { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalProfit { get; set; }

        public string FormattedTotalIncome { get; set; }
        public string FormattedTotalExpense { get; set; }
        public string FormattedTotalProfit { get; set; }

        public decimal IncomePercentage { get; set; }
        public decimal ExpensePercentage { get; set; }
        public decimal ProfitPercentage { get; set; }

    }
    public class MonthlyRecapItem
    {
        public string Period { get; set; }
        public decimal INAmount { get; set; }
        public decimal ExAmount { get; set; }

    }

    public class SalesSummary
    {
        public string Period { get; set; }
        public string Amount { get; set; }
        public string duration { get; set; }
    }
}