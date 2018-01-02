using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SupplierPayments
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public string PaidFromComanyCode { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithdID { get; set; }
        public string BankCode { get; set; }
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ChequeClearDate { get; set; }
        public string ChequeDate { get; set; }
        public string GeneralNotes { get; set; }
        public decimal TotalPaidAmt { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string ReferenceBank { get; set; }
        public string Type { get; set; }
        public Common commonObj { get; set; }
        public string Date { get; set; }

        public Guid CreditID { get; set; }
        public string CreditNo { get; set; }
        public string hdfType { get; set; }
        public string hdfCreditID { get; set; }
        public string hdfSupplierID { get; set; }
        public string hdfCreditAmount { get; set; }
        public string PaymentDateFormatted { get; set; }
        public List<SupplierPaymentsDetail> supplierPaymentsDetail { get; set; }
        public SupplierPaymentsDetail supplierPaymentsDetailObj { get; set; }
        public Supplier supplierObj { get; set; }
        public Common CommonObj { get; set; }
        public Companies CompanyObj { get; set; }
        public string DetailXml { get; set; }
        public Guid hdnFileID { get; set; }
        public string OutstandingAmount { get; set; }
        public string InvoiceOutstanding { get; set; }
        public string CreditOutstanding { get; set; }
        public string AdvOutstanding { get; set; }
        public string PaymentBooked { get; set; }
        public string PaymentOutstanding { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalDate { get; set; }
        public string IsNotificationSuccess { get; set; }

        public ApprovalStatus ApprovalStatusObj { get; set; }
        public string Search { get; set; }

        public decimal Amount { get; set; }
    }
    public class SupplierPaymentsDetail
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CreatedDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PrevPayment { get; set; }
        public decimal CurrPayment { get; set; }
        public decimal BalancePayment { get; set; }
        public string PaymentDueDate { get; set; }
        public string DueDays { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }
       // public decimal PaidAmountEdit { get; set; }
        public string PaymentDueDateFormatted { get; set; }

    }

    public class SupplierPaymentsAdvanceSearch
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string PaymentMode { get; set; }
        public string Supplier { get; set; }
        public string ApprovalStatus { get; set; }
        public string Search { get; set; }
    }

}