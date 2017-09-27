using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SupplierInvoices
    {
        public Guid ID { get; set; }
        public String InvoiceNo { get; set; }
        public string InvoiceType { get; set; }

        public DateTime InvoiceDate { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public String PaymentDueDateFormatted { get; set; }
        public String BillingAddress { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxPercApplied { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingCharge { get; set; }
        public int DueDays { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public String Notes { get; set; }
        public string CompanyCode { get; set; }
        public Companies companiesObj { get; set; }
        public Guid SupplierID { get; set; }
        public Supplier suppliersObj { get; set; }
        public string PayCode { get; set; }
        public PaymentTerms paymentTermsObj { get; set; }
        public string TaxCode { get; set; }
        public TaxTypes TaxTypeObj { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OtherPayments { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }

        public SupplierPayments SuppPaymentObj { get; set; }
        public Guid hdnFileID { get; set; }
        public decimal BalanceDue { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public String LastPaymentDateFormatted { get; set; }
        public String Status { get; set; }
        public Common commonObj { get; set; }
        public decimal PaymentProcessed { get; set; }
    }
    public class SupplierInvoiceSummary
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

        public string Approved { get; set; }
        public string NotApproved { get; set; }

    }


    public class SupplierSummaryforMobile
    {
        public SupplierInvoiceSummaryformobile supInvSumObj { get; set; }
        public List<SupplierInvoices> SupInv { get; set; }
    }

    public class SupplierInvoiceSummaryformobile
    {
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public int count { get; set; }

    }
}