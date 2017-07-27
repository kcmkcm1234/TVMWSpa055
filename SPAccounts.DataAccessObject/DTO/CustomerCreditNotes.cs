using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class CustomerCreditNotes
    {
        public Guid? ID { get; set; }
        public Guid? CustomerID { get; set; }
        public string OriginComanyCode { get; set; }
        public string CustomerName { get; set; }
        public string CreditNoteNo { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AvailableCredit  { get; set; }
        public string Type { get; set; }
        public string GeneralNotes { get; set; }
        public string CreditNoteDateFormatted { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public decimal adjustedAmount { get; set; }
        public string BillingAddress { get; set; }
        public Common commonObj { get; set; }
    }
}