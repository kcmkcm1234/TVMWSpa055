 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class DashboardViewModel
    {
    }

    public class AdminDashboardViewModel
    {
    }


    public class MonthlyRecapViewModel
    {
        public string CompanyName { get; set; }
        public List<MonthlyRecapItemViewModel> MonthlyRecapItemList { get; set; }

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
    public class MonthlyRecapItemViewModel
    {
        public string Period { get; set; }
        public decimal INAmount { get; set; }
        public decimal ExAmount { get; set; }
       

    }

    public class ExpenseSummaryViewModel
    {
        public string CompanyName { get; set; }
        public OtherExpSummaryViewModel OtherExpSummary { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class OutstandingSummaryViewModel
    {
        public string CompanyName { get; set; }

    }

    public class TopCustomersViewModel
    {
        public string CompanyName { get; set; }

    }

    public class TopSuppliersViewModel
    {
        public string CompanyName { get; set; }

    }


    public class RecentDocumentsViewModel
    {
        public string DOM { get; set; }
        public string DocType { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public List<DocumentSummaryViewModel> DocList { get; set; }
    }

    public class DocumentSummaryViewModel
    {
        public Guid ID { get; set; }
        public string DocName { get; set; }
        public string CustName { get; set; }
        public decimal DocValue { get; set; }
        public string CreatedBy { get; set; }
        public string DocDate { get; set; }
        public string URL { get; set; }

    }
}