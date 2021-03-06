﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class DashboardViewModel
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
        public bool IsInternal { get; set; }
    }

    public class AdminDashboardViewModel
    {
        [Display(Name = "Include Internal Companies")]
        public bool IsInternal { get; set; }
    }

    public class TopDocsVewModel
    {
        public string DocType { get; set; }
        public List<TopDocsItemVewModel> DocItems { get; set; }       

    }

    public class TopDocsItemVewModel
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
        public bool IsInternal { get; set; }

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
        public bool IsInternal { get; set; }

    }
    public class MonthlyRecapItemViewModel
    {
        public string Period { get; set; }
        public decimal INAmount { get; set; }
        public decimal ExAmount { get; set; }
        public bool IsInternal { get; set; }


    }
    public class MonthlySalesPurchaseViewModel
    {
    
        public string Caption { get; set; }
        public List<MonthlySalesPurchaseItemViewModel> MonthlyItemList { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalProfit { get; set; }
        public string summarytype { get; set; }
        public bool IsInternal { get; set; }

    }
    public class MonthlySalesPurchaseItemViewModel
    {
        public string Period { get; set; }
        public decimal Sales { get; set; }
        public decimal Purchase { get; set; }

    }
    public class ExpenseSummaryViewModel
    {
        public string CompanyName { get; set; }
        public OtherExpSummaryViewModel OtherExpSummary { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool IsInternal { get; set; }
    }

    public class OutstandingSummaryViewModel
    {
        public string CompanyName { get; set; }
        public decimal OutstandingInv { get; set; }
        public decimal OuttandingPay { get; set; }

        public int invCount { get; set; }
        public int payCount { get; set; }

        public string OutstandingInvFormatted { get; set; }
        public string OuttandingPayFormatted { get; set; }
        public bool IsInternal { get; set; }

    }

    public class TopCustomersViewModel
    {
        public string CompanyName { get; set; }
        public TopDocsVewModel Docs { get; set; }
        public string BaseURL { get; set; }
        public bool IsInternal { get; set; }
    }

    public class TopSuppliersViewModel
    {
        public string CompanyName { get; set; }
        public TopDocsVewModel Docs { get; set; }
        public string BaseURL { get; set; }
        public bool IsInternal { get; set; }

    }


    public class RecentDocumentsViewModel
    {
        public string DOM { get; set; }
        public string DocType { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public string BaseURL { get; set; }
        public TopDocsVewModel Docs { get; set; }
        public bool IsInternal { get; set; }

    }

    //public class DocumentSummaryViewModel
    //{
    //    public Guid ID { get; set; }
    //    public string DocName { get; set; }
    //    public string CustName { get; set; }
    //    public decimal DocValue { get; set; }
    //    public string CreatedBy { get; set; }
    //    public string DocDate { get; set; }
    //    public string URL { get; set; }

    //}

    public class SalesSummaryViewModel
    {
        public string Period { get; set; }
        public string Amount { get; set; }
        public string duration { get; set; }
        public Boolean IsinternalComp { get; set; }
    }

    public class InvoiceAgeingSummary {

        public CustomerInvoiceAgeingSummaryViewModel CustInvAgeSummary { get; set; }
        public SupplierInvoiceAgeingSummaryViewModel SuppInvAgeSummary { get; set; }
    }
}


 
