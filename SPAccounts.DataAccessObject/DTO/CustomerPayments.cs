using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class CustomerPayments
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public string RecdToComanyCode { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithdID { get; set; }
        public string BankCode { get; set; }
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        public string GeneralNotes { get; set; } 
        public decimal TotalRecdAmt  { get; set; }
        public decimal AdvanceAmount { get; set; }

        public string hdfCustomerID { get; set; }
        public string hdfCreditAmount { get; set; }
        public string PaymentDateFormatted { get; set; }
        public List<CustomerPaymentsDetail> CustomerPaymentsDetail { get; set; }
        public CustomerPaymentsDetail CustPaymentDetailObj { get; set; }
        public Customer customerObj { get; set; }
        public Common CommonObj { get; set; }
        public string DetailXml { get; set; }


    }
    public class CustomerPaymentsDetail
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidAmountEdit { get; set; }
        public string PaymentDueDateFormatted { get; set; }
       


    }
}