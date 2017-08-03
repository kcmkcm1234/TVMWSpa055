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
        public string GeneralNotes { get; set; }
        public decimal TotalPaidAmt { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string Type { get; set; }
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
        public string DetailXml { get; set; }
        public string OutstandingAmount { get; set; }
    }
    public class SupplierPaymentsDetail
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }
       // public decimal PaidAmountEdit { get; set; }
        public string PaymentDueDateFormatted { get; set; }

    }
}