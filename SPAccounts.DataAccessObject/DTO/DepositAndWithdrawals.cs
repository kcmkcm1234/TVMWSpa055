using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class DepositAndWithdrawals
    {
        public Guid ID { get; set; }
        public string TransactionType { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime ChequeClearDate { get; set; }
        public decimal Amount { get; set; }
        public string BankCode { get; set; }
        public string GeneralNotes { get; set; }
        public string DateFormatted { get; set; }
        public string ChequeFormatted { get; set; }
        public string AmountFormatted { get; set; }
        public string BankName { get; set; }
        public Common commonObj { get; set; }
        public string DepositRowValues { get; set; }
        public List<DepositAndWithdrawals> CheckedRows { get; set; }
        public string DepositMode { get; set; }
        public string ChequeStatus { get; set; }
        public string PaymentMode { get; set; }
        public string ReferenceBank { get; set; }
        public string UndepositedChequeCount { get; set; }
        public string ChequeDate { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerID { get; set; }
        public Guid TransferID { get; set; }
        public string FromBank { get; set;}
        public string ToBank { get; set; }
        public string ChequeNo { get; set; }
        public OutGoingCheques OutGoingObj { get; set; }
        public Companies CompanyObj { get; set; }
        public Customer CustomerObj { get; set; }
        public UndepositedChequeAdvanceSearch UndepositedChequeAdvanceSearchObj { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public string Bank { get; set; }
        public Bank BankObj { get; set; }
        public string Search { get; set; }
    }

    public class OutGoingCheques
    {
        public Guid ID { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public string Bank { get; set; }
        public string Party { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string Remarks { get; set; }
        public string CreatedDate { get; set; }
        public string ChequeFormatted { get; set; }
        public Common commonObj { get; set; }
    }
    public class OutgoingChequeAdvanceSearch
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Search { get; set; }       
    }

    public class UndepositedChequeAdvanceSearch
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public string BankCode { get; set; }
        public string Search { get; set; }
    }

}