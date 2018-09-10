using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Dashboard
    {
    }


    public class TopDocs {
        public string DocType { get; set; }
        public List<TopDocsItem> DocItems{ get; set; }

}

    public class TopDocsItem
    {
        public string DocNo { get; set; }
        public string Customer { get; set; }
        public decimal Value { get; set; }
        public string ValueFormatted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get; set; }
        public Guid ID { get; set; }
        public string URL { get; set; }
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
        public bool IsInternal { get; set; }
    }
    public class MonthlyRecapItem
    {
        public string Period { get; set; }
        public decimal INAmount { get; set; }
        public decimal ExAmount { get; set; }

    }
    public class MonthlySalesPurchase
    {
        public string Caption { get; set; }
        public List<MonthlySalesPurchaseItem> MonthlyItemList { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalProfit { get; set; }
        public string summarytype { get; set; }
        public bool IsInternal { get; set; }
    }
    public class MonthlySalesPurchaseItem
    {
        public string Period { get; set; }
        public decimal Sales { get; set; }
        public decimal Purchase { get; set; }

    }
    public class ExpenseSummary
    {
        public string CompanyName { get; set; }
        public OtherExpSummary OtherExpSummary { get; set; }
        int month { get; set; }
        int year { get; set; }
    }

    public class SalesSummary
    {
        public string Period { get; set; }
        public string Amount { get; set; }
        public string duration { get; set; }
        public Boolean IsinternalComp { get; set; }
    }
}