using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class OtherExpense
    {
        public Guid ID { get; set; }
        public string ExpenseDate { get; set; }
        public string AccountCode { get; set; }
        public ChartOfAccounts chartOfAccountsObj { get; set; }
        public string PaidFromCompanyCode { get; set; }
        public Companies companies { get; set; }
        public Employee employee { get; set; }
        public Guid EmpID { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithID { get; set; }
        public string EmpTypeCode { get; set; }
        public string ReferenceBank { get; set; }
        public string ReferenceNo { get; set; }
        public DepositAndWithdrawals depositAndWithdrwal { get; set; }
        public string BankCode { get; set; }
        public string ExpenseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Common commonObj { get; set; }
        public string ChequeClearDate { get; set; }
        public string ChequeDate { get; set; }
        public string creditAmountFormatted { get; set; }
        public string RefNo { get; set; }
        public string ReversalRef { get; set; }
        public decimal ReversableAmount { get; set; }
        public SysSettings SysSettingsObj { get; set; }
        public int? ApprovalStatus { get; set; }
        public string ApprovalDate { get; set; }
        public bool? IsNotified { get; set; }
        public string OELimitOnEntry { get; set; }
        public string OpeningBank { get; set; }
        public string OpeningNCBank { get; set; }
        public string OpeningCash { get; set; }
        public string UndepositedCheque { get; set; }
        public bool IsReverse { get; set; }
        //For BankWise Balance//
        public string BankName { get; set; }
        public string TotalAmount { get; set; }
        public string UnClearedAmount { get; set; }
        public string UnderClearingAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class OtherExpSummary {

        public List<OtherExpSummaryItem> ItemsList { get; set; }
        public string Month { get; set; }
        public decimal Total { get; set; }
        public String TotalFormatted { get; set; }
    }
    public class OtherExpSummaryItem
    {
        public string Head { get; set; }
        public decimal Amount  { get; set; }
        public string AmountFormatted { get; set; }
        public string color { get; set; }

    }
}