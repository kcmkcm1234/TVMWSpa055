using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SupplierCreditNote
    {
        public Guid ID { get; set; }
        public Supplier supplier { get; set; }
        public Companies Company { get; set; }
        public string CreditToComanyCode { get; set; }
        public string CreditNoteDateFormatted { get; set; }
        public Guid SupplierID { get; set; }
        public string CRNRefNo { get; set; }
        public string CRNDate { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public string SupplierAddress { get; set; }
        public string CRNDateFormatted { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal adjustedAmount { get; set; }
        public string Type { get; set; }
        public string GeneralNotes { get; set; }
        public Common commonObj { get; set; }
    }
}