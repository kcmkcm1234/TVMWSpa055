using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Approver
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public string Company { get; set; }
        public string Date { get; set; }
        public string PaymentMode { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ApprovalDate { get; set; }
        public string GeneralNotes { get; set; }
    }
}