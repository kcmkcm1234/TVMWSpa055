﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class SystemReportViewModel
    {
        public Guid AppID { get; set; }
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
    }
    public class SaleSummaryViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string CustomerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal NetDue { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string OriginCompany { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }
    public class SaleDetailReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string OriginCompany { get; set; }
        public string GeneralNotes { get; set; }
        public string CustomerName { get; set; }
        public decimal Credit { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;

    }

    public class OtherExpenseSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string AccountHeadORSubtype { get; set; }
        public string SubTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

   
    public class OtherExpenseDetailsReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string AccountHead { get; set; }
        public string SubType { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentReference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string OriginCompany { get; set; }
        public string Company { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

    public class CustomerContactDetailsReportViewModel
    {
      
        public string CompanyCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
     
    }

    public class SalesTransactionLogReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }
    public class PurchaseSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string SupplierName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Credit { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public decimal NetDue { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
        public string OriginCompany { get; set; }

    }

    public class PurchaseDetailReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string OriginCompany { get; set; }
        public string GeneralNotes { get; set; }
        public string SupplierName { get; set; }
        public decimal Credit { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;

    }

    public class SupplierContactDetailsReportViewModel
    {

        public string CompanyCode { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string OtherPhoneNos { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

    }

    public class PurchaseTransactionLogReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string DocNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

    public class AccountsReceivableAgeingReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string TransactionDate { get; set; }
        public string DocNo { get; set; }
        public string CustomerName { get; set; }
        public string DueDate { get; set; }
        public string DaysPastDue { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public string Group { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

    public class AccountsReceivableAgeingSummaryReportViewModel
    {
        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
        public string CompanyCode { get; set; }
        public string OriginatedCompany { get; set; }
        public string Customer { get; set; }
        public int Current { get; set; }
        public int  OneToThirty { get; set; }
        public int ThirtyOneToSixty { get; set; }
        public int SixtyOneToNinety { get; set; }
        public int NinetyOneAndOver { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<CompaniesViewModel> companiesList;
    }

}
