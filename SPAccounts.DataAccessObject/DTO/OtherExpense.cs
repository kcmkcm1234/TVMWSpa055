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
        public ChartOfAccounts chartOfAccounts { get; set; }
        public string PaidFromCompanyCode { get; set; }
        public Companies companies { get; set; }
        public Employee employee { get; set; }
        public Guid EmpID { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithID { get; set; }
        public string EmpTypeCode { get; set; }
        public DepositAndWithdrawals depositAndWithdrwal { get; set; }
        public string BankCode { get; set; }
        public string ExpenseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Common commonObj { get; set; }

    }
}