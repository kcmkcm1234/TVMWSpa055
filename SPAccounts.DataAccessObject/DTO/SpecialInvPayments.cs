using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SpecialInvPayments
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid CustID { get; set; }
        public Guid GroupID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentRef { get; set; }
        public string RefBank { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Remarks { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string PaymentDueDate { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public Companies companiesObj { get; set; }
        public Customer customerObj { get; set; }
        public Common commonObj { get; set; }
        public string DetailXml { get; set; }
        public string hdfpaymentDetail { get; set; }
        public string hdfCustomerID { get; set; }
        public string hdfPaymentID { get; set; }
        //   public decimal CurrentAmount { get; set; }
        public string paymentDateFormatted { get; set; }
        public string chequeDateFormatted { get; set; }
        public PaymentModes PaymentModesObj { get; set; }
        public SpecialInvPaymentsDetail specialDetailObj { get; set; }
        public List<SpecialInvPaymentsDetail> specialList { get; set; }
        public string InvoiceOutstanding { get; set; }
        public string BalanceOutstanding { get; set; }
        public string Search { get; set; }
    }
    public class SpecialInvPaymentsDetail
    {
       
        public decimal PaidAmount { get; set; }
        public Guid InvoiceID { get; set; }
        public decimal CurrentAmount { get; set; }
        //  public List<SpecialInvPaymentsDetail> specialList { get; set; }

    }
    public class SpecialPaymentsSearch
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string PaymentMode { get; set; }
        public string Customer { get; set; }
        public string Search { get; set; }
    }
}