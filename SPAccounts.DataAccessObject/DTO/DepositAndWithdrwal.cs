using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class DepositAndWithdrwal
    {
        public Guid ID { get; set; }
        public string TransactionType{ get; set; }
        public string ReferenceNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string BankCode { get; set; }
        public string GeneralNotes { get; set; }
        public Common CommonObj { get; set; }
    }
}