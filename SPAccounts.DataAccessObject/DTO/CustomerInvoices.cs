using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{


    public class CustomerInvoice
    {
        public Guid ID { get; set; }
        public String InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public String PaymentDueDateFormatted { get; set; }
        public String BillingAddress { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxPercApplied { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public int DueDays { get; set; }
        public String Notes { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }

        public string hdfCustomerID { get; set; }
        public Guid hdnFileID { get; set; }

        public Companies companiesObj { get; set; }
        public Customer customerObj { get; set; }
        public PaymentTerms paymentTermsObj { get; set; }
        public TaxTypes TaxTypeObj { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal OtherPayments { get; set; }
        public CustomerPayments CustPaymentObj { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public String LastPaymentDateFormatted { get; set; }
        public String Status { get; set; }
        public Common commonObj { get; set; }
        public decimal TotalAmount { get; set; }

        public string InvoiceType { get; set; }
        public Guid? RefInvoice { get; set; }
        public string SpecialPayStatus { get; set; }
        public SpecialPayment SpecialPayObj { get; set; }

    }
    public class SpecialPayment
    {
        public Guid ID { get; set; }
        public decimal SpecialPaidAmount { get; set; }
        public string SpecialPaymentDate { get; set; }
        public string Remarks { get; set; }

        public string PaymentRef { get; set; }
        public string RefBank { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeDate { get; set; }

        public List<SpecialPayment> SpecialPaymentList { get; set; }

    }

    public class CustomerInvoiceSummary
    {
        public decimal OverdueAmount { get; set; }
        public string OverdueAmountFormatted { get; set; }
        public int OverdueInvoices { get; set; }

        public decimal OpenAmount { get; set; }
        public string OpenAmountFormatted { get; set; }
        public int OpenInvoices { get; set; }

        public decimal PaidAmount { get; set; }
        public string PaidAmountFormatted { get; set; }
        public int PaidInvoices { get; set; }

    }

    public class CustomerInvoicesSummaryForMobile
    {
        public InvoiceSummaryformobile CustInvSumObj { get; set; }
        public List<CustomerInvoice> CustInv { get; set; }
    }

    public class InvoiceSummaryformobile
    {
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public int count { get; set; }
       

    }
}