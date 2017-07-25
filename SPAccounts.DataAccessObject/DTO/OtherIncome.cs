using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class OtherIncome
    {
        public Guid ID { get; set; }
        public DateTime IncomeDate { get; set; }
        public string AccountCode { get; set; }
        public string PaymentRcdComanyCode { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepWithdID { get; set; }
        public string BankCode { get; set; }
        public string IncomeRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int slNo { get; set; }
        public string AccountDesc { get; set; }
        public string IncomeDateFormatted { get; set; }
        public Common commonObj { get; set; }
    }
}