using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class ImportOtherExpenses
    {
        public Guid ID { get; set; }
        public Guid EmpID { get; set; }
        public string PaidFromCompanyCode { get; set; }

        public string ExpenseDate { get; set; }
        public string AccountCode { get; set; }
        public string Company { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpCompany { get; set; }
        public string PaymentMode { get; set; }
        public string ExpenseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public Guid DepWithdID { get; set; }
        public string ChequeDate { get; set; }
        public string BankCode { get; set; }
        public string ReferanceBank { get; set; }
        public string RefNo { get; set; }
        public string ReversalRef { get; set; }
        public string ChequeClearDate { get; set; }
        
        public string Error { get; set; }
        public int ErrorRow { get; set; }
        public ChartOfAccounts ChartOfAccountsObj { get; set; }
        public Companies CompaniesObj { get; set; }
        public Employee EmployeeObj { get; set; }
        public Common CommonObj { get; set; }
    }
}